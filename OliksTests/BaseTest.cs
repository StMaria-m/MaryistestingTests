using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using OliksTests.Enums;

namespace OliksTests
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
            _webDriver.Url = $"https://https://www.sendit.pl/{_path}";

        }

        [TearDown]
        public void Close()
        {
            _webDriver.Quit();
        }

        public void ClickAcceptContainer()
        {
            var agreement = _webDriver.FindElement(By.CssSelector("#onetrust-accept-btn-handler"));
            agreement.Click();
        }

       
        public void WaitForAction(string selector, SearchByTypeEnums selectorType = SearchByTypeEnums.CssSelector)
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            wait.Until(driver => {
                var selectorFunction = selectorType == SearchByTypeEnums.CssSelector ? By.CssSelector(selector) : By.XPath(selector);

                var element = driver.FindElements(selectorFunction).FirstOrDefault();
                if (element == null)
                {
                    return false;
                }
                return element.Displayed;
            });
        }

        //olx
        public void LogInSteps(string path = null)
        {
            ClickAcceptContainer();

            //kliknąć element "Mój OLX"
            var loginLink = _webDriver.FindElement(By.CssSelector("#topLoginLink"));
            loginLink.Click();

            WaitForAction("#loginForm");

            //uzupełnić "E-mail do konta OLX"
            var userEmailInput = _webDriver.FindElement(By.CssSelector("#userEmail"));
            userEmailInput.SendKeys("m.test@o2.pl");

            //uzupełnić hasło
            var userPasswordInput = _webDriver.FindElement(By.CssSelector("#userPass"));
            userPasswordInput.SendKeys("123456^%$#@!Qa");

            //kliknać "Zaloguj się"
            var logInButton = _webDriver.FindElement(By.CssSelector("#se_userLogin"));
            logInButton.Click();

            WaitForAction("#se_accountShop");

            if (path != null)
            {
                _webDriver.Navigate().GoToUrl($"https://www.olx.pl/{path}");
            }
        }
    }
}
