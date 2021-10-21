using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;

namespace TomikTests
{
    public class LostPasswordTests: BaseTest
    {
        public LostPasswordTests()
        {
            _path = "LostPassword.aspx";
        }

        [Test]
        [Category("Lost password tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania z przypomnieniem hasła")]
        public void Test2()
        {
            RemoveAcceptContainer();

            //1. Kliknąć w przycisk "zapomniałem"
            var loginButton = _webDriver.FindElement(By.CssSelector(".forgotPass"));
            loginButton.Click();

            Thread.Sleep(300);

            //2.Uzupełnić e-mail, na który ma być wysłane przypomnienie hasła
            var useremail = _webDriver.FindElement(By.CssSelector("#ctl00_CT_txtEmailAddress"));
            useremail.SendKeys(_appSettings.Login);

            try
            {
                //3. Sprawdzenie czy pojawi się Captcha (jeśli tak, to można przyjąć, że link resetujący hasło został wysłany)
                _webDriver.FindElement(By.CssSelector("#ctl00_CT_CaptchaEntered"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Captcha not found");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("Lost passwors tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie logowania z przypomnieniem hasła i wpisaniem błędnego adresu e-mail")]
        public void Test3()
        {
            RemoveAcceptContainer();

            //1.Wpisać błędny format adresu e-mail, na który ma być wysłane przypomnienie hasła, np. "abcabc.pl"
            var useremail = _webDriver.FindElement(By.CssSelector("#ctl00_CT_txtEmailAddress"));
            useremail.SendKeys("abcabc.pl");

            _webDriver.FindElement(By.TagName("body")).Click();

            //2. Sprawdzenie komunikatu walidacyjnego formularza
            var emailError = _webDriver.FindElement(By.CssSelector("#ctl00_CT_revEmailValidator"));
            StringAssert.Contains("Format adresu e-mail jest nieprawidłowy", emailError.Text);
        }
    }
}
