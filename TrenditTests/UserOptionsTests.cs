using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TrenditTests
{
    [Author("Maria", "http://maryistesting.com")]
    [Category("Update user details")]
    public class UserOptionsTests: BaseTest
    {
        string popUpSelector = "#lightBoxWhiteBox";
        string submitButton = "input[value=Zapisz]";

        [Test]
        [Description("Edycja danych osobowych - dodanie nr telefonu")]
        public void CorrectUpdateUserDetails_addPhone_Test()
        {
            string phoneNumber = "555666667";
            string phoneNumberInputSelector = "[name='customer[phone_number]']";
            string updateUserBasicDataSelector = "a[href='javascript:settingsCompanyBasicInfo();']";            

            LogInUserAccount();

            _webDriver.Navigate().GoToUrl($"https://panel.sendit.pl/ustawienia.php");

            //kliknąć przycisk Edytuj w sekcji "informacje podstawowe"
            _webDriver.FindElement(By.CssSelector(updateUserBasicDataSelector))
                .Click();

            WaitForElementDisplayed(popUpSelector);

            //podać nr telefonu
            var phoneInput = _webDriver.FindElement(By.CssSelector(phoneNumberInputSelector));
            phoneInput.Clear();
            phoneInput.SendKeys(phoneNumber);

            //zapisać zmiany
            _webDriver.FindElement(By.CssSelector(submitButton))
                .Click();
           
            //sprawdzić czy zmiany zostały zapisane
            //kliknąć przycisk Edytuj w sekcji "informacje podstawowe"
            _webDriver.FindElement(By.CssSelector(updateUserBasicDataSelector))
                .Click();

            WaitForElementDisplayed(popUpSelector);

            //sprawdzić nr telefonu
            var phoneInputToCheck = _webDriver.FindElement(By.CssSelector(phoneNumberInputSelector));
            StringAssert.StartsWith(phoneNumber, phoneInputToCheck.GetAttribute("value"));
        }
            
        [Test]
        [Description("Edycja danych osobowych - edycja sekcji 'Dane do fakturowania'")]
        public void UpdateEmailToSendBillsTest()
        {
            string updateEmailSelector = "a[href='javascript:settingsCompanyInvoiceAddress()']";
            string emailInput = "abc123@op.pl";

            LogInUserAccount();

            _webDriver.Navigate().GoToUrl($"https://panel.sendit.pl/ustawienia.php");

            //kliknąć przycisk Edytuj w sekcji "Dane do fakturowania"
            var updateBillsDetailsButton = _webDriver.FindElement(By.CssSelector(updateEmailSelector));
            updateBillsDetailsButton.Click();

            WaitForElementDisplayed(popUpSelector);

            var billsDetailsInput = _webDriver.FindElement(By.CssSelector("[name='invoice[email_invoice]'"));
            billsDetailsInput.Clear();
            billsDetailsInput.SendKeys(emailInput);

            //kliknąć przycisk "Zapisz"
            _webDriver.FindElement(By.CssSelector(submitButton)).Click();
            
            //sprawdzić, czy dane zostały zmienione 
            //kliknąć przycisk Edytuj w sekcji "Dane do fakturowania"
            _webDriver.FindElement(By.CssSelector(updateEmailSelector)).Click();

            WaitForElementDisplayed(popUpSelector);
           
            //porównać informacje, które zostały wpisane z wyświetlonymi
            var emailToCheck = _webDriver.FindElement(By.CssSelector("[name='invoice[email_invoice]'"));
            StringAssert.Contains(emailInput, emailToCheck.GetAttribute("value"));
        }
    }
}
