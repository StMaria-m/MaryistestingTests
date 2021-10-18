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

        private string _url = "https://panel.sendit.pl/ksiazka-adresowa";

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

            _webDriver.Navigate().GoToUrl(_url);

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

            _webDriver.Navigate().GoToUrl(_url);

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

            _webDriver.Navigate().GoToUrl(_url);

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

        [Test]
        [Description("Usunięcie adresu z książki adresowej")]
        public void RemoveAdressTest()
        {
            LogInUserAccount();

            _webDriver.Navigate().GoToUrl(_url);

            //sprawdzić czy  w książce adresowej istnieje adres, który chcemy usunąć
            try
            {
                _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
            }
            catch
            {
                //jeśli nie ma, to dodać
                //kliknąć przycisk "Dodaj adres"
                FindAndClick(_addButtonSelector);

                WaitForElementDisplayed(_addressFormSelector);

                //uzupełnić pole "Nazwa własna"
                FindAndSendKeys(_adressInputNameSelector, _adressName);

                //kliknąć przycisk "Dodaj adres"
                FindAndClick(_submitButtonSelector);

                WaitForElementDisplayed(GetRowXPathSelector(), SearchByTypeEnums.XPath);
            }

            //znaleźć adres i kliknąć przycisk "Edytuj"
            var rowElement = _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
            rowElement.FindElement(By.CssSelector(_rowEditButtonSelector))
                .Click();

            //kliknąć przycisk "Usuń adres"
            FindAndClick("[onclick='AddressBook.instance().removeConfirm();']");

            WaitForElementDisplayed(_actionConfirmPopUp);

            FindAndClick(".swal2-confirm.swal2-styled");


            WaitForElementDisappeared(_actionConfirmPopUp);

            //sprawdzenie czy adres został usunięty
            Assert.Throws<NoSuchElementException>(() => _webDriver.FindElement(By.XPath(GetRowXPathSelector())));
        }

        [Test]
        [Description("Wyszukiwanie adresu w książce adresowej we wszystkich typach adresów")]
        public void FindAdressInAdressBookTest()
        {
            LogInUserAccount();

            _webDriver.Navigate().GoToUrl(_url);

            try
            {
                //sprawdzić, czy w książce adresowej istnieje wyszukiwany adres
                _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
            }
            catch
            {
                //jeśli nie ma, to trzeba dodać
                //kliknąć przycisk "Dodaj adres"
                FindAndClick(_addButtonSelector);

                WaitForElementDisplayed(_addressFormSelector);

                //uzupełnić pole "Nazwa własna"
                FindAndSendKeys(_adressInputNameSelector, _adressName);

                //kliknąć przycisk "Dodaj adres"
                FindAndClick(_submitButtonSelector);

                WaitForElementDisplayed(GetRowXPathSelector(), SearchByTypeEnums.XPath);
            }

            //uzupełnić pole "Szukaj..."
            FindAndClick("[for=searchAny]");
            FindAndSendKeys("[name=searchQuery]", _adressName);

            //kliknąć w sekcję body (brak przycisku "Wyszukaj")
            FindAndClick("body");

            //sprawdzić, czy w wynikach wyszukiwania pojawił się wyszukiwany adres
            Assert.DoesNotThrow(() => _webDriver.FindElement(By.XPath(GetRowXPathSelector())));
        }

        [Test]
        [Description("Edycja istniejącego adresu - zmiana nazwy")]
        public void UpdateAdress_changeAdressNameTest()
        {
            string newAdressName = "nowa nazwa adresu";
            LogInUserAccount();

            _webDriver.Navigate().GoToUrl(_url);

            try
            {
                //sprawdzić, czy w książce adresowej istnieje wyszukiwany adres
                _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
            }
            catch
            {
                //jeśli nie ma, to trzeba dodać
                //kliknąć przycisk "Dodaj adres"
                FindAndClick(_addButtonSelector);

                WaitForElementDisplayed(_addressFormSelector);

                //uzupełnić pole "Nazwa własna"
                FindAndSendKeys(_adressInputNameSelector, _adressName);

                //kliknąć przycisk "Dodaj adres"
                FindAndClick(_submitButtonSelector);

                WaitForElementDisplayed(GetRowXPathSelector(), SearchByTypeEnums.XPath);
            }

            //sprawdzić, czy w książce adresowej istnieje adres o nazwie docelowej
            //znaleźć adres i kliknąć przycisk "Edytuj" i wypierdolić (usunąć)            
            try
            {
                var rowElement2 = _webDriver.FindElement(By.XPath(GetRowXPathSelector(newAdressName)));
                rowElement2.FindElement(By.CssSelector(_rowEditButtonSelector))
                    .Click();

                //kliknąć przycisk "Usuń adres"
                FindAndClick("[onclick='AddressBook.instance().removeConfirm();']");

                WaitForElementDisplayed(_actionConfirmPopUp);

                FindAndClick(".swal2-confirm.swal2-styled");


                WaitForElementDisappeared(_actionConfirmPopUp);
            }
            catch
            {
                //nic nie rób, nie ma takiego adresu 
            }

            //kliknąć przycisk "Edytuj"
            var rowElement = _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
            rowElement.FindElement(By.CssSelector(_rowEditButtonSelector))
                .Click();

            WaitForElementDisplayed(_addressFormSelector);

            var beforeAddressName = _webDriver.FindElement(By.CssSelector(_adressInputNameSelector)).GetAttribute("value");

            //uzupełnić pole "Nazwa własna"
            var newNameAdress = _webDriver.FindElement(By.CssSelector(_adressInputNameSelector));
            newNameAdress.Clear();
            newNameAdress.SendKeys(newAdressName);

            //klikąć "Zapisz zmiany"
            FindAndClick(_submitButtonSelector);

            WaitForElementDisappeared(_actionConfirmPopUp);

            ////sprawdzić, czy nazwa adresu została zmieniona
            ////znaleźć dodany adres i kliknąć przycysk "Edytuj"
            rowElement = _webDriver.FindElement(By.XPath(GetRowXPathSelector(newAdressName)));
            rowElement.FindElement(By.CssSelector(_rowEditButtonSelector))
                .Click();

            WaitForElementDisplayed(_addressFormSelector);

            string afterAddressName = _webDriver.FindElement(By.CssSelector(_adressInputNameSelector)).GetAttribute("value");

            //sprawdzić, czy nazwa adresu została zmieniona
            StringAssert.DoesNotStartWith(afterAddressName, beforeAddressName);
        }

        [Test]
        [Description("Edycja istniejącego adresu - zmiana typu adresu")]
        public void UpdateAdress_changeAdressTypeInAdressBookTest()
        {
            LogInUserAccount();

            _webDriver.Navigate().GoToUrl(_url);

            try
            {
                //sprawdzić, czy w książce adresowej istnieje wyszukiwany adres
                _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
            }
            catch
            {
                //jeśli nie ma, to trzeba dodać
                //kliknąć przycisk "Dodaj adres"
                FindAndClick(_addButtonSelector);

                WaitForElementDisplayed(_addressFormSelector);

                //uzupełnić pole "Nazwa własna"
                FindAndSendKeys(_adressInputNameSelector, _adressName);

                //kliknąć przycisk "Dodaj adres"
                FindAndClick(_submitButtonSelector);

                WaitForElementDisplayed(GetRowXPathSelector(), SearchByTypeEnums.XPath);
            }

            //kliknąć przycisk "Edytuj"
            var rowElement = _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
            rowElement.FindElement(By.CssSelector(_rowEditButtonSelector))
                .Click();

            WaitForElementDisplayed(_addressFormSelector);

            //sprawdzić czy zaznaczono checkbox "Adres nadania"
            bool beforeShippmentAddressCheckboxSelected = _webDriver.FindElement(By.CssSelector("#formItemSender"))
                .Selected;
            bool beforeDeliveryAddressCheckboxSelected = _webDriver.FindElement(By.CssSelector("#formItemReceiver"))
                .Selected;

            if (beforeShippmentAddressCheckboxSelected && beforeDeliveryAddressCheckboxSelected)
            {
                FindAndClick("[for=formItemSender]");
            }
            else
            {
                if (beforeShippmentAddressCheckboxSelected == false)
                {
                    FindAndClick("[for=formItemSender]");
                }
                else if (beforeDeliveryAddressCheckboxSelected == false)
                {
                    FindAndClick("[for=formItemReceiver]");
                }
            }

            //kliknąć przycisk "Zapisz zmiany"
            FindAndClick(_submitButtonSelector);

            Thread.Sleep(1000);

            //sprawdzić, czy typ adresu został zmieniony
            //znaleźć dodany adres i kliknąć przycysk "Edytuj"
            var rowElement1 = _webDriver.FindElement(By.XPath(GetRowXPathSelector()));
            rowElement1.FindElement(By.CssSelector(_rowEditButtonSelector))
                .Click();

            //sprawdzenie czy zmieniono typ adresu - czy dodany adres jest adresem nadania i dostawy
            var afterShippmentAddressCheckboxSelected = _webDriver.FindElement(By.CssSelector("#formItemSender"))
                .Selected;
            var afterDeliveryAddressCheckboxSelected = _webDriver.FindElement(By.CssSelector("#formItemReceiver"))
                .Selected;

            Assert.IsTrue(afterShippmentAddressCheckboxSelected != beforeShippmentAddressCheckboxSelected
                || afterDeliveryAddressCheckboxSelected != beforeDeliveryAddressCheckboxSelected);
        }   

        [Test]
        [Description("Dodanie opinii o stronie")]
        public void AddOpinionAboutSiteTest()
        {
            LogInUserAccount();

            //kliknąć element "Opinia" po prawej stronie ekranu
            FindAndClick("._hj-21t0-__MinimizedWidgetMiddle__label");

            //czekać na wyświetlenie okienka do wyrażania opinii
            WaitForElementDisplayed("._hj-3PSb0__ExpandedWidget__innerContainer");

            //kliknąć wybrany element - Dobrze
            FindAndClick("._hj-2oMDA__EmotionOption__like");

            //uzupełnić input "Podziel się z nami swoją opinią"
            FindAndSendKeys("._hj-2bByr__EmotionComment__textArea._hj-FAirH__styles__inputField", "OK");

            //kliknij przycisk "Wyślij"
            FindAndClick("._hj-qnMJa__styles__primaryButton._hj-2wZPy__PrimaryButtonBase__primaryButton");

            //sprwdzić czy wyświetlono potwierdzenie wysłania opinii
            var confirmWindow = _webDriver.FindElement(By.CssSelector("._hj-2ObUs__MinimizedWidgetMessage__messageText"));
            Assert.IsTrue(confirmWindow.Displayed);
        }
    }
}
