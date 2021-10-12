using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TrenditTests.Enums;

namespace TrenditTests
{
    [Category("User adress book tests")]
    [Author("Maria", "http://maryistesting.com")]
    public class AdressBookTests : BaseTest
    {
        private string _adressName = "nowyAdres";
        private string _rowEditButtonSelector = "[onclick^='javascript:AddressBook.instance().edit']";
        private string _addButtonSelector = "#buttonAdd";
        private string _submitButtonSelector = "#buttonSubmit";
        private string _addressFormSelector = "#formAddressHeader";
        private string _actionConfirmPopUp = ".swal2-header";
        private string _adressInputNameSelector = "[name=alias]";

        private string GetRowXPathSelector(string newAdressName = null)
        {
            string adressName = string.IsNullOrEmpty(newAdressName) ? _adressName : newAdressName;
            return $"//*[contains(text(), '{adressName}')]/ancestor::div[contains(@class,'tabulator-row')]";
        }

        private void DeleteAddressAndClickConfirmButton()
        {
            _webDriver.FindElement(By.CssSelector("[onclick='AddressBook.instance().removeConfirm();']")).Click();

            WaitForElementDisplayed(_actionConfirmPopUp);

            _webDriver.FindElement(By.CssSelector(".swal2-confirm.swal2-styled")).Click();

            WaitForElementDisappeared(_actionConfirmPopUp);
        }

        [Test]
        [Description("Dodanie adresu do książki adresowej - wypełnione tylko pola domyślne i wymagane")]
        public void AddNewAdress_onlyRequiredAndDefaultFieldsFilledTest()
        {
            LogInUserAccount();

            _webDriver.Navigate().GoToUrl($"https://panel.sendit.pl/ksiazka-adresowa");

            //sprawdzić, czy w książce adresów istnieje adres o nazwie, którą chcemy dodać
            try
            {
                var rowElement = _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
                rowElement.FindElement(By.CssSelector(_rowEditButtonSelector))
                    .Click();

                //jeśli jest, to usuń
                DeleteAddressAndClickConfirmButton();
            }
            catch
            {
                //nic nie rób, nie ma takiego adresu
            }

            Thread.Sleep(1000);

            //kliknąć przycisk "Dodaj adres"
            FindAndClick(_addButtonSelector);

            WaitForElementDisplayed(_addressFormSelector);

            //uzupełnić pole "Nazwa własna"
            FindAndSendKeys(_adressInputNameSelector, (_adressName));

            //kliknąć przycisk "Dodaj adres"
            FindAndClick(_submitButtonSelector);

            WaitForElementDisplayed(GetRowXPathSelector(), SearchByTypeEnums.XPath);

            //sprawdzić czy nowy adres został dodany
            var newAdress = _webDriver.FindElements(By.XPath(GetRowXPathSelector()));
            Assert.IsTrue(newAdress.Count == 1);
        }

