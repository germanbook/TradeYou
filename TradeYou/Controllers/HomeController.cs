using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TradeYou.Models;

namespace TradeYou.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Products Database context
        // Bowen 24-09-2021
        private readonly TradeYouContext _context;

        public HomeController(ILogger<HomeController> logger, TradeYouContext context)
        {
            _logger = logger;
            _context = context;
        }


        // GET: Products
        // Get all products or searched products
        public async Task<IActionResult> Index(string searchString)
        {
            
            var products = from p in _context.Products
                           where p.PQuantity > 0
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.PProductname.Contains(searchString));
            }

            return View(await products.ToListAsync());
        }

        // Sort A-Z
        // Sort products in ascending order
        public async Task<IActionResult> SortAZ()
        {
            var products = (from p in _context.Products
                            where p.PQuantity > 0
                            select p).OrderBy(p => p.PProductname);

            return View("Index", await products.ToListAsync());
        }

        // Sort Z-A
        // Sort products in descending order
        public async Task<IActionResult> SortZA()
        {
            var products = (from p in _context.Products
                            where p.PQuantity > 0
                            select p).OrderByDescending(p => p.PProductname);

            return View("Index", await products.ToListAsync());
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
