using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using TomikTests.Models;
using TomikTests.Wrappers;

namespace TomikTests
{
    [Parallelizable(ParallelScope.Children)]
    public class SearchFilesTests
    {
        private string _path = "action/SearchFiles";

        private string _inputFileNameSelector = "#FileName";
        private string _selectFileTypeSelector = "#FileType";
        private string _searchButtonSelector = "#Search"; 
        private string _resultContainerSelector = "#searchFilesView";

        protected AppSettingsModel _appSettings;

        [OneTimeSetUp]
        public void PrepareData()
        {
            var appSettingsString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//appsettings.json");
            _appSettings = JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsString);
        }      

        [Test]
        [Category("Search files tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Wyszukiwanie z wyborem z listy rozwijanej")]
        public void CorrectSearchFilesTest()
        {
            var webDriver = OpenBrowser();

            //1. Uzupełnić pole "Nazwa pliku"
            var searchingInput = webDriver.FindElement(By.CssSelector(_inputFileNameSelector));
            searchingInput.SendKeys("sapkowski");

            //2. Wybrać rodzaj plików z listy rozwijanej
            var selectOptions = webDriver.FindElement(By.CssSelector(_selectFileTypeSelector));
            new SelectElement(selectOptions).SelectByValue("video");            

            //3. Kliknąć w przycisk "Szukaj"           
            var searchingAvatar = webDriver.FindElement(By.CssSelector(_searchButtonSelector));
            searchingAvatar.Click();

            Assert.DoesNotThrow(() => webDriver.FindElement(By.CssSelector($"{_resultContainerSelector} .filerow")));

            webDriver.Quit();           
        }

        [Test]
        [Category("Search files tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Wyszukiwanie za krótkiej frazy")]
        public void IncorrectSerachFiles_tooShortFileNameTest()
        {
            var webDriver = OpenBrowser();

            //1. Uzupełnić pole "Nazwa pliku"
            var searchingInput = webDriver.FindElement(By.CssSelector(_inputFileNameSelector));
            searchingInput.SendKeys("as");

            //2. Kliknąć w przycisk "Szukaj"           
            var searchingAvatar = webDriver.FindElement(By.CssSelector(_searchButtonSelector));
            searchingAvatar.Click();

            //3. Sprawdzenie komunikatu walidacyjnego
            var loginError = webDriver.FindElement(By.CssSelector($"{_resultContainerSelector} div:last-child h1"));
            StringAssert.Contains("Wprowadzone zapytanie jest za krótkie", loginError.Text);

            webDriver.Quit();
        }

        private IWebDriver OpenBrowser()
        {
            BrowserWrapper browserWrapper = new BrowserWrapper(_appSettings);
            browserWrapper.CreateWebDriver(_path);
            browserWrapper.AddCookie();
            return browserWrapper.GetWebDriver();
        }
    }
}