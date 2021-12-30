using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using RosChemistTests.Models;
using RosChemistTests.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace RosChemistTests
{
    [Parallelizable(ParallelScope.Children)]
    public class FooterTests
    {
        protected AppSettingsModel _appSettings;

        [OneTimeSetUp]
        public void PrepareData()
        {
            var appSettingsString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//appsettings.json");
            _appSettings = JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsString);
        }

        [Test]
        [TestCase("play")]
        [TestCase("apple")]
        public void RedirectToOutsideServiceTest(string storeName)
        {
            var buttonSelector = $".footer-download__store a[href*='{storeName}']";

            var browserWrapper = BrowserWrapper.OpenBrowser(_appSettings);
            var webDriver = browserWrapper.GetWebDriver();

            browserWrapper.AcceptCookies();

            IWebElement element = webDriver.FindElement(By.CssSelector(".footer-download__store"));

            ((IJavaScriptExecutor)webDriver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(2000);
            ((IJavaScriptExecutor)webDriver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(2000);

            element.FindElement(By.CssSelector(buttonSelector))
                .Click();

            //sprawdź czy nastąpiło przekierowanie do zewnętrznego serwisu
            StringAssert.Contains(storeName, webDriver.Url);

            webDriver.Quit();
        }

        [Test]
        public void RedirectToSocialMediaServiceTest()
        {
            var browserWrapper = BrowserWrapper.OpenBrowser(_appSettings);
            var webDriver = browserWrapper.GetWebDriver();

            IWebElement element = webDriver.FindElement(By.CssSelector(".footer-download__store"));

            ((IJavaScriptExecutor)webDriver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(2000);

            var fbFanpage = webDriver.FindElement(By.CssSelector(".footer__social a[href*=facebook]"));
            fbFanpage.Click();

            Thread.Sleep(1000);

            //sprawdź czy nastąpiło przekierowanie do zewnętrznego serwisu
            StringAssert.Contains("facebook", webDriver.Url);

            webDriver.Quit();
        }
    }
}
