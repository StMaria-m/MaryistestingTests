using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyExampleTests
{
    [Category("MyBlogPosts")]
    [Description("https://maryistesting.com/2021/10/findelement-vs-findelements-roznice-i-sposoby-obsluzenia")]
    public class FindElementVsFindElementsTests
    {
        protected IWebDriver _webDriver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _webDriver = new ChromeDriver();
            _webDriver.Url = "https://maryistesting.com";
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _webDriver.Quit();
        }

        [Test]
        [Description("Sprawdzenie czy element istnieje")]
        public void CheckIfElementExistingTest()
        {
            _webDriver.Navigate().GoToUrl("https://maryistesting.com/2021/05/zastosowanie-warunkow-where");

            By cssSelector = By.CssSelector(".vcard.author b");

            //FindElement
            //Metoda DoesNotThrow oczekuje, że wyjątek nie został zwrócony. Jeżeli wyjątek wystąpił to wynik będzie negatywny
            Assert.DoesNotThrow(() => _webDriver.FindElement(cssSelector));

            //Za pomocą konstrukcji try-catch przypisujemy do zmiennej result informację o tym czy element został znaleziony czy też nie
            bool result;
            try
            {
                _webDriver.FindElement(cssSelector);
                result = true;
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                result = false;
            }
            Assert.IsTrue(result);

            //FindElements
            //Wykorzystujemy metodę Any do sprawdzenia czy w kolekcji znajduje się jakikolwiek element
            Assert.IsTrue(_webDriver.FindElements(cssSelector).Any());

            //Sprawdzamy czy w kolekcji znajduje się 0 elementów
            Assert.IsTrue(_webDriver.FindElements(cssSelector).Count == 0);
        }

        [Test]
        [Description("Sprawdzenie czy tekst wybranego elementu zawiera poprawnego autora")]
        public void CheckIfAuthorIsCorrectTest()
        {
            _webDriver.Navigate().GoToUrl("https://maryistesting.com/2021/05/zastosowanie-warunkow-where");

            By cssSelector = By.CssSelector(".vcard.author b");
            IWebElement authorElement;

            //FindElement
            //Najpierw wyszukujemy element, jeżeli go nie znajdziemy, zostanie zwrócony wyjątek
            //jeżeli znajdziemy element będziemy mieć możliwość sprawdzenia tekstu
            authorElement = _webDriver.FindElement(cssSelector);
            StringAssert.Contains("Maria", authorElement.Text);

            //Najpierw wyszukujemy element, jeżeli go nie znajdziemy,
            //zostanie zwrócony wyjątek jeżeli znajdziemy element będziemy mieć możliwość sprawdzenia tekstu
            try
            {
                authorElement = _webDriver.FindElement(cssSelector);
                StringAssert.Contains("Maria", authorElement.Text);
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("IWebElement not found");
            }

            //FindElements
            //W tej konstrukcji unikamy sytuacji z pojawiąjącym się wyjątkiem używając konstrukcji if 
            //Z listy elementów pobieramy pierwszy element lub element domyślny, jeżeli kolekcja jest pusta
            //Konstrukcja if sprawdza czy element jest null-em
            authorElement = _webDriver.FindElements(cssSelector).FirstOrDefault();
            if (authorElement == null)
            {
                Assert.Fail("IWebElement not found");
            }

            StringAssert.Contains("Maria", authorElement.Text);
        }
    }
}