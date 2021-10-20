using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TrenditTests.Enums;
using TrenditTests.Models;

namespace TrenditTests
{
    public class BaseTest
    {
        protected IWebDriver _webDriver;
        protected string _path;
        protected AppSettingsModel _appSettings;

        public BaseTest()
        {
            var appSettingsString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//appsettings.json");
            _appSettings = JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsString);
        }

        [SetUp]
        public void Open()
        {
            _webDriver = new ChromeDriver();
            _webDriver.Manage().Window.Maximize();
            _webDriver.Url = $"{_appSettings.BaseUrl}/{_path}";
        }

        [TearDown]
        public void Close()
        {
            _webDriver.Quit();
        }

        public void WaitForElementDisplayed(string selector, SearchByTypeEnums selectorType = SearchByTypeEnums.CssSelector)
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

        public void WaitForElementDisappeared(string selector, SearchByTypeEnums selectorType = SearchByTypeEnums.CssSelector)
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            wait.Until(driver => {
                try
                {
                    var selectorFunction = selectorType == SearchByTypeEnums.CssSelector ? By.CssSelector(selector) : By.XPath(selector);

                    driver.FindElement(selectorFunction);
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
            });
        }

        public void LogInUserAccount()
        {
            _webDriver.Navigate().GoToUrl(_appSettings.LoginFormUrl);

            WaitForElementDisplayed("[name=login]");

            //uzupełnić Login
            var loginInput = _webDriver.FindElement(By.CssSelector("[name=login]"));
            loginInput.SendKeys(_appSettings.Login);

            //uzupełnić hasło
            var passwordInput = _webDriver.FindElement(By.CssSelector("[name=password]"));
            passwordInput.SendKeys(_appSettings.Password);

            //kliknąć przycisk "Zaloguj się"
            var registrationButton = _webDriver.FindElement(By.CssSelector("[type=submit].button100"));
            registrationButton.Click();

            WaitForElementDisplayed("a[href='/wyloguj']");
        }

        public void FindAndClick(string selector, SearchByTypeEnums selectorType = SearchByTypeEnums.CssSelector)
        {
            var selectorFunction = selectorType == SearchByTypeEnums.CssSelector ? By.CssSelector(selector) : By.XPath(selector);
            _webDriver.FindElement(selectorFunction)
                .Click();
        }

        public void FindAndSendKeys(string selector, string text, SearchByTypeEnums selectorType = SearchByTypeEnums.CssSelector)
        {
            By selectorFunction = selectorType == SearchByTypeEnums.CssSelector ? By.CssSelector(selector) : By.XPath(selector);
            var input = _webDriver.FindElement(selectorFunction);
            input.Clear();
            input.SendKeys(text);
        }
    }
}
