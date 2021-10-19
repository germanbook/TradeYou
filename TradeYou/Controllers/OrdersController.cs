using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeYou.Models;

namespace TradeYou.Controllers
{
    public class OrdersController : Controller
    {
        private readonly TradeYouContext _context;
        private static int ShoppingCartProductQuantity;
        public OrdersController(TradeYouContext context)
        {
            _context = context;
        }

        // GET: Orders
        // Get orders for uers
        // get all orders for admin
        // normal user's own orders to himself

        public async Task<IActionResult> Index()
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            // ID is 0, need login before adding products to shopping cart
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            // Check account type
            int accountType = Convert.ToInt32(HttpContext.Session.GetInt32("Admin"));


            var tradeYouContext = (from o in _context.Orders
                                  where o.OOrderumber != null
                                  select o).Include(o => o.PIdNavigation).Include(o => o.UIdNavigation);

            // Normal user
            if (accountType == 0)
            {
                tradeYouContext = (from o in _context.Orders
                                   where o.OOrderumber != null && o.UId == tempUserID
                                   select o).Include(o => o.PIdNavigation).Include(o => o.UIdNavigation);
            }

            string previousOrderNumber;
            double orderPrice = 0;
            for (int i=0; i<tradeYouContext.ToList().Count(); i++)
            {
                
                // Start from second order record
                if (i>0)
                {
                    // Previous record's ordernumber
                    previousOrderNumber = tradeYouContext.ToList()[i-1].OOrderumber.ToString();
                    // If the same order
                    if (tradeYouContext.ToList()[i].OOrderumber.ToString() == previousOrderNumber)
                    {
                        // Set values to null
                        //tradeYouContext.ToList()[i].OOrderumber = null;
                        //tradeYouContext.ToList()[i].OPaymentype = null;
                        //tradeYouContext.ToList()[i].OShippingtype = null;
                    }
                    else
                    if (tradeYouContext.ToList()[i].OOrderumber.ToString() != previousOrderNumber)
                    {
                        // Generate order line and the order price
                        ViewData[Convert.ToString(tradeYouContext.ToList()[i - 1].OId)] = ("$"+ Convert.ToString(orderPrice));
                        orderPrice = 0;
                    }
 
                }
                // Every order record's price by quantity x unit price
                orderPrice += tradeYouContext.ToList()[i].OQuantity * tradeYouContext.ToList()[i].PIdNavigation.PPrice;
                if (i == (tradeYouContext.ToList().Count() - 1))
                {
                    // Generate order line and the order price
                    ViewData[Convert.ToString(tradeYouContext.ToList()[i].OId)] = ("$" + Convert.ToString(orderPrice));
                    orderPrice = 0;
                }
            }
            
