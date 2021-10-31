using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace TomikTests
{
    public class SearchFilesTests: BaseTest
    {
        private string _inputFileNameSelector = "#FileName";
        private string _selectFileTypeSelector = "#FileType";
        private string _searchButtonSelector = "#Search"; 
        private string _resultContainerSelector = "#searchFilesView";

        public SearchFilesTests()
        {
            _path = "action/SearchFiles";
        }        

        [Test]
        [Category("Search files tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Wyszukiwanie z wyborem z listy rozwijanej")]
        public void CorrectSearchFilesTest()
        {
            //1. Uzupełnić pole "Nazwa pliku"
            var searchingInput = _webDriver.FindElement(By.CssSelector(_inputFileNameSelector));
            searchingInput.SendKeys("sapkowski");

            //2. Wybrać rodzaj plików z listy rozwijanej
            var selectOptions = _webDriver.FindElement(By.CssSelector(_selectFileTypeSelector));
            new SelectElement(selectOptions).SelectByValue("video");            

            //3. Kliknąć w przycisk "Szukaj"           
            var searchingAvatar = _webDriver.FindElement(By.CssSelector(_searchButtonSelector));
            searchingAvatar.Click();

            try
            {
                //4. Sprawdzenie czy pojawi się galaria wyników wyszukiwania, (jeśli tak, to wyszukiwarka działa poprawnie)
                _webDriver.FindElement(By.CssSelector($"{_resultContainerSelector} .filerow"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("List not found");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("Search files tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Wyszukiwanie za krótkiej frazy")]
        public void IncorrectSerachFiles_tooShortFileNameTest()
        {
            //1. Uzupełnić pole "Nazwa pliku"
            var searchingInput = _webDriver.FindElement(By.CssSelector(_inputFileNameSelector));
            searchingInput.SendKeys("as");

            //2. Kliknąć w przycisk "Szukaj"           
            var searchingAvatar = _webDriver.FindElement(By.CssSelector(_searchButtonSelector));
            searchingAvatar.Click();

            //3. Sprawdzenie komunikatu walidacyjnego
            var loginError = _webDriver.FindElement(By.CssSelector($"{_resultContainerSelector} div:last-child h1"));
            StringAssert.Contains("Wprowadzone zapytanie jest za krótkie", loginError.Text);
        }      
    }
}