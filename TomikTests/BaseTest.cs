using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TomikTests
{
    public class BaseTest
    {
        protected IWebDriver _webDriver;

       [SetUp]
        public void Open()
        {
            _webDriver = new ChromeDriver();
            _webDriver.Manage().Window.Maximize();
            _webDriver.Url = "";

        }

        [TearDown]
        public void Close()
        {
            _webDriver.Quit();
        }
    }
}