        [Test]
        [Description("Dodanie adresu do książki adresowej wypełniając tylko wymagane pola, bez typu adresu")]
        public void AddNewAdress_OnlyRequiredFieldsFilledTest()
        {
            LogInUserAccount();

            _webDriver.Navigate().GoToUrl($"https://panel.sendit.pl/ksiazka-adresowa");

            //sprawdzić czy  w książce adresowej jest już adres o nazwie, jaką chcemy dodać
            try
            {
                var rowElement = _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
                rowElement.FindElement(By.CssSelector(_rowEditButtonSelector))
                    .Click();

                //jeśli jest, to usuń
                DeleteAddressAndClickConfirmButton();
            }
            catch
            {
                //nic nie rób, nie ma takiego adresu
            }

            Thread.Sleep(1000);

            //kliknąć przycisk "Dodaj adres"
            FindAndClick(_addButtonSelector);

            WaitForElementDisplayed(_addressFormSelector);

            //uzupełnić pole "Nazwa własna"
            FindAndSendKeys(_adressInputNameSelector, (_adressName));

            //sprawdzić czy zaznaczono checkbox "Adres dostawy"
            try
            {
                _webDriver.FindElement(By.CssSelector("#formItemReceiver:checked"));
                FindAndClick("[for=formItemReceiver]");
            }
            catch
            {
                //nic nie rób, checkbox nie jest zaznaczony
            }
            //sprawdzić czy zaznaczono checkbox "Adres nadania"
            try
            {
                _webDriver.FindElement(By.CssSelector("#formItemSender:checked"));
                FindAndClick("[for=formItemSender]");
            }
            catch
            {
                //nic nie rób, checkbox nie jest zaznaczony
            }

            //kliknąć przycisk "Dodaj adres"
            FindAndClick(_submitButtonSelector);

            WaitForElementDisplayed(GetRowXPathSelector(), SearchByTypeEnums.XPath);

            //znaleźć dodany adres i kliknąć przycysk "Edytuj"
            var rowElement1 = _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
            rowElement1.FindElement(By.CssSelector(_rowEditButtonSelector))
                .Click();

            //sprawdzenie czy pola uzupełniły się domyślnie - czy dodany adres jest adresem nadania i dostawy
            var checkedTypes = _webDriver.FindElements(By.CssSelector("#formAddress #formItemSender:checked, #formAddress #formItemReceiver:checked"));
            Assert.AreEqual(2, checkedTypes.Count);
        }

        [Test]
        [Description("Dodanie adresu do książki adresowej - wszystkie pola uzupełnione")]
        public void AddNewAdress_allFieldsFilledTest()
        {
            LogInUserAccount();

            _webDriver.Navigate().GoToUrl($"https://panel.sendit.pl/ksiazka-adresowa");

            //sprawdzić czy  w książce adresowej jest już adres o nazwie, jaką chcemy dodać
            try
            {
                var rowElement = _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
                rowElement.FindElement(By.CssSelector(_rowEditButtonSelector))
                    .Click();

                //jeśli jest, usuń ten adres
                DeleteAddressAndClickConfirmButton();
            }
            catch
            {
                //nic nie rób, nie ma takiego adresu
            }

            Thread.Sleep(1000);

            //kliknąć przycisk "Dodaj adres"
            FindAndClick(_addButtonSelector);

            WaitForElementDisplayed(_addressFormSelector);

            //uzupełnić pole "Nazwa własna"
            FindAndSendKeys(_adressInputNameSelector, (_adressName));

            //sprawdzić czy zaznaczono checkbox "Adres dostawy"
            try
            {
                _webDriver.FindElement(By.CssSelector("#formItemReceiver:checked"));
            }
            catch
            {
                FindAndClick("[for=formItemReceiver]");
            }

            //uzupełnić pole "Nazwa firmy/Imię i nazwisko"
            FindAndSendKeys("[name=name]", "Jan Kowalski");

            //uzupełnić pole "Nr domu"
            FindAndSendKeys("[name=house_number]", "1");

            //uzupełnić pole "Kod pocztowy"
            FindAndSendKeys("[name=postal_code]", "10-150");

            //uzupełnić pole "Miejscowość"
            FindAndSendKeys("[name=city]", "Zalesie");

            //wybrać radio input Typ adresu - Prywatny
            FindAndClick("[for=orderAddressSenderTypeResidential]");

            //uzupełnić pole "Osoba kontaktowa"
            FindAndSendKeys("[name=contact_person]", "Janina Kowalska");

            //uzupełnić pole "E-mail"
            FindAndSendKeys("[name=email]", "janina@kowalska.pl");

            //uzupełnić pole "Telefon"
            FindAndSendKeys("[name=phone]", "501111222");

            //kliknąć przycisk "Dodaj adres"
            FindAndClick(_submitButtonSelector);

            WaitForElementDisplayed(GetRowXPathSelector(), SearchByTypeEnums.XPath);

            //sprawdzić czy adres został dodany do książki adresowej
            var newAdress = _webDriver.FindElements(By.XPath(GetRowXPathSelector()));
            Assert.IsTrue(newAdress.Count == 1);
        }
    }
}
