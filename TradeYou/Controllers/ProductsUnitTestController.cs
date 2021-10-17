using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeYou.Models;

namespace TradeYou.Controllers
{
    public class ProductsUnitTestController : Controller
    {

        public List<Product> GetProductsList()
        {
            List<Product> products = new List<Product>();

            Product product1 = new Product(
                                            1001,
                                            "product1",
                                            2,
                                            5,
                                            "Apple",
                                            1
                                           );

            Product product2 = new Product(
                                            1002,
                                            "product2",
                                            2,
                                            5,
                                            "Banana",
                                            1
                                           );
            products.Add(product1);
            products.Add(product2);
            return products;
        }

        // Index
        public IActionResult Index()
        {
            var products = from p in GetProductsList() select p;
            return View(products);
        }

        // Products
        public ActionResult Products()
        {
            var products = from p in GetProductsList()
                        orderby p.PId
                        select p;

            return View(products);
        }

        // Details
        public ActionResult Details(int id)
        {

            return View();
        }
    }
}
