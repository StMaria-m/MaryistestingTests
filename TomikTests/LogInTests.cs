using NUnit.Framework;
using OpenQA.Selenium;

namespace TomikTests
{
    public class LogInTests : BaseTest
    {
        private static string _errorMessageContainerSelector = "#loginErrorContent";
        private static string _displayErrorBlockSelector = "#topBarLoginError[style='display: block;']";

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Poprawne logowanie na konto użytkownika")]
        public void CorrectLogInTest()
        {
            LogInSteps();

            try
            {
                //sprawdzenie czy użytkownik jest zalogowany (przycisk wyloguj się pojawił)
                _webDriver.FindElement(By.CssSelector("#logout"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Logout button not found");
            }

            Assert.Pass();
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania za pomocą adresu e-mail")]
        public void IncorrectLogInByEmailTest()
        {
            FillInLoginSteps("abcdef@w.ple");

            ClickLoginButton();

            WaitForAction(_displayErrorBlockSelector);

            //sprawdzenie komunikatu
            var loginError = _webDriver.FindElement(By.CssSelector(_errorMessageContainerSelector));
            StringAssert.Contains("Logowanie za pomocą email nie jest dozwolone - użyj nazwy konta", loginError.Text);
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania bez podania hasła")]
        public void IncorrectLogIn_noPasswordTest()
        {
            FillInLoginSteps("hgdghjhdsfgjfdgg");

            ClickLoginButton();

            WaitForAction(_displayErrorBlockSelector);

            //sprawdzam komunikat walidacyjny formularza - "Niepoprawne dane logowania"
            var loginError = _webDriver.FindElement(By.CssSelector(_errorMessageContainerSelector));
            StringAssert.Contains("Niepoprawne dane logowania", loginError.Text);
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania bez podania danych")]
        public void IncorrectLogIn_noDataTest()
        {
            ClickLoginButton();

            WaitForAction(_displayErrorBlockSelector);

            //sprawdzam komunikat walidacyjny formularza - "Podaj nazwę chomika"
            var loginError = _webDriver.FindElement(By.CssSelector(_errorMessageContainerSelector));
            StringAssert.Contains("Podaj nazwę chomika", loginError.Text);
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Niepoprawne logowanie na konto użytkownika -błędny login i hasło")]
        public void IncorrectLogIn_tooShortLoginTest()
        {
            FillInLoginSteps("a");

            FillInPasswordSteps("a");
            
            ClickLoginButton();

            WaitForAction(_displayErrorBlockSelector);

            //sprawdzenie komunikatu walidacyjnego formularza - "Niepoprawne dane logowania"
            var loginError = _webDriver.FindElement(By.CssSelector(_errorMessageContainerSelector));
            StringAssert.Contains("Niepoprawne dane logowania", loginError.Text);

        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Niepoprawne logowanie na konto użytkownika - logowanie bez uzupełnienia nazwy Chomika")]
        public void IncorrectLogIn_noLoginTest()
        {
            FillInPasswordSteps("abcde");

            ClickLoginButton();

            WaitForAction(_displayErrorBlockSelector);

            //4. Sprawdzenie komunikatu walidacyjnego formularza - "Podaj nazwę chomika"
            var emptyLogin = _webDriver.FindElement(By.CssSelector("#loginErrorContent"));
            StringAssert.Contains("Podaj nazwę chomika", emptyLogin.Text);
        }

        private void FillInLoginSteps(string login)
        {
            var loginInput = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            loginInput.SendKeys(login);
        }

        private void ClickLoginButton()
        {
            var loginButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            loginButton.Click();
        }

        private void FillInPasswordSteps(string password)
        {
            var passwordInput = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            passwordInput.SendKeys(password);
        }
    }
}
