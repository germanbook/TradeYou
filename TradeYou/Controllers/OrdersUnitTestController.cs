using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeYou.Models;

namespace TradeYou.Controllers
{
    public class OrdersUnitTestController : Controller
    {
        public List<Order> GetOrdersList()
        {
            List<Order> orders = new List<Order>();

            Order order1 = new Order(
                                        1001,
                                        1001,
                                        1001,
                                        1,
                                        1,
                                        DateTime.Now,
                                        5
                                    );

            Order order2 = new Order(
                                        1002,
                                        1002,
                                        1002,
                                        1,
                                        1,
                                        DateTime.Now,
                                        5
                                    );
            orders.Add(order1);
            orders.Add(order2);
            return orders;
        }

        // Index
        public IActionResult Index()
        {
            var orders = from o in GetOrdersList() select o;
            return View(orders);
        }

        // Orders
        public ActionResult Orders()
        {
            var orders = from o in GetOrdersList()
                        orderby o.OId
                        select o;

            return View(orders);
        }

        // Details
        public ActionResult Details(int id)
        {

            return View();
        }
    }
}
