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
    }
}