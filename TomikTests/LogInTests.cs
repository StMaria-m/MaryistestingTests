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
        public void Test33()
        {
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys("");

            Thread.Sleep(300);

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys("");

            Thread.Sleep(300);

            //3. Uliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            Thread.Sleep(1000);

            try
            {
                //4. Sprawdzenie czy użytkownik jest zalogowany (przycisk wyloguj się pojawił)
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
        public void Test6()
        {
            //1. W polu "Chomik" wpisać nazwę adres e-mail, np. "abcdef@w.ple"
            var loginInput = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            loginInput.SendKeys("abcdef@w.ple");

            Thread.Sleep(300);

            //2. Kliknąć w przycisk Zaloguj
            var loginButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            loginButton.Click();

            Thread.Sleep(300);

            //3. Sprawdzenie komunikatu
            var loginError = _webDriver.FindElement(By.CssSelector("#loginErrorContent"));
            StringAssert.Contains("Logowanie za pomocą email nie jest dozwolone - użyj nazwy konta", loginError.Text);
        }

        [Test]
        [Category("User log in account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania bez podania hasła")]
        public void Test5()
        {
            //1.  W polu nazwa chomika wpisać niepoprawną nazwę np. "hjghjghgjhhjhjg"
            var loginInput = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            loginInput.SendKeys("hjghjghgjhhjhjg");

            //2. kliknąć w przycisk Zaloguj
            var loginButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            loginButton.Click();

            Thread.Sleep(300);

            //co chce sprawdzić/ co chcesz zobaczyć
            //1. Sprawdzam komunika walidacyjny formularza (jaki?) - "Niepoprawne dane logowania"
            var loginError = _webDriver.FindElement(By.CssSelector("#loginErrorContent"));
            StringAssert.Contains("Niepoprawne dane logowania", loginError.Text);
        }

        [Test]
        [Category("User log in account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania bez podania danych")]
        public void Test4()
        {
            //1. kliknąć w przycisk Zaloguj
            var loginButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            loginButton.Click();

            Thread.Sleep(300);

            //co chce sprawdzić/ co chcesz zobaczyć
            //1. Sprawdzam komunika walidacyjny formularza (jaki?) - "Podaj nazwę chomika"
            var loginError = _webDriver.FindElement(By.CssSelector("#loginErrorContent"));
            StringAssert.Contains("Podaj nazwę chomika", loginError.Text);
        }
    }
}
