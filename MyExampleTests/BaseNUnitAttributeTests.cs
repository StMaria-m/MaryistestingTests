using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MyExampleTests
{
    public class BaseNUnitAttributeTests
    {
        protected IWebDriver _webDriver;

        [SetUp]
        public void SetUp()
        {
            //_webDriver = new ChromeDriver();
            //_webDriver.Url = "https://maryistesting.com";
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _webDriver = new ChromeDriver();
            _webDriver.Url = "https://maryistesting.com";
        }

        [TearDown]
        public void TearDown()
        {
            //_webDriver.Quit();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _webDriver.Quit();
        }

        [Test]
        [Author("Maria")]
        [Category("MyTests")]
        [Order(3)]
        public void Test_1()
        {
            _webDriver.Navigate().GoToUrl("https://maryistesting.com/2021/05/zastosowanie-warunkow-where");

            var authorElement = _webDriver.FindElement(By.CssSelector(".vcard.author b"));
            StringAssert.Contains("Maria", authorElement.Text);
        }

        [Test]
        [Author("Maria")]
        [Category("MyTests")]
        [Order(2)]
        public void Test_2()
        {
            _webDriver.Navigate().GoToUrl("https://maryistesting.com/2021/05/zastosowanie-warunkow-where");

            Assert.DoesNotThrow(() => _webDriver.FindElement(By.CssSelector(".vcard.author b")));
        }

        [Test]
        [Author("Maria")]
        [Category("MyTests")]
        [Order(1)]
        public void Test_3()
        {
            _webDriver.Navigate().GoToUrl("https://maryistesting.com/2021/05/zastosowanie-warunkow-where");

            var authorElements = _webDriver.FindElements(By.CssSelector(".vcard.author b"));
            Assert.AreEqual(1, authorElements.Count);
        }
    }
}