            return View(await tradeYouContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.PIdNavigation)
                .Include(o => o.UIdNavigation)
                .FirstOrDefaultAsync(m => m.OId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["PId"] = new SelectList(_context.Products, "PId", "PMade");
            ViewData["UId"] = new SelectList(_context.Users, "UId", "UName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OId,UId,PId,OPaymentype,OShippingtype,OOrderumber,OQuantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PId"] = new SelectList(_context.Products, "PId", "PMade", order.PId);
            ViewData["UId"] = new SelectList(_context.Users, "UId", "UName", order.UId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["PId"] = new SelectList(_context.Products, "PId", "PMade", order.PId);
            ViewData["UId"] = new SelectList(_context.Users, "UId", "UName", order.UId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OId,UId,PId,OPaymentype,OShippingtype,OOrderumber,OQuantity")] Order order)
        {
            if (id != order.OId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PId"] = new SelectList(_context.Products, "PId", "PMade", order.PId);
            ViewData["UId"] = new SelectList(_context.Users, "UId", "UName", order.UId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.PIdNavigation)
                .Include(o => o.UIdNavigation)
                .FirstOrDefaultAsync(m => m.OId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            var sameOrder = _context.Orders.Where(o => o.OOrderumber == order.OOrderumber);

            if (sameOrder != null)
            {
                for (int i = 0; i < sameOrder.Count(); i++)
                {
                    _context.Orders.Remove(sameOrder.ToList()[i]);
                }
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OId == id);
        }

        // Shopping Cart
        // Bowen 25-09-2021
        // View shopping cart
        /**
         * Use order number to determine "order" and "shopping cart item". 
         * The unpaid "order" only exists in the shopping cart.
         * and shopping cart item will become the order after payment.
         */
        [HttpGet]
        public async Task<IActionResult> ShoppingCart()
        {
            ViewData["PProductname"] = new SelectList(_context.Products, "PId", "PProductname");
            ViewData["PImagePath"] = new SelectList(_context.Products, "PId", "PImagePath");
            ViewData["PPrice"] = new SelectList(_context.Products, "PId", "PPrice");

            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            // ID is 0, need login before adding products to shopping cart
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            var shoppingCarOrders = _context.Orders.FromSqlRaw("SELECT *" +
                "                                               FROM [ORDER]" +
                "                                               WHERE U_Id = " + tempUserID+
                "                                               AND O_Orderumber IS NULL")
                                                                .Include(o => o.PIdNavigation)
                                                                .Include(o => o.UIdNavigation).ToListAsync();            

            return View(await shoppingCarOrders);
        }

        // Shopping Cart Deletet
        // Bowen 26-09-2021
        // GET: Orders/Delete/5
        [HttpGet]
        public async Task<IActionResult> ShoppingCartDelete(int? id)
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.OId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // Shopping Cart Delete
        // Post
        /**
         * If delete an item in the shopping cart
         * the corresponding quantity of products 
         * will be returned to products stock
         */
        [HttpPost]
        public async Task<IActionResult> ShoppingCartDelete(int id)
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }
            var order = await _context.Orders.FindAsync(id);

            // Increase the product' quantity when delete the product from shopping cart
            var tempProducts = _context.Products.FromSqlRaw("SELECT *" +
                        "                                           FROM [PRODUCT]" +
                        "                                           WHERE P_Id = " + order.PId).ToList();
            tempProducts[0].PQuantity += order.OQuantity;

            Product tempProduct = tempProducts[0];
            _context.Update(tempProduct);
            await _context.SaveChangesAsync();


            // Delete

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            // Got how many item in shopping cart 
            var shoppingCartItemList = _context.Orders.FromSqlRaw("SELECT * " +
                "                                          FROM [ORDER] " +
                "                                          WHERE U_Id = " + HttpContext.Session.GetInt32("UserId") + " " +
                "                                          AND O_Orderumber IS NULL").ToArray();
            int shoppingCartCount = shoppingCartItemList.Count();
            HttpContext.Session.SetInt32("ShoppingCartCount", shoppingCartCount);

            return RedirectToAction("ShoppingCart");

        }

        // Shopping Cart edit product's quantity
        // Bowen 26-09-2021
        [HttpGet]
        public async Task<IActionResult> ShoppingCartQuantityEdit(int? id)
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["PId"] = new SelectList(_context.Products, "PId", "PMade", order.PId);
            ViewData["UId"] = new SelectList(_context.Users, "UId", "UName", order.UId);

            // Product quantity before edit
            ShoppingCartProductQuantity = order.OQuantity;

            return View(order);
        }

        // Shopping Cart edit product's quantity
        // Post
        /**
         * The change of item quantity in the shopping cart will affect the products stock.
         * If change the item quantity to 0, the item will be removed from shopping cart automatically.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShoppingCartQuantityEdit(int id, Order order)
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            if (id != order.OId)
            {
                return NotFound();
            }

            // Get products quantity
            //int productNumber = Convert.ToInt32(_context.Products.FromSqlRaw("SELECT P_Quantity " + " FROM [PRODUCT] " + " WHERE P_Id = " + order.PId).ToString());

            var tempProduct = await _context.Products.FirstOrDefaultAsync(m => m.PId == order.PId);
            int productQuantity = tempProduct.PQuantity;
            // 
            if (ModelState.IsValid)
            {
                // Product quantity increased 
                // check stock
                if (order.OQuantity > ShoppingCartProductQuantity)
                {
                    if ((order.OQuantity- ShoppingCartProductQuantity) > productQuantity)
                    {
                        ViewData["MQ"] = (Convert.ToString(productQuantity));
                        return View("ShoppingCartQuantityError");
                    }

                    int tempQuantity = order.OQuantity - ShoppingCartProductQuantity;
                    var tempProducts = _context.Products.FromSqlRaw("SELECT *" +
                        "                                           FROM [PRODUCT]" +
                        "                                           WHERE P_Id = " + order.PId).ToList();


                    //tempProducts[0].PQuantity += ShoppingCartProductQuantity - order.OQuantity;
                    tempProducts[0].PQuantity -= tempQuantity;


                    Product product = tempProducts[0];

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                
                if (order.OQuantity < ShoppingCartProductQuantity)
                {
                    int tempQuantity = ShoppingCartProductQuantity - order.OQuantity;
                    var tempProducts = _context.Products.FromSqlRaw("SELECT *" +
                        "                                           FROM [PRODUCT]" +
                        "                                           WHERE P_Id = "+order.PId).ToList();


                    //tempProducts[0].PQuantity += ShoppingCartProductQuantity - order.OQuantity;
                    tempProducts[0].PQuantity += tempQuantity;

                    // If change quantity to 0
                    if (order.OQuantity == 0)
                    {
                        // Delete
                        _context.Orders.Remove(order);
                        await _context.SaveChangesAsync();

                        // Got how many item in shopping cart 
                        var shoppingCartItemList = _context.Orders.FromSqlRaw("SELECT * " +
                            "                                          FROM [ORDER] " +
                            "                                          WHERE U_Id = " + HttpContext.Session.GetInt32("UserId") + " " +
                            "                                          AND O_Orderumber IS NULL").ToArray();
                        int shoppingCartCount = shoppingCartItemList.Count();
                        HttpContext.Session.SetInt32("ShoppingCartCount", shoppingCartCount);

                        return RedirectToAction("ShoppingCart");
                    }

                    Product product = tempProducts[0];

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }


                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ShoppingCart");
            }
            return View(order);
        }

        // Shopping Cart check out
        // Get
        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            // 
            var shoppingCarOrders = _context.Orders.FromSqlRaw("SELECT *" +
                "                                               FROM [ORDER]" +
                "                                               WHERE U_Id = " + tempUserID +
                "                                               AND O_Orderumber IS NULL").Include(o => o.PIdNavigation).ToList();
            //
            double totalOrderPrice = 0;
            for (int i=0; i<shoppingCarOrders.Count; i++)
            {
                totalOrderPrice += shoppingCarOrders[i].OQuantity * shoppingCarOrders[i].PIdNavigation.PPrice;
            }

            ViewData["TotalOrderPrice"] = new SelectList("TotalOrderPrice", totalOrderPrice);
            
            return View();
        }

        // Shopping Cart check out
        // Post
        /**
         * The order number will be generated when checking out.
         * items will become the order and show in the Orders module
         */
        [HttpPost]
        public async Task<IActionResult> CheckOut(int id, Order order, IFormCollection collection)
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            var shoppingCarOrders = _context.Orders.FromSqlRaw("SELECT *" +
                "                                               FROM [ORDER]" +
                "                                               WHERE U_Id = " + tempUserID +
                "                                               AND O_Orderumber IS NULL").ToList();
            DateTime orderNumber = DateTime.Now;

            int ss = Convert.ToInt32(order.OPaymentype);
            int tt = Convert.ToInt32(order.OShippingtype);


            // Update orders from shoppingcart to confirmed order
            for (int i = 0; i < shoppingCarOrders.Count; i++)
            {
                shoppingCarOrders[i].OOrderumber = orderNumber;
                shoppingCarOrders[i].OPaymentype = Convert.ToInt32(collection["paymentype"]);
                shoppingCarOrders[i].OShippingtype = Convert.ToInt32(collection["shippingtype"]);
                order = shoppingCarOrders[i];
                _context.Update(order);
                await _context.SaveChangesAsync();
            }

            // Empty the shopping cart after checkout
            HttpContext.Session.SetInt32("ShoppingCartCount", 0);

            return View("CheckOutSeccessful");
        }
    }
}
