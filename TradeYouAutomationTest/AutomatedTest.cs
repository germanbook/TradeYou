using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;

namespace TradeYouAutomationTest
{
    [TestClass]
    public class HomepageTest
    {
        private readonly IWebDriver _driver;

        public HomepageTest()
        {
            _driver = new ChromeDriver();
        }

        private static void Delay()
        {
            Thread.Sleep(500);
        }

        [TestMethod]
        public void HomepageAccessTest()
        {
            _driver.Navigate().GoToUrl("https://localhost:44372/");

            Assert.AreEqual("Trade You - Weltec's #1 webpage - Home Page", _driver.Title);
        }

        [TestMethod]
        public void HomepageSearchTest()
        {
            _driver.Navigate().GoToUrl("https://localhost:44372/");

            // Searching string
            IWebElement searchingString = _driver.FindElement(By.Name("SearchString"));
            searchingString.SendKeys("iPhone");
            Delay();

            // Searching button
            IWebElement button = _driver.FindElement(By.Id("searchButton"));

            button.Click();

        }

        [TestMethod]
        public void HomepageSortAZTest()
        {
            _driver.Navigate().GoToUrl("https://localhost:44372/");

            // SortAZ button
            IWebElement button = _driver.FindElement(By.Id("sortAZ"));
            Delay();
            button.Click();

        }

        [TestMethod]
        public void HomepageSortZATest()
        {
            _driver.Navigate().GoToUrl("https://localhost:44372/");

            // SortZA button
            IWebElement button = _driver.FindElement(By.Id("sortZA"));
            Delay();
            button.Click();

        }

        [TestMethod]
        public void HomepageLoginTest()
        {
            _driver.Navigate().GoToUrl("https://localhost:44372/");

            // Login button
            IWebElement button = _driver.FindElement(By.Id("login"));
            Delay();
            button.Click();

            // User name
            IWebElement userName = _driver.FindElement(By.Id("typeUsername"));

            // User password
            IWebElement userPassword = _driver.FindElement(By.Id("typePasswordX"));

            userName.SendKeys("admin");
            Delay();
            userPassword.SendKeys("admin");


            // Login button
            IWebElement login = _driver.FindElement(By.Id("loginSubmit"));
            Delay();
            login.Click();

        }

        [TestMethod]
        public void HomepageAddToCartTest()
        {
            // Login first
            HomepageLoginTest();

            // Back to home page
            _driver.Navigate().GoToUrl("https://localhost:44372/");

            // Adding button
            IWebElement addingButton = _driver.FindElement(By.Id("addToCart"));
            Delay();
            addingButton.Click();
        }
    }

    [TestClass]
    public class ProductsTest
    {
        private readonly IWebDriver _driver;

        public ProductsTest()
        {
            _driver = new ChromeDriver();
        }

        private static void Delay()
        {
            Thread.Sleep(500);
        }

        // Login
        public void Login()
        {
            _driver.Navigate().GoToUrl("https://localhost:44372/");
            // Login button
            IWebElement button = _driver.FindElement(By.Id("login"));
            button.Click();
            // User name
            IWebElement userName = _driver.FindElement(By.Id("typeUsername"));
            // User password
            IWebElement userPassword = _driver.FindElement(By.Id("typePasswordX"));
            userName.SendKeys("admin");
            userPassword.SendKeys("admin");
            // Login button
            IWebElement login = _driver.FindElement(By.Id("loginSubmit"));
            login.Click();

        }


        [TestMethod]
        public void ProductsAccessTest()
        {
            Login();

            Delay();

            _driver.Navigate().GoToUrl("https://localhost:44372/");

            // Products button
            IWebElement button = _driver.FindElement(By.Id("products"));
            Delay();
            button.Click();

            Assert.AreEqual("Trade You - Weltec's #1 webpage - Products", _driver.Title);
        }

        [TestMethod]
        public void ProductsSearchTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Products");

            // Searching string
            IWebElement searchingString = _driver.FindElement(By.Name("SearchString"));
            searchingString.SendKeys("iPhone");
            Delay();

            // Searching button
            IWebElement button = _driver.FindElement(By.Id("searchButton"));

            button.Click();

        }

        [TestMethod]
        public void ProductsSortAZTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Products");

