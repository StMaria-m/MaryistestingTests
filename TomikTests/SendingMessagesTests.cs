using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.IO;
using TomikTests.Enums;
using TomikTests.Helpers;
using TomikTests.Models;
using TomikTests.Wrappers;

namespace TomikTests
{
    [Parallelizable(ParallelScope.Children)]
    public class SendingMessagesTests
    {
        private string _userName = "m.test";
        private string _messageBodyText = "To jest wiadomość testowa";
        private string _confirmSendMessage = ".//*[contains(text(), 'Wiadomość została wysłana!')]";

        private string _newMessageTooltipSelector = "#ui-tooltip-writePrivateMessage";
        private string _searchFormSelector = "#searchFormAccounts";

        protected AppSettingsModel _appSettings;

        [OneTimeSetUp]
        public void PrepareData()
        {
            var appSettingsString = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}//appsettings.json");
            _appSettings = JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsString);
        }

        [Test]
        [Category("Sending messages tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Wyszukiwanie  chomików")]
        public void CorrectSendMessagesCheckOutboxTest()
        {
            var browserWrapper = OpenBrowser();
            var webDriver = browserWrapper.GetWebDriver();

            string messageSubject = Guid.NewGuid().ToString();

            SearchUserByFullName(_userName, webDriver);

            SendMessage(messageSubject, _messageBodyText, browserWrapper);

            //sprawdzić, czy wiadomość została wysłana - w sekcji wiadomości wysłane
            var userMessages = webDriver.FindElement(By.CssSelector("#topbarMessage"));
            userMessages.Click();

            var userAllSendMessagesTab = webDriver.FindElement(By.CssSelector("#tabMenu a[href$=outbox]"));
            userAllSendMessagesTab.Click();

            browserWrapper.WaitForAction("#outbox");

            Assert.DoesNotThrow(() => webDriver.FindElement(By.XPath($".//*[contains(text(), '{messageSubject}')]")));
           
            webDriver.Quit();
        }

        [Test]
        [Category("Search files tests next")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Wyszukiwanie  chomików")]
        public void CorrectSendMessagesCheckCofirmationDisplayTest()
        {
            string messageSubject = Guid.NewGuid().ToString();

            var browserWrapper = OpenBrowser();
            var webDriver = browserWrapper.GetWebDriver();

            SearchUserByFullName(_userName, webDriver);

            SendMessage(messageSubject, _messageBodyText, browserWrapper);

            Assert.DoesNotThrow(() => webDriver.FindElement(By.XPath(_confirmSendMessage)));

            webDriver.Quit();           
        }       

        [Test]
        [Category("Sending messages tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Usuwanie wiadomości ze skrzynki nadawczej")]
        public void CorrectSendMessagesCheckOutbox()
        {
            string messageSubject = Guid.NewGuid().ToString();

            var browserWrapper = OpenBrowser();
            var webDriver = browserWrapper.GetWebDriver();

            SearchUserByFullName(_userName, webDriver);

            SendMessage(messageSubject, _messageBodyText, browserWrapper);

            //sprawdzić, czy wiadomość została wysłana - w sekcji wiadomości wysłane
            var userMessages = webDriver.FindElement(By.CssSelector("#topbarMessage"));
            userMessages.Click();

            var userAllSendMessagesTab = webDriver.FindElement(By.CssSelector("#tabMenu a[href$=outbox]"));
            userAllSendMessagesTab.Click();

            browserWrapper.WaitForAction("#outbox tbody > tr");
          
            //znajdź wiadomośc testową w wysłanych wiadomościach i kliknij
            var sentTestMessage = webDriver.FindElement(By.XPath($"//a[contains(text(), '{messageSubject}')]"));
            sentTestMessage.Click();

            browserWrapper.WaitForAction("#SelectedMessageContainer .deleteMessage");

            var deleteMessage = webDriver.FindElement(By.CssSelector("#SelectedMessageContainer > .readMessageHeader > .deleteMessage"));
            deleteMessage.Click();

            browserWrapper.WaitForAction("#outbox tbody > tr");

            Assert.DoesNotThrow(() => webDriver.FindElement(By.XPath($".//*[contains(text(), '{messageSubject}')]")));

            webDriver.Quit();           
        }

        private void SearchUserByFullName(string userName, IWebDriver webDriver)
        {
            //zaznaczyć radio button wyszukiwanie chomików i wpisać pełną unikatową nazwę chomika
            var inputRadio = webDriver.FindElement(By.CssSelector("#searchOptionAccount"));
            inputRadio.Click();

            //uzupełnić pole "Nazwa"
            var searchingInput = webDriver.FindElement(By.CssSelector($"{_searchFormSelector} #Query"));
            searchingInput.SendKeys(userName);

            //kliknąć w przycisk "Szukaj"           
            var searchingAvatar = webDriver.FindElement(By.CssSelector($"{_searchFormSelector} .quickSearchButton"));
            searchingAvatar.Click();
        }

        private void SendMessage(string messageSubject, string messageBodyText, BrowserWrapper browserWrapper)
        {
            var webDriver = browserWrapper.GetWebDriver();

            //kliknąć "Wyslij wiadomość do chomika"
            var sendMessageSelector = webDriver.FindElement(By.CssSelector("#accInfoSendMsg"));
            sendMessageSelector.Click();

            browserWrapper.WaitForAction(_newMessageTooltipSelector);

            //wpisać temat wiadomości
            var messageSubjectInput = webDriver.FindElement(By.CssSelector($"{_newMessageTooltipSelector} #Subject"));
            messageSubjectInput.SendKeys(messageSubject);

            //wpisać treść wiadomości
            var messageBodyInput = webDriver.FindElement(By.CssSelector($"{_newMessageTooltipSelector} #Body"));
            messageBodyInput.SendKeys(messageBodyText);

            //kliknać "Wyślij"
            var sendMessage = webDriver.FindElement(By.CssSelector("#sendPMBtn"));
            sendMessage.Click();

            browserWrapper.WaitForAction(_confirmSendMessage, SearchByTypeEnums.XPath);
        }

        private BrowserWrapper OpenBrowser()
        {
            BrowserWrapper browserWrapper = new BrowserWrapper(_appSettings);
            browserWrapper.CreateWebDriver();
            browserWrapper.AddCookie();

            ProcessStepHeplers.LogInSteps(browserWrapper, _appSettings);
            return browserWrapper;
        }       
    }
}
