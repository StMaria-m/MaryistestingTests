using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;

namespace TomikTests
{
    public class RegistrationAccountTests : BaseTest
    {
        [Test]
        [Category("Registration account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Utworzenie nowego konta użytkownika z poprawnym uzupełnieniem formularza")]
        public void TestRegistration()
        {
            //1. Uzupełnić pole "Twój email"
            var registrationEmail = _webDriver.FindElement(By.CssSelector(".registration-email"));
            registrationEmail.SendKeys("qwertyu@qw.ou");

            Thread.Sleep(300);

            //2. Uzupełnić pole "Nazwa konta"
            var accountName = _webDriver.FindElement(By.CssSelector(".registration-accountName"));
            accountName.SendKeys("qwertyu");

            Thread.Sleep(300);

            //3.Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector(".registration-password"));
            password.SendKeys("zaq1!1");

            Thread.Sleep(300);

            //4.Kliknąć "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#registrationForm .orangeButtonCSS"));
            createButton.Click();

            Thread.Sleep(1000);

            //5.Zaznaczyć potwierdzenie wymaganego wieku
            var ageField = _webDriver.FindElement(By.CssSelector("#AgeConfirmed"));
            ageField.Click();

            //6.Zaznaczyć akceptację regulaminu 
            var termsField = _webDriver.FindElement(By.CssSelector("#TermsAccepted"));
            termsField.Click();

            //7.Kliknąć "Dalej"
            var nextButton = _webDriver.FindElement(By.CssSelector("#registrationForm .orangeButtonCSS"));
            nextButton.Click();

            Thread.Sleep(1000);

            try
            {
                //8. Sprawdzenie czy pojawi się Captcha (jeśli tak, to można przyjąć, że założenie konta się powiodło)
                _webDriver.FindElement(By.CssSelector(".captcha-container"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Captcha not found");
            }

            Assert.Pass();
        }

        [Test]
        [Category("Registration account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Użycie za słabego hasła podczas rejestracji nowego użytkownika")]
        public void TestRegistration1()
        {
            //1.Uzupełnić pole "Twój e-mail"
            var registrationEmail = _webDriver.FindElement(By.CssSelector(".registration-email"));
            registrationEmail.SendKeys("qwertyu@qw.ou");

            Thread.Sleep(300);

            //2.Uzupełnić pole "Nazwa konta"
            var accountName = _webDriver.FindElement(By.CssSelector(".registration-accountName"));
            accountName.SendKeys("qwertyu");

            Thread.Sleep(300);

            //3.Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector(".registration-password"));
            password.SendKeys("111111");

            Thread.Sleep(1300);

            //4. Sprawdzić komunikat walidacyjny formularza
            var loginError = _webDriver.FindElement(By.CssSelector(".registration-status-password.registrationError"));
            StringAssert.Contains("Ustalone przez Ciebie hasło jest jednym z popularnych, prostych haseł. Ustal inne hasło, aby mieć pewność, że Twoje konto będzie bezpieczne.", loginError.Text);
        }

        [Test]
        [Category("Registration account tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Użycie hasła niezgodnego z walidacją podczas rejestracji nowego użytkownika")]
        public void TestRegistration2()
        {
            //1.Uzupełnić pole "Twój e-mail"
            var registrationEmail = _webDriver.FindElement(By.CssSelector(".registration-email"));
            registrationEmail.SendKeys("qwertyu@qw.ou");

            Thread.Sleep(300);

            //2.Uzupełnić pole "Nazwa konta"
            var accountName = _webDriver.FindElement(By.CssSelector(".registration-accountName"));
            accountName.SendKeys("qwertyu");

            Thread.Sleep(300);

            //3.Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector(".registration-password"));
            password.SendKeys("aaaaaaa");

            Thread.Sleep(1300);

            //4. Sprawdzić komunikat walidacyjny formularza
            var passwordError = _webDriver.FindElement(By.CssSelector(".registration-status-password"));
            StringAssert.Contains("Hasło musi posiadać przynajmniej 6 znaków, w tym jedną dużą literę lub cyfrę albo znak specjalny.", passwordError.Text);
        }       
    }
}