            // SortAZ button
            IWebElement button = _driver.FindElement(By.Id("sortAZ"));
            Delay();
            button.Click();

        }

        [TestMethod]
        public void ProductsSortZATest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Products");

            // SortZA button
            IWebElement button = _driver.FindElement(By.Id("sortZA"));
            Delay();
            button.Click();

        }

        [TestMethod]
        public void ProductsAddNewProductTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Products");

            // Add New Product button
            IWebElement button = _driver.FindElement(By.Id("addNewProducts"));
            Delay();
            button.Click();

            // name
            IWebElement name = _driver.FindElement(By.Id("name"));
            name.SendKeys("A Kitty");
            Delay();

            // price
            IWebElement price = _driver.FindElement(By.Id("price"));
            price.SendKeys("3");
            Delay();

            // quantity
            IWebElement quantity = _driver.FindElement(By.Id("quantity"));
            quantity.SendKeys("5");
            Delay();

            // brand
            IWebElement brand = _driver.FindElement(By.Id("made"));
            brand.SendKeys("TestBrand");
            Delay();

            // condition
            IWebElement conditionSelect = _driver.FindElement(By.Id("newUsed"));

            // SelectBox
            SelectElement condition = new SelectElement(conditionSelect);

            // Select Used
            condition.SelectByText("Used");
            Delay();

            // image
            IWebElement image = _driver.FindElement(By.Id("image"));
            image.SendKeys("C:/Users/fanbi/Desktop/2021_T2/SecureCoding/cN60QVJ1.jpg");
            Delay();

            // Create
            IWebElement create = _driver.FindElement(By.Id("create"));
            Delay();
            create.Click();

        }

        [TestMethod]
        public void ProductsEditTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Products");

            // Add New Product button
            IWebElement button = _driver.FindElement(By.Id("edit"));
            Delay();
            button.Click();

            // name
            IWebElement name = _driver.FindElement(By.Id("name"));
            name.SendKeys(" Edit");
            Delay();

            // price
            IWebElement price = _driver.FindElement(By.Id("price"));
            price.SendKeys("3");
            Delay();

            // quantity
            IWebElement quantity = _driver.FindElement(By.Id("quantity"));
            quantity.SendKeys("5");
            Delay();

            // brand
            IWebElement brand = _driver.FindElement(By.Id("made"));
            brand.SendKeys(" Edit");
            Delay();

            // condition
            IWebElement conditionSelect = _driver.FindElement(By.Id("newUsed"));

            // SelectBox
            SelectElement condition = new SelectElement(conditionSelect);

            // Select New
            condition.SelectByText("New");
            Delay();

            // Create
            IWebElement save = _driver.FindElement(By.Id("save"));
            Delay();
            save.Click();
        }

        [TestMethod]
        public void ProductsDeleteTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Products");

            // Delete button
            IWebElement button = _driver.FindElement(By.Id("delete"));
            Delay();
            button.Click();

            try
            {
                // Can not delete page
                IWebElement ok = _driver.FindElement(By.Id("ok"));
                Delay();
                ok.Click();

            }
            catch (NoSuchElementException)
            {
                // Confirm delete
                IWebElement delete = _driver.FindElement(By.Id("delete"));
                Delay();
                delete.Click();

            }

        }
    }

    [TestClass]
    public class UsersTest
    {
        private readonly IWebDriver _driver;

        public UsersTest()
        {
            _driver = new ChromeDriver();
        }

        private static void Delay()
        {
            Thread.Sleep(500);
        }

        // Login
        public void Login()
        {
            _driver.Navigate().GoToUrl("https://localhost:44372/");
            // Login button
            IWebElement button = _driver.FindElement(By.Id("login"));
            button.Click();
            // User name
            IWebElement userName = _driver.FindElement(By.Id("typeUsername"));
            // User password
            IWebElement userPassword = _driver.FindElement(By.Id("typePasswordX"));
            userName.SendKeys("admin");
            userPassword.SendKeys("admin");
            // Login button
            IWebElement login = _driver.FindElement(By.Id("loginSubmit"));
            login.Click();

        }


        [TestMethod]
        public void UsersAccessTest()
        {
            Login();

            Delay();

            _driver.Navigate().GoToUrl("https://localhost:44372/");

            // Users button
            IWebElement button = _driver.FindElement(By.Id("users"));
            Delay();
            button.Click();

            Assert.AreEqual("Trade You - Weltec's #1 webpage - Users", _driver.Title);
        }

        [TestMethod]
        public void UsersAddNewUserTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Users");

            // Add New User button
            IWebElement button = _driver.FindElement(By.Id("addNewUser"));
            Delay();
            button.Click();

            // name
            IWebElement name = _driver.FindElement(By.Id("name"));
            name.SendKeys("TestUser");
            Delay();

            // password
            IWebElement password = _driver.FindElement(By.Id("password"));
            password.SendKeys("password");
            Delay();

            // data of birth
            IWebElement dob = _driver.FindElement(By.Id("dob"));
            DateTime testDate = DateTime.Now;
            dob.SendKeys("09/05/2013");
            dob.SendKeys(Keys.Tab);
            dob.SendKeys("0245PM");

            Delay();

            // email
            IWebElement email = _driver.FindElement(By.Id("email"));
            email.SendKeys("test@test.com");
            Delay();

            // account type
            IWebElement accountType = _driver.FindElement(By.Id("isAdmin"));

            // SelectBox
            SelectElement type = new SelectElement(accountType);

            // Select user type
            type.SelectByText("Normal User");
            Delay();

            // Create
            IWebElement create = _driver.FindElement(By.Id("create"));
            Delay();
            create.Click();

        }


        [TestMethod]
        public void UsersEditTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Users");

            // Edit user button
            IWebElement button = _driver.FindElement(By.Id("edit"));
            Delay();
            button.Click();

            // name
            IWebElement name = _driver.FindElement(By.Id("name"));
            name.Clear();
            Delay();
            name.SendKeys("TestUser");
            Delay();

            // password
            IWebElement password = _driver.FindElement(By.Id("password"));
            password.Clear();
            Delay();
            password.SendKeys("TestPassword");
            Delay();

            // data of birth
            IWebElement dob = _driver.FindElement(By.Id("dob"));
            dob.Clear();
            dob.SendKeys("10/10/2001");
            dob.SendKeys(Keys.Tab);
            dob.SendKeys("0355AM");
            Delay();

            // email
            IWebElement email = _driver.FindElement(By.Id("email"));
            email.Clear();
            Delay();
            email.SendKeys("test@test.com");
            Delay();

            // account type
            IWebElement accountType = _driver.FindElement(By.Id("isAdmin"));

            // SelectBox
            SelectElement type = new SelectElement(accountType);
            // Select Normal User
            type.SelectByText("Normal User");
            Delay();

            // Save
            IWebElement save = _driver.FindElement(By.Id("save"));
            Delay();
            save.Click();
        }

        [TestMethod]
        public void UsersDeleteTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Users");

            // Delete button
            IWebElement button = _driver.FindElement(By.Id("delete"));
            Delay();
            button.Click();

            try
            {
                // Can not delete page
                IWebElement ok = _driver.FindElement(By.Id("ok"));
                Delay();
                ok.Click();

            }
            catch (NoSuchElementException)
            {
                // Confirm delete button
                IWebElement delete = _driver.FindElement(By.Id("delete"));
                Delay();

                delete.Click();

            }


        }

    }

    [TestClass]
    public class MyTradeYouTest
    {
        private readonly IWebDriver _driver;

        public MyTradeYouTest()
        {
            _driver = new ChromeDriver();
        }

        private static void Delay()
        {
            Thread.Sleep(500);
        }

        // Login
        public void Login()
        {
            _driver.Navigate().GoToUrl("https://localhost:44372/");
            // Login button
            IWebElement button = _driver.FindElement(By.Id("login"));
            button.Click();
            // User name
            IWebElement userName = _driver.FindElement(By.Id("typeUsername"));
            // User password
            IWebElement userPassword = _driver.FindElement(By.Id("typePasswordX"));
            userName.SendKeys("admin");
            userPassword.SendKeys("admin");
            // Login button
            IWebElement login = _driver.FindElement(By.Id("loginSubmit"));
            login.Click();

        }

        [TestMethod]
        public void UserProfileTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372");

            // My Trade You button
            IWebElement myTradeYou = _driver.FindElement(By.Id("navbarDropdown"));
            Delay();
            myTradeYou.Click();

            // User profile
            IWebElement profile = _driver.FindElement(By.Id("profile"));
            Delay();
            profile.Click();

            // Password
            IWebElement password = _driver.FindElement(By.Id("password"));
            password.Clear();
            Delay();
            password.SendKeys("test");

            // data of birth
            IWebElement dob = _driver.FindElement(By.Id("dob"));
            dob.Clear();
            dob.SendKeys("12/12/2012");

            dob.SendKeys(Keys.Tab);

            dob.SendKeys("0355AM");

            Delay();

            // Eamil
            IWebElement email = _driver.FindElement(By.Id("email"));
            email.Clear();
            Delay();
            email.SendKeys("test@test.com");
            Delay();

            // Save
            IWebElement save = _driver.FindElement(By.Id("save"));
            save.Click();
            Delay();

        }

        [TestMethod]
        public void OrdersViewTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372");

            // My Trade You button
            IWebElement myTradeYou = _driver.FindElement(By.Id("navbarDropdown"));
            Delay();
            myTradeYou.Click();

            // Orders
            IWebElement orders = _driver.FindElement(By.Id("myOrder"));
            Delay();
            orders.Click();

            Assert.AreEqual("Trade You - Weltec's #1 webpage - Orders", _driver.Title);
        }

        [TestMethod]
        public void OrdersDeleteTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372");

            // My Trade You button
            IWebElement myTradeYou = _driver.FindElement(By.Id("navbarDropdown"));
            Delay();
            myTradeYou.Click();

            // Orders
            IWebElement orders = _driver.FindElement(By.Id("myOrder"));
            Delay();
            orders.Click();

            // delete
            IWebElement delete = _driver.FindElement(By.Id("delete"));
            Delay();
            delete.Click();

            // confirm
            IWebElement deleteConfirm = _driver.FindElement(By.Id("delete"));
            Delay();
            deleteConfirm.Click();
        }

        [TestMethod]
        public void LogoutTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372");

            // My Trade You button
            IWebElement myTradeYou = _driver.FindElement(By.Id("navbarDropdown"));
            Delay();
            myTradeYou.Click();

            // Logout
            IWebElement logout = _driver.FindElement(By.Id("userLogout"));
            Delay();
            logout.Click();

            // logout confirm
            IWebElement logoutConfirm = _driver.FindElement(By.Id("logout"));
            Delay();
            logoutConfirm.Click();

        }

    }

    [TestClass]
    public class ShoppingCartTest
    {
        private readonly IWebDriver _driver;

        public ShoppingCartTest()
        {
            _driver = new ChromeDriver();
        }

        private static void Delay()
        {
            Thread.Sleep(500);
        }

        // Login
        public void Login()
        {
            _driver.Navigate().GoToUrl("https://localhost:44372/");
            // Login button
            IWebElement button = _driver.FindElement(By.Id("login"));
            button.Click();
            // User name
            IWebElement userName = _driver.FindElement(By.Id("typeUsername"));
            // User password
            IWebElement userPassword = _driver.FindElement(By.Id("typePasswordX"));
            userName.SendKeys("admin");
            userPassword.SendKeys("admin");
            // Login button
            IWebElement login = _driver.FindElement(By.Id("loginSubmit"));
            login.Click();

        }

        [TestMethod]
        public void ShoppingCartAccessTest()
        {
            Login();

            Delay();

            _driver.Navigate().GoToUrl("https://localhost:44372");

            // Shopping cart 
            IWebElement button = _driver.FindElement(By.Id("shoppingCart"));
            Delay();
            button.Click();

            Assert.AreEqual("Trade You - Weltec's #1 webpage - ShoppingCart", _driver.Title);
        }

        [TestMethod]
        public void ShoppingCartEditTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Orders/ShoppingCart");

            // Edit
            IWebElement button = _driver.FindElement(By.Id("edit"));
            Delay();
            button.Click();

            // Quantity
            IWebElement quantity = _driver.FindElement(By.Id("quantity"));
            quantity.Clear();
            quantity.SendKeys("3");
            Delay();

            // Save
            IWebElement save = _driver.FindElement(By.Id("save"));
            Delay();
            save.Click();

            try
            {
                // No more stock page
                IWebElement errorBack = _driver.FindElement(By.Id("errorBack"));
                Delay();
                errorBack.Click();

            }
            catch (NoSuchElementException)
            {
                // Do nothing
            }

 
        }

        [TestMethod]
        public void ShoppingCartDeleteTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Orders/ShoppingCart");

            // Delete
            IWebElement button = _driver.FindElement(By.Id("delete"));
            Delay();
            button.Click();

            // Confirm
            IWebElement delete = _driver.FindElement(By.Id("confirmDelete"));
            Delay();
            delete.Click();
            Delay();

        }


        [TestMethod]
        public void ShoppingCartCheckoutTest()
        {
            Login();

            _driver.Navigate().GoToUrl("https://localhost:44372/Orders/ShoppingCart");

            // Checkout
            IWebElement button = _driver.FindElement(By.Id("checkout"));
            Delay();
            button.Click();

            Delay();
            // Payment type
            IWebElement payment = _driver.FindElement(By.Id("payment"));

            // SelectBox
            SelectElement paymentType = new SelectElement(payment);

            // Select Apple Pay
            paymentType.SelectByText("Apple Pay");
            Delay();

            // Shipping typt
            IWebElement shipping = _driver.FindElement(By.Id("shipping"));

            // SelectBox
            SelectElement shippingType = new SelectElement(shipping);

            // Select Free Shipping
            shippingType.SelectByText("Free Shipping");
            Delay();


            // Save
            IWebElement save = _driver.FindElement(By.Id("save"));
            Delay();
            save.Click();


        }


    }


}