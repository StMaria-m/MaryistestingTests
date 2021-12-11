using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using RosChemistTests.Helpers;
using RosChemistTests.Models;
using RosChemistTests.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RosChemistTests
{
    [Parallelizable(ParallelScope.Children)]
    public class UserProfileTests
    {
        protected AppSettingsModel _appSettings;

        [OneTimeSetUp]
        public void PrepareData()
        {
            var appSettingsString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//appsettings.json");
            _appSettings = JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsString);
        }

        [Test]
        [TestCase("profil/zamowienia", "Brak zamówień w historii")]
        [TestCase("profil/ulubione", "Nie masz jeszcze ulubionych")]
        public void CheckIfPurchaseHistoryAndFavouritesIsEmptyTest(string endPoint, string text)
        {
            var webDriver = BrowserWrapper.OpenBrowser(_appSettings, endPoint);

            ProcessStepHeplers.LogInSteps(webDriver, _appSettings);

            var purchaseHistory = webDriver.FindElements(By.CssSelector(".empty-list .h3.mt-5"));
            var element = purchaseHistory.Where(i => i.Text.Contains(text))
                .ToList();
            Assert.IsNotNull(element);

            webDriver.Quit();
        }

        [Test]
        public void LogInTest()
        {
            var webDriver = BrowserWrapper.OpenBrowser(_appSettings);

            var expectedUrl = $"{_appSettings.BaseUrl}/profil/ustawienia-konta";
            webDriver.Navigate().GoToUrl(expectedUrl);

            ProcessStepHeplers.LogInSteps(webDriver, _appSettings);

            var currentUrl = webDriver.Url;
            StringAssert.Contains(expectedUrl, currentUrl);

            webDriver.Quit();
        }
    }
}
