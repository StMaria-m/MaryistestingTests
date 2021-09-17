using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;

namespace TomikTests
{
    public class LogInTests : BaseTest
    {
        [Test]
        [Category("User login  account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Poprawne logowanie na konto użytkownika")]
        public void Test1()
        {
            LogInSteps();

            try
            {
                // Sprawdzenie czy użytkownik jest zalogowany (przycisk wyloguj się pojawił)
                _webDriver.FindElement(By.CssSelector("#logout"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Logout button not found");
            }

            Assert.Pass();
        }

        [Test]
        [Category("User log in account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania za pomocą adresu e-mail")]
        public void Test2()
        {
            FillInLoginSteps("abcdef@w.ple");

            ClickLoginButton();

            Thread.Sleep(300);

            //3. Sprawdzenie komunikatu
            var loginError = _webDriver.FindElement(By.CssSelector("#loginErrorContent"));
            StringAssert.Contains("Logowanie za pomocą email nie jest dozwolone - użyj nazwy konta", loginError.Text);
        }

        [Test]
        [Category("User log in account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania bez podania hasła")]
        public void Test3()
        {
            FillInLoginSteps("hgdghjhdsfgjfdgg");

            ClickLoginButton();

            Thread.Sleep(300);

            //3. Sprawdzam komunikat walidacyjny formularza (jaki?) - "Niepoprawne dane logowania"
            var loginError = _webDriver.FindElement(By.CssSelector("#loginErrorContent"));
            StringAssert.Contains("Niepoprawne dane logowania", loginError.Text);
        }

        [Test]
        [Category("User log in account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania bez podania danych")]
        public void Test4()
        {
            ClickLoginButton();

            Thread.Sleep(300);

            //2. Sprawdzam komunikat walidacyjny formularza (jaki?) - "Podaj nazwę chomika"
            var loginError = _webDriver.FindElement(By.CssSelector("#loginErrorContent"));
            StringAssert.Contains("Podaj nazwę chomika", loginError.Text);
        }

        [Test]
        [Category("User login  account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Niepoprawne logowanie na konto użytkownika -błędny login i hasło")]
        public void Test1a()
        {
            FillInLoginSteps("a");

            FillInPasswordSteps("a");
            
            ClickLoginButton();

            Thread.Sleep(500);

            //4. Sprawdzenie komunikatu walidacyjnego formularza - "Niepoprawne dane logowania"
            var loginError = _webDriver.FindElement(By.CssSelector("#loginErrorContent"));
            StringAssert.Contains("Niepoprawne dane logowania", loginError.Text);

        }

        [Test]
        [Category("User login  account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Niepoprawne logowanie na konto użytkownika - logowanie bez uzupełnienia nazwy Chomika")]
        public void Test1ab()
        {
            FillInPasswordSteps("abcde");

            ClickLoginButton();

            Thread.Sleep(500);

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
