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
