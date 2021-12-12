using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using RosChemistTests.Helpers;
using RosChemistTests.Models;
using RosChemistTests.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace RosChemistTests
{
    public class UserSettingsTests
    {
        protected AppSettingsModel _appSettings;

        [OneTimeSetUp]
        public void PrepareData()
        {
            var appSettingsString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//appsettings.json");
            _appSettings = JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsString);
        }

        [Test]
        public void EditDefaultUserStore()
        {
            var browserWrapper = BrowserWrapper.OpenBrowser(_appSettings, "profil/ustawienia-konta");
            var webDriver = browserWrapper.GetWebDriver();

            ProcessStepHeplers.LogInSteps(webDriver, _appSettings);
            browserWrapper.AcceptCookies();

            string newShop = "os. Zgody 4 (CH Huta)";

            //kliknąć pole Edytuj,
            var editionButton = webDriver.FindElement(By.CssSelector(".settings__card [href$=edycja-drogerii]"));
            editionButton.Click();

            //kliknać Znajdź sklep i wpisać Kraków Zgody
            var shopInput = webDriver.FindElement(By.CssSelector(".profile-drugstore__search-holder [placeholder='Znajdź sklep']"));
            shopInput.SendKeys(newShop);

            browserWrapper.WaitForAction(".profile-drugstore__list .profile-drugstore__list-item");
            //w liście kliknąć Wybierz przy konkretnym sklepie
            var shopsList = webDriver.FindElements(By.CssSelector(".profile-drugstore__list .profile-drugstore__list-item"));

            foreach (var item in shopsList)
            {
                var h3 = item.FindElement(By.CssSelector(".h3"));
                if (h3.Text == newShop)
                {
                    var button = item.FindElement(By.CssSelector(".btn-outline-black.btn-more"));

                    ((IJavaScriptExecutor)webDriver).ExecuteScript("arguments[0].scrollIntoView(true);", button);
                    Thread.Sleep(1000);
                    Actions action = new Actions(webDriver);
                    action.MoveToElement(button)
                        .Perform();

                    button.Click();
                }
            }

            ((IJavaScriptExecutor)webDriver).ExecuteScript("window.scrollTo(0, 0);");
            Thread.Sleep(1000);
            var backToButton = webDriver.FindElement(By.CssSelector(".container a.btn-back[href$='ustawienia-konta']"));
            backToButton.Click();

            var currentShop = webDriver.FindElement(By.CssSelector(".settings__card .col-sm-10.col-md-8.col-lg-6 p")).Text;
            StringAssert.AreEqualIgnoringCase(currentShop, newShop);

            webDriver.Quit();
        }
    }
}
