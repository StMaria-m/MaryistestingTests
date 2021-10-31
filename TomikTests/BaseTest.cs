using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TomikTests.Enums;
using TomikTests.Models;

namespace TomikTests
{
    public class BaseTest
    {
        protected IWebDriver _webDriver;
        protected string _path;
        protected AppSettingsModel _appSettings;

        [OneTimeSetUp]
        public void PrepareData()
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

            var cookies = JsonConvert.DeserializeObject<List<NameValueModel>>(_appSettings.CookiesData);

            foreach (NameValueModel cookieData in cookies)
            {
                var newCookie = new Cookie(cookieData.Name, cookieData.Value, _appSettings.CookieDomain, "/", DateTime.Now.AddYears(1));
                _webDriver.Manage().Cookies.AddCookie(newCookie);
            }

            _webDriver.Navigate().Refresh();
        }        

        [TearDown]
        public void Close()
        {
            _webDriver.Quit();
        }        

        public void LogInSteps()
        {
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(_appSettings.Login);

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(_appSettings.Password);

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            WaitForAction("#logout");
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
        }
    }
}
