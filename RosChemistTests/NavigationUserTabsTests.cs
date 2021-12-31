using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using RosChemistTests.Helpers;
using RosChemistTests.Models;
using RosChemistTests.Wrappers;
using System;
using System.IO;
using System.Threading;

namespace RosChemistTests
{
    [Parallelizable(ParallelScope.Children)]
    public class NavigationUserTabsTests
    {
        protected AppSettingsModel _appSettings;

        [OneTimeSetUp]
        public void PrepareData()
        {
            var appSettingsString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//appsettings.json");
            _appSettings = JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsString);
        }

        [Test]
        public void CheckFavouritesTabTest()
        {
            var webDriver = BrowserWrapper.OpenBrowser(_appSettings, "profil/ustawienia-konta")
                .GetWebDriver();

            ProcessStepHeplers.LogInSteps(webDriver, _appSettings);

            Actions action = new Actions(webDriver);
            var userSettingButton = webDriver.FindElement(By.CssSelector(".d-lg-inline-block a[href$='ustawienia-konta'] .nav-user__icon.icon"));
            action.MoveToElement(userSettingButton).Perform();

            var favouritesTab = webDriver.FindElement(By.CssSelector(".nav-user .nav-user__item:nth-child(4) [href$=ulubione]"));
            action.MoveToElement(favouritesTab).Perform();
            favouritesTab.Click();

            StringAssert.Contains("/profil/ulubione", webDriver.Url);

            webDriver.Quit();
        }

        [Test]
        public void CheckYourProfileTabTest()
        {
            var webDriver = BrowserWrapper.OpenBrowser(_appSettings, "profil/ustawienia-konta")
                .GetWebDriver();

            ProcessStepHeplers.LogInSteps(webDriver, _appSettings);

            Actions action = new Actions(webDriver);
            var userSettingButton = webDriver.FindElement(By.CssSelector(".d-lg-inline-block a[href$='ustawienia-konta'] .nav-user__icon.icon"));
            action.MoveToElement(userSettingButton)
                .Perform();

            var favouritesTab = webDriver.FindElement(By.CssSelector(".nav-user .nav-user__item:nth-child(4) [href$=profil]"));
            action.MoveToElement(favouritesTab)
                .Perform();
            favouritesTab.Click();

            StringAssert.Contains("/profil", webDriver.Url);

            webDriver.Quit();
        }

        [Test]
        public void CheckPurchaseHistoryTabTest()
        {
            var webDriver = BrowserWrapper.OpenBrowser(_appSettings, "profil/ustawienia-konta")
              .GetWebDriver();

            ProcessStepHeplers.LogInSteps(webDriver, _appSettings);

            Actions action = new Actions(webDriver);
            var userSettingButton = webDriver.FindElement(By.CssSelector(".d-lg-inline-block a[href$='ustawienia-konta'] .nav-user__icon.icon"));
            action.MoveToElement(userSettingButton).Perform();

            var favouritesTab = webDriver.FindElement(By.CssSelector(".nav-user .nav-user__item:nth-child(4) [href$=zamowienia]"));
            action.MoveToElement(favouritesTab).Perform();
            favouritesTab.Click();

            StringAssert.Contains("/profil/zamowienia", webDriver.Url);

            webDriver.Quit();
        }

        [Test]
        public void SprawdzicCzyPojawiaSieMenuRozwijane()
        {
            var browserWrapper = BrowserWrapper.OpenBrowser(_appSettings, "profil/ustawienia-konta");
            var webDriver = browserWrapper.GetWebDriver();

            ProcessStepHeplers.LogInSteps(webDriver, _appSettings);
           
            //najechać myszą na tab Kategorie i spr czy pojawia się menu
            Actions action = new Actions(webDriver);
            var categoriesMenu = webDriver.FindElement(By.CssSelector("a[href$=kategorie]"));            
            action.MoveToElement(categoriesMenu).Perform();

            Thread.Sleep(1000);

            var faceTab = webDriver.FindElement(By.CssSelector("[href*='8686'].sub-nav__link"));
            action.MoveToElement(faceTab).Perform();
            faceTab.Click();

            Thread.Sleep(1000);

            var a = webDriver.FindElement(By.CssSelector(".mega-menu [href*='8686'].btn"));
            action.MoveToElement(a).Perform();
            Thread.Sleep(500);
            a.Click();

            StringAssert.Contains("Twarz", webDriver.Url);

            webDriver.Quit();



        }
    }
}
