using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using System.Linq;

namespace TrenditTests
{
    [Author("Maria", "http://maryistesting.com")]
    [Category("Update user details")]
    public class UserOptionsTests: BaseTest
    {
        private string _popUpSelector = "#lightBoxWhiteBox";
        private string _submitButtonSelector = "input[value=Zapisz]";
        private string _editDefaultSettingsTabSelector = "[href^='javascript:settingsUserBasicInfo']";
        private string _userNameInputSelector = "[name='user[first_name]']";
        private string _userPhoneInputSelector = "[name='user[phone_number]']";

        private static string[] _nameExamples = new string[3]
        {
            "Karol Strzałkowski",
            "Piotr Czajkowski",
            "Ludwik Poniedziałek"
        };

        private string[] _phoneExamples = new string[3]
        {
            "511222333",
            "888777999",
            "600111222"
        };

        [Test]
        [Description("Edycja ustawień konta - edycja nr telefonu")]
        public void CorrectUpdateUserDetails_updatePhone_Test()
        {
            string phoneNumber = "555666667";
            string phoneNumberInputSelector = "[name='customer[phone_number]']";
            string updateUserBasicDataSelector = "a[href='javascript:settingsCompanyBasicInfo();']";

            LogInUserAccount();

            _webDriver.Navigate().GoToUrl(GetPageUrl());

            //kliknąć przycisk Edytuj w sekcji "informacje podstawowe"
            _webDriver.FindElement(By.CssSelector(updateUserBasicDataSelector))
                .Click();

            WaitForElementDisplayed(_popUpSelector);

            //podać nr telefonu
            FindAndSendKeys(phoneNumberInputSelector, phoneNumber);

            //zapisać zmiany
            FindAndClick(_submitButtonSelector);

            //sprawdzić czy zmiany zostały zapisane
            //kliknąć przycisk Edytuj w sekcji "informacje podstawowe"
            FindAndClick(updateUserBasicDataSelector);

            WaitForElementDisplayed(_popUpSelector);

            //sprawdzić nr telefonu
            var phoneInputToCheck = _webDriver.FindElement(By.CssSelector(phoneNumberInputSelector));
            StringAssert.StartsWith(phoneNumber, phoneInputToCheck.GetAttribute("value"));
        }

        [Test]
        [Description("Edycja ustawień konta - edycja sekcji 'Dane do fakturowania'")]
        public void CorrectUpdateUserDetails_updateEmailToSendBills_Test()
        {
            string updateEmailSelector = "a[href='javascript:settingsCompanyInvoiceAddress()']";
            string emailInput = "abc123@op.pl";
            string billsDetailsInputSelector = "[name='invoice[email_invoice]'";

            LogInUserAccount();

            _webDriver.Navigate().GoToUrl(GetPageUrl());

            //kliknąć przycisk Edytuj w sekcji "Dane do fakturowania"
            FindAndClick(updateEmailSelector);

            WaitForElementDisplayed(_popUpSelector);

            FindAndSendKeys(billsDetailsInputSelector, emailInput);

            //kliknąć przycisk "Zapisz"
            FindAndClick(_submitButtonSelector);

            //sprawdzić, czy dane zostały zmienione 
            //kliknąć przycisk Edytuj w sekcji "Dane do fakturowania"
            FindAndClick(updateEmailSelector);

            WaitForElementDisplayed(_popUpSelector);

            //porównać informacje, które zostały wpisane z wyświetlonymi
            var emailToCheck = _webDriver.FindElement(By.CssSelector(billsDetailsInputSelector));
            StringAssert.Contains(emailInput, emailToCheck.GetAttribute("value"));
        }      

        [Test]
        [Description("Edycja moich ustawień domyślnych - edycja nr telefonu")]
        public void CorrectUpdateDefautUserDetails_updateContactPersonAddPhone_Test()
        {
            LogInUserAccount(); 

            _webDriver.Navigate().GoToUrl($"{GetPageUrl()}?userSettings=1");

            //kliknąć przycisk Edytuj w sekcji dane podstawowe
            FindAndClick(_editDefaultSettingsTabSelector);

            WaitForElementDisplayed(_userNameInputSelector);

            //wyczyścić i uzupełnić input Osoba kontaktowa
            var newUserName = FindAndSendKeyFromExamples(_userNameInputSelector, _nameExamples);

            //wyczyścić i uzupełnić pole Telefon kontaktowy
            var newUserPhone = FindAndSendKeyFromExamples(_userPhoneInputSelector, _phoneExamples);

            //kliknąć przycisk Zapisz
            FindAndClick(_submitButtonSelector);

            Thread.Sleep(1500);

            //sprawdzić czy pojawiły się wprowadzone zmiany
            //porównać informacje, które zostały wpisane z wyświetlonymi
            FindAndClick(_editDefaultSettingsTabSelector);

            WaitForElementDisplayed(_userNameInputSelector);

            var personToCheck = _webDriver.FindElement(By.CssSelector(_userNameInputSelector));
            var phoneToCheck = _webDriver.FindElement(By.CssSelector(_userPhoneInputSelector));

            StringAssert.Contains(newUserName, personToCheck.GetAttribute("value"));
            StringAssert.Contains(newUserPhone, phoneToCheck.GetAttribute("value"));
        }

        [Test]
        [Description("Edycja moich ustawień domyślnych - zmiana hasła dostępu")]
        public void CorrectUpdateDefautUserDetails_changePassword_Test()
        {
            string passwordInputSelector = "[name=password]";
            string newPassword = "nowe_haslo!2";
            string errorMessageSelector = "[id='password.error']";

            LogInUserAccount();
            
            _webDriver.Navigate().GoToUrl($"{GetPageUrl()}?userSettings=1");

            //kliknąć przycisk Zmień hasło
            FindAndClick("[href^='javascript:settingsUserChangePassword']");

            WaitForElementDisplayed(passwordInputSelector);

            //uzupełnić pole Nowe hasło
            FindAndSendKeys(passwordInputSelector, newPassword);

            //uzupełnić pole Powtórz hasło
            FindAndSendKeys("[name=password2]", newPassword);

            //kliknąć zapisz
            FindAndClick(_submitButtonSelector);

            Thread.Sleep(1500);

            //kliknąć przycisk Wyloguj
            FindAndClick("[href$='wyloguj']");

            WaitForElementDisplayed(".panel400");

            //sprawdzić możliwość zalogowania z użyciem nowego hasła - jeśli nie da się zalogować wpisując stare hasło, to znaczy, że hasło zostało zmienione
            //wyczyścić i uzupełnić pole Login
            FindAndSendKeys("[name=login]", _appSettings.Login);

            //uzupełnić hasło
            FindAndSendKeys(passwordInputSelector, _appSettings.Password);

            //kliknąć przycisk "Zaloguj się"
            FindAndClick("[type=submit].button100");

            WaitForElementDisplayed(errorMessageSelector);

            //sprawdzić, czy wyświetlił się komunikat "Niepoprawny login lub hasło."
            var errorMessage = _webDriver.FindElement(By.CssSelector(errorMessageSelector));
            StringAssert.Contains("Niepoprawny login lub hasło.", errorMessage.Text);
        }

        private string GetPageUrl() => $"{_appSettings.UserPanelUrl}/ustawienia.php";

        private string FindAndSendKeyFromExamples(string selector, string[] examples)
        {
            var input = _webDriver.FindElement(By.CssSelector(selector));
            string previousValue = input.GetAttribute("value");

            var newValue = examples.FirstOrDefault(x => x != previousValue);
            input.Clear();
            input.SendKeys(newValue);

            return newValue;
        }
    }
}
