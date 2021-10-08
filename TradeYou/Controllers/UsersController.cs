using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeYou.Models;
using Microsoft.AspNetCore.Session;

namespace TradeYou.Controllers
{
    public class UsersController : Controller
    {
        private readonly TradeYouContext _context;

        public UsersController(TradeYouContext context)
        {
            _context = context;
        }

        // GET: Users
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

            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UId,UName,UPassword,UDob,UEmail,UIsAdmin")] User user, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                user.UIsAdmin = Convert.ToInt32(collection["isadmin"]);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            int accountType = Convert.ToInt32(HttpContext.Session.GetInt32("Admin"));
            // Check if the user is logged in
            // ID is 0, need login before adding products to shopping cart
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            if (accountType == 0)
            {
                return RedirectToAction("UserProfileEdit");
            }

            if (id == null)
            {
                id = tempUserID;
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UId,UName,UPassword,UDob,UEmail,UIsAdmin")] User user, IFormCollection collection)
        {
            if (id != user.UId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.UIsAdmin = Convert.ToInt32(collection["isadmin"]);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UId == id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if the user has any order
            var order = _context.Orders.FromSqlRaw("SELECT *" +
                "                                   FROM [ORDER]" +
                "                                   WHERE U_Id = " + user.UId).ToArray();

            // If products in any order, can not be deleted.
            if (order.Count() > 0)
            {
                return View("UserDeleteError");
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UId == id);
        }



        // User login
        // Bowen 24-09-2-21
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(IFormCollection collection)
        {
            for (int i=0; i<_context.Users.ToList().Count; i++)
            {
                // Check User Name
                if (_context.Users.ToList()[i].UName == collection["UName"])
                {
                    // Check User Password
                    if (_context.Users.ToList()[i].UPassword == collection["UPassword"])
                    {
                        // Admmin
                        //if (_context.Users.ToList()[i].UIsAdmin == 1)
                        //{
                            //HttpContext.Session.SetInt32("Admin",1);
                        //}

                        // Normal User
                        // Set User ID and Account type to normal user
                        HttpContext.Session.SetInt32("UserId", _context.Users.ToList()[i].UId);
                        HttpContext.Session.SetInt32("Admin", _context.Users.ToList()[i].UIsAdmin);
                        HttpContext.Session.SetString("UserName", _context.Users.ToList()[i].UName);
                        HttpContext.Session.SetInt32("Login", 1);

                        // Got how many item in shopping cart 
                        var shoppingCartItemList = _context.Orders.FromSqlRaw("SELECT * " +
                            "                                          FROM [ORDER] " +
                            "                                          WHERE U_Id = " + HttpContext.Session.GetInt32("UserId") + " " +
                            "                                          AND O_Orderumber IS NULL").ToArray();
                        int shoppingCartCount = shoppingCartItemList.Count();
                        HttpContext.Session.SetInt32("ShoppingCartCount", shoppingCartCount);

                        return View("LoginSuccessful");
                    }
                }

            }






            HttpContext.Session.SetInt32("Login", 0);
            return View("LoginError");

        }

        // User Profile
        [HttpGet]
        public async Task<IActionResult> UserProFileDetails()
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            // ID is 0, need login before adding products to shopping cart
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UId == tempUserID);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        [HttpGet]
        public async Task<IActionResult> UserProfileEdit()
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            // ID is 0, need login before adding products to shopping cart
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            var user = await _context.Users.FindAsync(tempUserID);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProfileEdit([Bind("UId,UName,UPassword,UDob,UEmail,UIsAdmin")] User user)
        {
            // Get User ID from Session
            int tempUserID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // Check if the user is logged in
            // ID is 0, need login before adding products to shopping cart
            if (tempUserID == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("UserProfileEditSuccessful");
            }
            return View(user);
        }

        // User logout
        // Bowen 29-09-2021
        [HttpGet]
        public ActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logout(IFormCollection collection)
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");

        }
    }
}
