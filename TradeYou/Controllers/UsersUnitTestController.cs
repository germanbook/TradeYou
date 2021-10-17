using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeYou.Models;

namespace TradeYou.Controllers
{
    public class UsersUnitTestController : Controller
    {

        public List<User> GetUserList()
        {
            List<User> users = new List<User>();

            User user1 = new User(
                                    1001,
                                    "user1",
                                    "password",
                                    Convert.ToDateTime("01-01-2001"),
                                    "tesst@test.com",
                                    1
                                 );

            User user2 = new User(
                                    1002,
                                    "user2",
                                    "password",
                                    Convert.ToDateTime("02-02-2001"),
                                    "tesst@test.com",
                                    1
                                 );
            users.Add(user1);
            users.Add(user2);
            return users;
        }

        // Index
        public IActionResult Index()
        {
            var users = from u in GetUserList() select u;
            return View(users);
        }

        // Users
        public ActionResult Users()
        {
            var users = from u in GetUserList()
                        orderby u.UId
                        select u;

            return View(users);
        }

        // Details
        
        public ActionResult Details(int id)
        {
            return View("Details");
        }

    }
}
