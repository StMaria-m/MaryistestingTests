using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using TomikTests.Enums;

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
            _webDriver.Url = $"https://.pl/{_path}";

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

        public void LogInSteps()
        {
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys("");

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys("");

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            WaitForAction("#logout");

            RemoveAcceptContainer();
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

        public void ClickUserAvatarStep()
        {
            var userAccount = _webDriver.FindElement(By.CssSelector("#topbarAvatar .friendSmall"));
            userAccount.Click();

            RemoveAcceptContainer();
        }
    }
}
