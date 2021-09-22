using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;

namespace TomikTests
{
   public class HomePageTests: BaseTest
    {
        [Test]
        [Category("Home page tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie przejścia na stronę przypomnienie hasła")]
        public void CorrectGoToLostPasswordPageTest()
        {
            //kliknąć w przycisk "zapomniałem"
            var loginButton = _webDriver.FindElement(By.CssSelector(".forgotPass"));
            loginButton.Click();

            //sprawdzenie przekierowania na stronę do przypomnienia hasła
            var url = _webDriver.Url;
            StringAssert.Contains("LostPassword.aspx", url);
        }

        [Test]
        [Category("User login  account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie działania przycisku Wyloguj")]
        public void CorrectLogOutTest()
        {
            LogInSteps();

            //kliknąć przycisk "Wyloguj"
            var logoutButton = _webDriver.FindElement(By.CssSelector("#logout"));
            logoutButton.Click();

            try
            {
                //sprawdzenie czy pojawi się przycisk "Zaloguj", (jeśli tak, to wylogowanie działa poprawnie)
                _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Button not found");
                return;
            }

            Assert.Pass();
        }
    }
}
