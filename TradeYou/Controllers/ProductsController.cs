using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using TradeYou.Models;
using System.Diagnostics;

namespace TradeYou.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TradeYouContext _context;

        public ProductsController(TradeYouContext context)
        {
            _context = context;
        }

        // GET: Products
        // Get all products or searched products
        public async Task<IActionResult> Index(string searchString)
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            // ID is 0, need login before adding products to shopping cart
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            var products = from p in _context.Products
                            select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.PProductname.Contains(searchString));
            }

            return View(await products.ToListAsync());
        }

        // Sort A-Z
        // Sort products in ascending order
        // Bowen 29-09-2021
        [HttpGet]
        public async Task<IActionResult> SortAZ()
        {
            var products = from p in _context.Products
                           select p;

            products = products.OrderBy(n => n.PProductname);


            return View("Index", await products.ToListAsync());
        }

        // Sort Z-A
        // Sort products in descending order
        // Bowen 29-09-2021
        [HttpGet]
        public async Task<IActionResult> SortZA()
        {
            var products = from p in _context.Products
                           select p;

            products = products.OrderByDescending( n => n.PProductname);

            return View("Index", await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.PId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PId,PProductname,PPrice,PQuantity,PMade,PNewUsed,PImagePath")] Product product, IFormCollection collection)
        {
            
            if (ModelState.IsValid)
            {

                // Whether the http request contains a image or not
                // Upload image to "Image" folder
                // Save image path to product records as reference
                // Bowen 24-09-2021
                IFormFile image = Request.Form.Files.GetFile("PImagePath");
                if (image != null)
                {
                    String filePath = "ProductsImage/" + DateTime.Now.ToString("yymmssfff") + image.FileName;
                    FileStream imageStream = new FileStream("wwwroot/" + filePath, FileMode.Create);
                    image.CopyTo(imageStream);
                    imageStream.Close();
                    product.PImagePath = filePath;


                }
                else
                if (image == null)
                {
                    product.PImagePath = "ProductsImage/default.jpg";
                }
                //*************************************************


                product.PNewUsed = Convert.ToInt32(collection["newused"]);

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);


        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PId,PProductname,PPrice,PQuantity,PMade,PNewUsed,PImagePath")] Product product, IFormCollection collection)
        {
            if (id != product.PId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.PNewUsed = Convert.ToInt32(collection["newused"]);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.PId))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.PId == id);
            if (product == null)
            {
                return NotFound();
            }

            // Check if the products exists in any order
            var order = _context.Orders.FromSqlRaw("SELECT *" +
                "                                   FROM [ORDER]" +
                "                                   WHERE P_Id = "+product.PId).ToArray();

            // If products in any order, can not be deleted.
            if (order.Count() > 0)
            {
                return View("ProductDeleteError");
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.PId == id);
        }

        // Add product to shopping cart
        // Bowen 24-09-2021
        /**
         * Products added to the shopping cart 
         * will automatically reduce the products stock
         */
        [HttpPost]
        public async Task<IActionResult> AddShoppingCart(int id)
        {

            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            // ID is 0, need login before adding products to shopping cart
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            Order order = new Order();

            // User ID
            order.UId = tempUserID;
            // Product ID
            order.PId = id;
            
            
            // Check if the product exists in shopping cart for the sepicific user
            // And check it payed or not by O_Ordernumber
            var shoppingList = _context.Orders.FromSqlRaw("SELECT * " +
                "                                          FROM [ORDER] " +
                "                                          WHERE U_Id = "+order.UId+" " +
                "                                          AND P_Id = "+order.PId+" " +
                "                                          AND O_Orderumber IS NULL").ToArray();

            // If exist
            if (shoppingList.Length != 0)
            {

                order = shoppingList.First();
                order.OQuantity++;

                _context.Update(order);
                await _context.SaveChangesAsync();

               
                
            }
            else
            {
                // Quantity set to 1 by default
                order.OQuantity = 1;

                _context.Add(order);
                await _context.SaveChangesAsync();

            }

            Product tempProduct = await _context.Products.FindAsync(id);

            tempProduct.PQuantity--;

            _context.Update(tempProduct);
            await _context.SaveChangesAsync();

            // Got how many item in shopping cart 
            var shoppingCartItemList = _context.Orders.FromSqlRaw("SELECT * " +
                "                                          FROM [ORDER] " +
                "                                          WHERE U_Id = " + HttpContext.Session.GetInt32("UserId") + " " +
                "                                          AND O_Orderumber IS NULL").ToArray();
            int shoppingCartCount = shoppingCartItemList.Count();
            HttpContext.Session.SetInt32("ShoppingCartCount", shoppingCartCount);

            return View("AddingSucessful");

        }

        // View Details
        [HttpGet]
        public async Task<IActionResult> ViewDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.PId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // From View Details back to Home page
        [HttpGet]
        public ActionResult BackHome()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
