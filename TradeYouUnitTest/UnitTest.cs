using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TradeYou.Controllers;
using TradeYou.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;



namespace TradeYouUnitTest
{
    [TestClass]
    public class UsersUnitTest
    {

        [TestMethod]
        public void UsersIndexTest()
        {
            UsersUnitTestController usersController = new UsersUnitTestController();

            IActionResult result = usersController.Index() as IActionResult;

            Assert.IsNotNull(result);

        }


        [TestMethod]
        public void UsersUsersTest()
        {
            UsersUnitTestController usersController = new UsersUnitTestController();

            IActionResult result = usersController.Users() as IActionResult;

            Assert.IsNotNull(result);

        }

    }

    [TestClass]
    public class ProductsUnitTest
    {

        [TestMethod]
        public void ProductsIndexTest()
        {
            ProductsUnitTestController productsController = new ProductsUnitTestController();

            IActionResult result = productsController.Index() as IActionResult;

            Assert.IsNotNull(result);

        }


        [TestMethod]
        public void ProductsProductsTest()
        {
            ProductsUnitTestController productsController = new ProductsUnitTestController();

            IActionResult result = productsController.Products() as IActionResult;

            Assert.IsNotNull(result);

        }


    }

    [TestClass]
    public class OrderssUnitTest
    {

        [TestMethod]
        public void OrdersIndexTest()
        {
            OrdersUnitTestController ordersController = new OrdersUnitTestController();

            IActionResult result = ordersController.Index() as IActionResult;

            Assert.IsNotNull(result);

        }


        [TestMethod]
        public void OrdersOrdersTest()
        {
            OrdersUnitTestController ordersController = new OrdersUnitTestController();

            IActionResult result = ordersController.Orders() as IActionResult;

            Assert.IsNotNull(result);

        }


    }
}
