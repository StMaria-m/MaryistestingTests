using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TomikTests
{
    public class BaseTest
    {
        protected IWebDriver _webDriver;
        protected string _path;

       [SetUp]
        public void Open()
        {
            _webDriver = new ChromeDriver();
            _webDriver.Manage().Window.Maximize();
            _webDriver.Url = $"https://xx.pl/{_path}";

        }

        [TearDown]
        public void Close()
        {
            _webDriver.Quit();
        }

        public void RemoveAcceptContainer()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_webDriver;
            js.ExecuteScript("$(\"#acceptChomikujTermsContainer, #acceptChomikujTermsOverlay\").remove()");
        }

    }
}
