using NUnit.Framework;
using OpenQA.Selenium;

namespace OliksTests
{
    public class LogInTests : BaseTest
    {
        private string _myAccountLinkSelector = "#topLoginLink";
        private string _logInFormSelector = "#loginForm";
        private string _errorMessageSelector = "#se_emailError";
        private string _loginFormErrorMessageSellector = "#loginForm > div.errorbox > p";
        private string _logInButtonSelector = "#se_userLogin";
        private string _userLoginInputSelector = "#userEmail";
        private string _userPasswordInputSelector = "#userPass";


        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Poprawne logowanie na konto użytkownika")]
        public void CorrectLogInTest()
        {
            LogInSteps();

            try
            {
                //Sprawdzenie czy użytkownik został zalogowany
                _webDriver.FindElement(By.CssSelector("#se_accountShop"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("User is not log in");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Brak loginu i hasła - niepoprawne logowanie na konto użytkownika")]
        public void IncorrectLog_noLoginAndPasswordTest()
        {
            ClickAcceptContainer();

            ClickMyAccount();

            WaitForAction(_logInFormSelector);

            //kliknać "Zaloguj się"
            var logInButton = _webDriver.FindElement(By.CssSelector(_logInButtonSelector));
            logInButton.Click();

            //sprawdzić, czy pojawi się komunikat "To pole jest wymagane"
            var emptyPasswordInput = _webDriver.FindElement(By.CssSelector(_errorMessageSelector));
            StringAssert.Contains("To pole jest wymagane", emptyPasswordInput.Text);
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Błędny format e-maila - niepoprawne logowanie na konto użytkownika")]
        public void IncorrectLogIn_wrongLoginTest()
        {
            string userMail = "23423423test";

            ClickAcceptContainer();

            ClickMyAccount();

            WaitForAction(_logInFormSelector);

            //uzupełnić "E-mail do konta OLX"
            var userEmailInput = _webDriver.FindElement(By.CssSelector(_userLoginInputSelector));
            userEmailInput.SendKeys(userMail);

            _webDriver.FindElement(By.TagName("body")).Click();

            WaitForAction(_errorMessageSelector);

            //sprawdzić, czy pojawi się komunikat "Niepoprawny format e-mail"
            var emptyPasswordInput = _webDriver.FindElement(By.CssSelector(_errorMessageSelector));
            StringAssert.Contains("Niepoprawny format e-mail", emptyPasswordInput.Text);
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Błędny login o poprawnym formacie i hasło - niepoprawne logowanie na konto użytkownika")]
        public void IncorrectLogIn_wrongLoginAndPasswordTest()
        {
            string userMail = "123abc@vp.pl";
            string userPassword = "abcdef";

            ClickAcceptContainer();

            ClickMyAccount();

            WaitForAction(_logInFormSelector);

            //uzupełnić "E-mail do konta OLX"
            var userEmailInput = _webDriver.FindElement(By.CssSelector(_userLoginInputSelector));
            userEmailInput.SendKeys(userMail);

            //uzupełnić hasło
            var userPasswordInput = _webDriver.FindElement(By.CssSelector(_userPasswordInputSelector));
            userPasswordInput.SendKeys(userPassword);

            //kliknać "Zaloguj się"
            var logInButton = _webDriver.FindElement(By.CssSelector(_logInButtonSelector));
            logInButton.Click();

            WaitForAction(_loginFormErrorMessageSellector);

            //sprawdzić, czy pojawi się komunikat "Nieprawidłowy login lub hasło"
            var logInError = _webDriver.FindElement(By.CssSelector(_loginFormErrorMessageSellector));
            StringAssert.Contains("Nieprawidłowy login lub hasło", logInError.Text);
        }

        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Błędna domena e-maila - niepoprawne logowanie na konto użytkownika")]
        public void IncorrectLogIn_wrongEmailDomainTest()
        {
            string usermail = "123abc@az.com";
            string password = "1234zaqz";

            ClickAcceptContainer();

            ClickMyAccount();

            WaitForAction(_logInFormSelector);

            //uzupełnić "E-mail do konta OLX"
            var userEmailInput = _webDriver.FindElement(By.CssSelector(_userLoginInputSelector));
            userEmailInput.SendKeys(usermail);

            //uzupełnić hasło
            var userPasswordInput = _webDriver.FindElement(By.CssSelector(_userPasswordInputSelector));
            userPasswordInput.SendKeys(password);

            //kliknać "Zaloguj się"
            var logInButton = _webDriver.FindElement(By.CssSelector(_logInButtonSelector));
            logInButton.Click();

            WaitForAction(_errorMessageSelector);

            //sprawdzić, czy pojawi się komunikat "E-mail w tej domenie nie jest dozwolony przy zakładaniu konta na OLX. Spróbuj jeszcze raz przy użyciu innego e-maila lub skontaktuj się z naszym Centrum pomocy."
            var wrongDomain = _webDriver.FindElement(By.CssSelector(_errorMessageSelector));
            StringAssert.Contains("E-mail w tej domenie nie jest dozwolony przy zakładaniu konta na OLX. Spróbuj jeszcze raz przy użyciu innego e-maila lub skontaktuj się z naszym Centrum pomocy.", wrongDomain.Text);
        }
        
        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Brak loginu i hasła - niepoprawne logowanie na konto użytkownika")]
        public void CorrectLogInTestMW()
        {
            ClickAcceptContainer();

            ClickMyAccount();

            WaitForAction(_logInFormSelector);

            //kliknać "Zaloguj się"
            var logInButton = _webDriver.FindElement(By.CssSelector(_logInButtonSelector));
            logInButton.Click();

            //sprawdzić, czy pojawi się komunikat "To pole jest wymagane"
            var emptyPasswordInput = _webDriver.FindElement(By.CssSelector(_errorMessageSelector));
            StringAssert.Contains("To pole jest wymagane", emptyPasswordInput.Text);
        }

        private void ClickMyAccount()
        {
            var loginLink = _webDriver.FindElement(By.CssSelector(_myAccountLinkSelector));
            loginLink.Click();
        }
    }
}
