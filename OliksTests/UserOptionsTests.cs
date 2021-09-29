using NUnit.Framework;
using OpenQA.Selenium;
using OliksTests.Enums;

namespace OliksTests
{
    public class UserOptionsTests: BaseTest
    {
        private string _successXPathSelector = "//*[contains(text(), 'Zmiany zostały zapisane')]";
        private string _sutosuggestFirstOptionSelector = "#autosuggest-geo-ul > li";

        [Test]
        [Category("User log out tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Ustawienia konta - zmiana danych kontaktowych")]
        public void AccountSettings_editContactDetailsTests()
        {
            LogInSteps("/mojolx/ustawienia/");

            //kliknąć "Zamień dane kontaktowe"
            _webDriver.FindElement(By.CssSelector("#se_changeContactDetails"))
                .Click();

            var townPostcodeInput = _webDriver.FindElement(By.CssSelector("#geoCity"));
            townPostcodeInput.Clear();
            townPostcodeInput.SendKeys("00-001");

            WaitForAction(_sutosuggestFirstOptionSelector);

            _webDriver.FindElement(By.CssSelector(_sutosuggestFirstOptionSelector))
                .Click();            

            var userName = _webDriver.FindElement(By.CssSelector("#defaultPerson"));
            userName.Clear();
            userName.SendKeys("Maria");
            
            _webDriver.FindElement(By.CssSelector("#submitDefault"))
                .Click();           

            WaitForAction(_successXPathSelector, SearchByTypeEnums.XPath);

            Assert.DoesNotThrow(() => _webDriver.FindElement(By.XPath(_successXPathSelector)));
        }
    }
}
