using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace TomikTests
{
    public class SearchFilesTests: BaseTest
    {
        public SearchFilesTests()
        {
            _path = "action/SearchFiles";
        }        

        [Test]
        [Category("Search files tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Wyszukiwanie z wyborem z listy rozwijanej")]
        public void Test1()
        {
            RemoveAcceptContainer();

            //1. Uzupełnić pole "Nazwa pliku"
            var searchingInput = _webDriver.FindElement(By.CssSelector("#FileName"));
            searchingInput.SendKeys("sapkowski");

            //2. Wybrać rodzaj plików z listy rozwijanej
            var selectOptions = _webDriver.FindElement(By.CssSelector("#FileType"));
            new SelectElement(selectOptions).SelectByValue("video");            

            //3. Kliknąć w przycisk "Szukaj"           
            var searchingAvatar = _webDriver.FindElement(By.CssSelector("#Search"));
            searchingAvatar.Click();

            Thread.Sleep(1000);

            RemoveAcceptContainer();

            try
            {
                //4. Sprawdzenie czy pojawi się galaria wyników wyszukiwania, (jeśli tak, to wyszukiwarka działa poprawnie)
                _webDriver.FindElement(By.CssSelector("#searchFilesView .filerow"));
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
        public void Test2()
        {
            RemoveAcceptContainer();

            //1. Uzupełnić pole "Nazwa pliku"
            var searchingInput = _webDriver.FindElement(By.CssSelector("#FileName"));
            searchingInput.SendKeys("as");

            //2. Kliknąć w przycisk "Szukaj"           
            var searchingAvatar = _webDriver.FindElement(By.CssSelector("#Search"));
            searchingAvatar.Click();

            //3. Sprawdzenie komunikatu walidacyjnego
            var loginError = _webDriver.FindElement(By.CssSelector("#searchFilesView div:last-child h1"));
            StringAssert.Contains("Wprowadzone zapytanie jest za krótkie", loginError.Text);
        }      
    }
}