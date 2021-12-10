using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;
using RosChemistTests.Wrappers;
using RosChemistTests.Models;
using System.IO;
using Newtonsoft.Json;

namespace RosChemistTests
{
    [Parallelizable(ParallelScope.Children)]
    public class RosChemistTests
    {
        protected AppSettingsModel _appSettings;

        [OneTimeSetUp]
        public void PrepareData()
        {
            var appSettingsString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//appsettings.json");
            _appSettings = JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsString);
        }     

        [Test]
        public void WylogowanieZProfiluUzytkownika()
        {
            var webDriver = OpenBrowser();

            webDriver.Navigate().GoToUrl($"{_appSettings.BaseUrl}/profil/ustawienia-konta");

            Logowanie(webDriver);

            Actions action = new Actions(webDriver);
            var userSettingButton = webDriver.FindElement(By.CssSelector(".d-lg-inline-block a[href$='ustawienia-konta'] .nav-user__icon.icon"));
            action.MoveToElement(userSettingButton).Perform();

            var logOut = webDriver.FindElement(By.CssSelector(".nav-user .nav-user__item:nth-child(4) .btn-outline-black.w-100"));
            action.MoveToElement(logOut).Perform();
            logOut.Click();

            var currentUrl = webDriver.Url;
            StringAssert.Contains(_appSettings.BaseUrl, currentUrl);

            webDriver.Quit();
        }             

        [Test]
        public void PrzejscieDoInnejZakładki()
        {
            PrzejscieDoInnejZakładki2("promocje", "promocje");
        }

        public void PrzejscieDoInnejZakładki2(string tabSelector, string expectedPath)
        {
            var webDriver = OpenBrowser();

            webDriver.FindElement(By.CssSelector($"#sticky-nav [href$={tabSelector}]"))
                .Click();

            var url = webDriver.Url;
            StringAssert.Contains(expectedPath, url);

            webDriver.Quit();
        }

        [Test]
        public void PoprawneLogowanieNaKontoUzytkownika()
        {
            var webDriver = OpenBrowser();

            var expectedUrl = $"{_appSettings.BaseUrl}/profil/ustawienia-konta";
            webDriver.Navigate().GoToUrl(expectedUrl);

            Logowanie(webDriver);

            var currentUrl = webDriver.Url;
            StringAssert.Contains(expectedUrl, currentUrl);

            webDriver.Quit();
        }       

        public void Logowanie(IWebDriver webDriver)
        {
            webDriver.FindElement(By.CssSelector(".login-form [name='login']"))
                .SendKeys(_appSettings.Login);

            webDriver.FindElement(By.CssSelector(".login-form [name='password']"))
                .SendKeys(_appSettings.Password);

            webDriver.FindElement(By.CssSelector(".login-form [type='submit']"))
                .Click();

            Thread.Sleep(1000);
        }

        private IWebDriver OpenBrowser()
        {
            BrowserWrapper browserWrapper = new BrowserWrapper(_appSettings);
            browserWrapper.CreateWebDriver();
            browserWrapper.AddCookie();
            return browserWrapper.GetWebDriver();
        }
    }
}
