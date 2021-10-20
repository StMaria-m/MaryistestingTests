using NUnit.Framework;
using OpenQA.Selenium;

namespace TrenditTests
{
    public class LogInTests : BaseTest
    {
        string submitButton = "[type=submit].button100";
        string loginSelector = "[name=login]";
        string wrongLoginInput = "mtest@vp.pl";
        string errorMessageSelector = "#formLogin span.error";
        string passwordSelector = "[name=password]";

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Logowanie bez loginu i hasła")]
        public void IncorrectLoginTest_noLoginAndNoPassword()
        {
            GoToLoginPage();

            //kliknąć przycisk "Zaloguj się"
            _webDriver.FindElement(By.CssSelector(submitButton)).Click();

            //Sprawdzenie, czy pojawią się dwa komunikaty "Niepoprawny login lub hasło."
            var noData = _webDriver.FindElements(By.CssSelector(errorMessageSelector));
            Assert.AreEqual(2, noData.Count);
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Błędne login i haslo - logowanie")]
        public void IncorrectLoginTest_wrongLoginAndPasswordLogInTest()
        {
            GoToLoginPage();

            //uzupełnić Login
            _webDriver.FindElement(By.CssSelector(loginSelector)).SendKeys(wrongLoginInput);

            //uzupełnić hasło
            _webDriver.FindElement(By.CssSelector(passwordSelector)).SendKeys("1111");

            //kliknąć przycisk "Zaloguj się"
            _webDriver.FindElement(By.CssSelector(submitButton)).Click();

            //Sprawdzenie, czy pojawią się dwa komunikaty "Niepoprawny login lub hasło."   
            var noData = _webDriver.FindElements(By.CssSelector(errorMessageSelector));
            Assert.AreEqual(2, noData.Count);
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Błędne haslo - logowanie")]
        public void IncorrectLoginTest_noPasswordLogInTest()
        {
            string errorPasswordMessageSelector = "[id='password.error']";

            GoToLoginPage();

            //uzupełnić Login
            _webDriver.FindElement(By.CssSelector(loginSelector)).SendKeys(_appSettings.Login);

            //kliknąć przycisk "Zaloguj się"
            _webDriver.FindElement(By.CssSelector(submitButton)).Click();

            WaitForElementDisplayed(errorPasswordMessageSelector);

            //Sprawdzenie, czy pojawi się komunikat "Pole jest wymagane."   
            var noPasswordMessage = _webDriver.FindElements(By.CssSelector(errorPasswordMessageSelector));
            Assert.IsTrue(noPasswordMessage.Count == 1);
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Poprawne logowanie")]
        public void LogInTest()
        {
            LogInUserAccount();

            //sprawdzenie
            var logOutButton = _webDriver.FindElements(By.CssSelector("a[href='/wyloguj']"));
            Assert.IsTrue(logOutButton.Count > 0);
        }

        private void GoToLoginPage() => _webDriver.Navigate().GoToUrl(_appSettings.LoginFormUrl);
    }
}
