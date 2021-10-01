using NUnit.Framework;
using OpenQA.Selenium;

namespace TrenditTests
{
    public class RegistrationTests: BaseTest
    {
        [Test]
        [Category("User log in tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Poprawna rejestracja")]
        public void UserAccountRegistrationTest()
        {
            string eMail = "test@test.pl";
            string termsConsentSelector = "#formItemAgreement5";
            string privacyPolicySelector = "#formItemAgreement6";
                        
            //kliknąć Utwórz konto
            var createAccount = _webDriver.FindElement(By.CssSelector("a[rel=nofollow]"));
            createAccount.Click();

            WaitForElementDisplayed("#formSectionFields");

            //wpisać e-mail
            var emailInput = _webDriver.FindElement(By.CssSelector("[name=login]"));
            emailInput.SendKeys(eMail);

            //powtórzyć email
            var emailInput2 = _webDriver.FindElement(By.CssSelector("[name=login2]"));
            emailInput2.SendKeys(eMail);            

            try
            {
                //sprawdzić zaznaczenie checkboxa "TAK, akceptuję Regulamin. *" jeśli jest zaznaczony, nic nie rób
                _webDriver.FindElement(By.CssSelector($"{termsConsentSelector}:checked"));
            }
            catch
            {
                //jeśli nie jest zaznaczony, zaznacz
                var termsAgree = _webDriver.FindElement(By.CssSelector($"{termsConsentSelector} + label"));
                termsAgree.Click();
            }

            try
            {
                //sprawdzić zaznaczenie checkboxa "TAK, zapoznałem się/zapoznałam się z Polityką Prywatności. *" jeśli jest zaznaczony, nic nie rób
                _webDriver.FindElement(By.CssSelector($"{privacyPolicySelector}:checked"));
            }
            catch
            {
                var privacyTermsAgree = _webDriver.FindElement(By.CssSelector($"{privacyPolicySelector} + label"));
                privacyTermsAgree.Click();
            }

            //kliknąć "ZAREJESTRUJ SIĘ"
            var registrationButton = _webDriver.FindElement(By.CssSelector("[type=submit].button100"));
            registrationButton.Click();

            WaitForElementDisplayed(".panel800.apaczkaForm");

            Assert.DoesNotThrow(() => _webDriver.FindElement(By.CssSelector(".panel800.apaczkaForm")));
        }
    }
}
