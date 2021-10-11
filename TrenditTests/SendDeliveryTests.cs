using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TrenditTests
{
    [Author("Maria", "http://maryistesting.com")]
    [Category("Update user details")]
    public class SendDeliveryTests : BaseTest
    {
        [Test]
        [Description("Uzupełnienie formularza do wysyłania przesyłek - tylko pola wymagane")]
        public void CorrectFillShippinigForm_RequiredFieldsOnly_Test()
        {
            LogInUserAccount();

            Thread.Sleep(1000);

            //uzupełnić pola wymagane do przygotowania listu przewozowego
            //uzupełnić wymiary przesyłki: długość, szerokość, wysokość
            FindAndSendKeys("#formItemShipment0Dimension1", "10");

            FindAndSendKeys("#formItemShipment0Dimension2", "10");

            FindAndSendKeys("#formItemShipment0Dimension3", "10");

            FindAndSendKeys("#formItemShipment0Weight", "2");

            //uzupełnić pole "Zawartość przesyłki"
            FindAndSendKeys("#formItemContent", "przesyłka");

            //uzupełnić pole "Telefon" w sekcji "Adres nadania"
            FindAndSendKeys("#formItemAddressSenderPhone", "500111222");

            //uzupełnić sekcję "Adres dostawy"
            //uzupełnić pole "Nawa firmy/Imię i nazwisko"
            FindAndSendKeys("#formItemAddressReceiverName", "Jan Nowak");

            //uzupełnić pole "Ulica"
            FindAndSendKeys("#formItemAddressReceiverStreet", "Warszawska");

            //uzupełnić pole "Kod pocztowy"
            FindAndSendKeys("#formItemAddressReceiverPostalCode", "00-001");

            _webDriver.FindElement(By.TagName("body")).Click();

            //uzupełnić pole "Miejscowość"
            FindAndSendKeys("#formItemAddressReceiverCity", "Warszawa");

            _webDriver.FindElement(By.TagName("body")).Click();

            //uzupełnić pole "Osoba kontaktowa"
            FindAndSendKeys("#formItemAddressReceiverContactPerson", "Janina Kowalska");

            //uzupełnić pole "Telefon"
            FindAndSendKeys("#formItemAddressReceiverPhone", "600111222");

            Thread.Sleep(3000);

            //kliknąć "Zapłać"
            FindAndClick("#formSubmit");

            //sprawdzenie przekierowania na stronę do płatności
            var url = _webDriver.Url;
            StringAssert.Contains("go.przelewy24.pl", url);
        }

        [Test]
        [Description("Uzupełnienie formularza do wysyłania przesyłek - wybór dostępnych opcji")]
        public void CorrectFillVariousFields_SendDelivery_Test()
        {
            LogInUserAccount();

            Thread.Sleep(1000);

            //zwiększyć liczbę paczek do 2
            FindAndClick("#formItemShipmentPlus");

            Thread.Sleep(1000);

            //zaznaczyć checkbox "Różne parametry"
            FindAndClick("#formItemShipmentDiffDimensions + .switchSlider");

            //uzupełnić pola wymagane do przygotowania listu przewozowego
            //uzupełnić wymiary przesyłki: długość, szerokość, wysokość, wagę pierwszej paczki
            FindAndSendKeys("#formItemShipment0Dimension1", "10");

            FindAndSendKeys("#formItemShipment0Dimension2", "10");

            FindAndSendKeys("#formItemShipment0Dimension3", "10");

            FindAndSendKeys("#formItemShipment0Weight", "1,5");

            //uzupełnić wymiary przesyłki: długość, szerokość, wysokość drugiej paczki
            FindAndSendKeys("#formItemShipment1Dimension1", "15");

            FindAndSendKeys("#formItemShipment1Dimension2", "15");

            FindAndSendKeys("#formItemShipment1Dimension3", "15");

            FindAndSendKeys("#formItemShipment1Weight", "1,5");

            //zaznaczyc checkbox "Przesyłka niestandardowa"
            FindAndClick("[for=formItemShipment1NotStandard]");

            //uzupełnić sekcję "Dodatkowe informacje i usługi"
            //uzupełnic pole "Zawartość przesyłki"
            FindAndSendKeys("#formItemContent", "przesyłka1");

            //uzupełnić pole "Dodatkowy komentarz"
            FindAndSendKeys("#formItemComment", "komentarz1");

            //uzupełnić pole "Deklarowana wartość"
            FindAndSendKeys("#formItemShipmentValue", "100");

            //uzupełnić pole "kwota pobrania" - musi być >= deklarowanej wartości
            FindAndSendKeys("#formItemCodAmount", "100");

            _webDriver.FindElement(By.TagName("body")).Click();

            //sprawdzić, czy nr konta został uzupełniony, jesli nie, to wpisać wartość jeżeli tak to idź dalej
            var accountNumberInput = _webDriver.FindElement(By.CssSelector("#formItemCodBankAccount"));
            if (String.IsNullOrEmpty(accountNumberInput.GetAttribute("value")))
            {
                accountNumberInput.SendKeys("999900008888777766665555");
            }

            //zaznaczyć checkbox "Powiadomienie SMS"
            FindAndClick("[for=formItemOption31]");

            //uzupełnić pole "Telefon" w sekcji "Adres nadania"
            FindAndSendKeys("#formItemAddressSenderPhone", "500111222");

            //uzupełnić sekcję "Adres dostawy"
            //uzupełnić pole "Nawa firmy/Imię i nazwisko"
            FindAndSendKeys("#formItemAddressReceiverName", "Jan Nowak");

            //uzupełnić pole "Ulica"
            FindAndSendKeys("#formItemAddressReceiverStreet", "Warszawska");

            //uzupełnić pole "Kod pocztowy"
            FindAndSendKeys("#formItemAddressReceiverPostalCode", "00-001");
            _webDriver.FindElement(By.TagName("body")).Click();

            //uzupełnić pole "Miejscowość"
            FindAndSendKeys("#formItemAddressReceiverCity", "Warszawa");
            _webDriver.FindElement(By.TagName("body")).Click();

            //zaznaczyć checkbox "Typ adresu: Prywatny"
            FindAndClick("[for=orderAddressReceiverTypeResidential]");

            //uzupełnić pole "Osoba kontaktowa"
            FindAndSendKeys("#formItemAddressReceiverContactPerson", "Janina Kowalska");

            //uzupełnić pole "Telefon"
            FindAndSendKeys("#formItemAddressReceiverPhone", "600111222");

            Thread.Sleep(3000);

            //kliknąć "Zapłać"
            FindAndClick("#formSubmit");

            //sprawdzenie przekierowania na stronę do płatności
            var url = _webDriver.Url;
            StringAssert.Contains("go.przelewy24.pl", url);
        }

        [Test]
        [Description("Uzupełnienie formularza przesyłek - sekcja Rodzaj przesyłki - Paleta/Półpaleta")]
        public void CorrectFillShippinigForm_shipmentTypePallet_Test()
        {
            LogInUserAccount();

            Thread.Sleep(1000);

            //w sekcji Rodzaj przesyłki wybrać Paleta/Półpaleta
            FindAndClick("[for=formItemShipmentTypePaletaExpand]");

            //sprawdzić, czy pojawia się przycisk "Kliknij i otrzymaj indywidualną wycenę"
            var messageBox = _webDriver.FindElements(By.CssSelector("#formMessageShipmentPaleta")).FirstOrDefault();
            Assert.IsTrue(messageBox.Displayed);
        }

        [Test]
        [Description("Uzupełnienie formularza przesyłek - sekcja Rodzaj przesyłki - Paleta/Półpaleta - materiały niebezpieczne ADR")]
        public void CorrectFillShippinigForm_shipmentTypePallet_adrShippment_Test()
        {
            LogInUserAccount();

            Thread.Sleep(1000);

            //w sekcji Rodzaj przesyłki wybrać Paleta/Półpaleta
            FindAndClick("[for=formItemShipmentTypePaletaExpand]");

            //zaznaczyć checkbox "Materiały niebezpieczne ADR"
            FindAndClick("[for=formItemOption23]");

            //sprawdzić, czy pojawia się przycisk "Kliknij i otrzymaj indywidualną wycenę"
            var messageBox = _webDriver.FindElements(By.CssSelector("#formMessageShipmentPaleta")).FirstOrDefault();
            Assert.IsTrue(messageBox.Displayed);
        }

        [Test]
        [Description("Uzupełnienie formularza przesyłek - sekcja Rodzaj przesyłki - Koperta- czy istnieje checkbox Opony")]
        public void CorrectFillShippinigForm_shipmentTypeEnvelope_Test()
        {
            LogInUserAccount();

            Thread.Sleep(1000);

            //w sekcji rodzaj przesyłki wybrać "Koperta"
            FindAndClick("[for=formItemShipmentTypeList]");

            //sprawdzić, czy znika checkbox "Opony"
            var checkboxTyre = _webDriver.FindElements(By.CssSelector("[for=formItemOption82]")).FirstOrDefault();
            Assert.False(checkboxTyre.Displayed);
        }        
    }
}
