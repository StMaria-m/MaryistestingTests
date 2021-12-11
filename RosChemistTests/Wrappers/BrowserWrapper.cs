using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RosChemistTests.Enums;
using RosChemistTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RosChemistTests.Wrappers
{
    public class BrowserWrapper
    {
        private AppSettingsModel _appSettings { get; set; }
        private IWebDriver _webDriver { get; set; }

        public BrowserWrapper(AppSettingsModel appSettings)
        {
            _appSettings = appSettings;
        }

        public IWebDriver GetWebDriver() => _webDriver;

        public void CreateWebDriver(string path = null)
        {
            _webDriver = new ChromeDriver();
            _webDriver.Manage().Window.Maximize();
            _webDriver.Url = $"{_appSettings.BaseUrl}/{path}";
        }

        public void AddCookie()
        {
            var cookies = JsonConvert.DeserializeObject<List<NameValueModel>>(_appSettings.CookiesData);

            foreach (NameValueModel cookieData in cookies)
            {
                var newCookie = new Cookie(cookieData.Name, cookieData.Value, _appSettings.CookieDomain, "/", DateTime.Now.AddYears(1));
                _webDriver.Manage().Cookies.AddCookie(newCookie);
            }

            _webDriver.Navigate().Refresh();
        }

        public void WaitForAction(string selector, SearchByTypeEnums selectorType = SearchByTypeEnums.CssSelector)
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            wait.Until(driver =>
            {
                var selectorFunction = selectorType == SearchByTypeEnums.CssSelector ? By.CssSelector(selector) : By.XPath(selector);

                var element = driver.FindElements(selectorFunction).FirstOrDefault();
                if (element == null)
                {
                    return false;
                }
                return element.Displayed;
            });
        }

        public static IWebDriver OpenBrowser(AppSettingsModel appSettings, string endPoint = null)
        {
            BrowserWrapper browserWrapper = new BrowserWrapper(appSettings);
            browserWrapper.CreateWebDriver(endPoint);
            browserWrapper.AddCookie();
            return browserWrapper.GetWebDriver();
        }
    }
}
