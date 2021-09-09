using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace TomikTests
{
    public class Tests : BaseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Category("Example")]
        [Author("Gal Anonim", "gal1356@siekiera.pl")]
        [Description("Sprawdzenie rejestracji konta")]
        public void Test1()
        {
            IWebElement emailTextField = _webDriver.FindElement(By.XPath(".//*[@class='registration-email']"));
            emailTextField.SendKeys("Test :)");

            Thread.Sleep(3000);
        }


        [Test, Category("Smoke testing")]
        public void Test2()
        {
            IWebElement emailTextField = _webDriver.FindElement(By.XPath(".//*[@class='registration-email']")); //znajdź inputa e-mail
            emailTextField.SendKeys("kijawal609@enamelme.com");                                                     //uzupełnij email

            Thread.Sleep(1000);                                                                                     //poczekaj 1 sek 

            //IWebElement accountNameField = _przegladarka.FindElement(By.XPath(".//*[@class='registration-accountName']"));
            //accountNameField.Clear();
            //accountNameField.SendKeys("Beata_Kozidrak");

            IWebElement registrationPasswordField = _webDriver.FindElement(By.XPath(".//*[@class='registration-password']"));//znajdź inputa hasło
            registrationPasswordField.SendKeys("BB");                                                                           //uzupełnij hasło

            //_przegladarka.FindElement(By.TagName("body")).Click();                                                      //odklikaj inputa hasło
            Thread.Sleep(1000);                                                                                           //poczekaj 1 sek 

            IWebElement passwordErrorField = _webDriver.FindElement(By.XPath(".//*[@class='registration-status-password registrationError']"));

            StringAssert.Contains("Hasło musi posiadać przynajmniej 6 znaków, w tym jedną dużą literę lub cyfrę albo znak specjalny.", passwordErrorField.Text);

            Thread.Sleep(5000);
        }

        [Test, Category("Smoke testing")]
        public void Test3()
        {
            //co użytkownik ma zrobić w krokach
            //1.kliknąć w załóż konto
            var registrationFormButton = _webDriver.FindElement(By.CssSelector("#registrationForm .orangeButtonCSS"));
            registrationFormButton.Click();

            Thread.Sleep(300);

            var registrationFormButton2 = _webDriver.FindElement(By.CssSelector("#registrationForm dd:nth-of-type(2) .field-validation-error"));
            var a = registrationFormButton2.Text;

            //co chce sprawdzić/ co chcesz zobaczyć
            //1. Sprawdzam komunikat walidacyjny dla pola e-mail - Wprowadź adres e-mail            
            var fieldValidationEmailError = _webDriver.FindElement(By.CssSelector(".field-validation-error > span"));
            StringAssert.Contains("Wprowadź adres e-mail", fieldValidationEmailError.Text);

            //2. Sprawdzam komunikat walidacyjny dla pola Nazwa konta - Wprowadź nazwę chomika
            var fieldValidationAccountNameError = _webDriver.FindElement(By.XPath(".//*[@data-valmsg-for='AccountName']"));
            StringAssert.Contains("Wprowadź nazwę chomika", fieldValidationAccountNameError.Text);

            Thread.Sleep(1000);
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
    }
}