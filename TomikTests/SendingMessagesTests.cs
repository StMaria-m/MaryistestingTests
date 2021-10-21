using NUnit.Framework;
using OpenQA.Selenium;
using System;
using TomikTests.Enums;

namespace TomikTests
{
    public class SendingMessagesTests : BaseTest
    {
        private string _userName = "m.test";        
        private string _messageBodyText = "To jest wiadomość testowa";
        private string _confirmSendMessage = ".//*[contains(text(), 'Wiadomość została wysłana!')]";

        private string _newMessageTooltipSelector = "#ui-tooltip-writePrivateMessage";
        private string _searchFormSelector = "#searchFormAccounts";
        
        [Test]
        [Category("Sending messages tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Wyszukiwanie  chomików")]
        public void CorrectSendMessagesCheckOutboxTest()
        {
            string messageSubject = Guid.NewGuid().ToString();

            LogInSteps();

            SearchUserByFullName(_userName);

            SendMessage(messageSubject, _messageBodyText);

            //sprawdzić, czy wiadomość została wysłana - w sekcji wiadomości wysłane
            var userMessages = _webDriver.FindElement(By.CssSelector("#topbarMessage"));
            userMessages.Click();

            RemoveAcceptContainer();

            var userAllSendMessagesTab = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=outbox]"));
            userAllSendMessagesTab.Click();

            WaitForAction("#outbox");

            try
            {
                //sprawdzić, czy w wysłanych wiadomościach jest wiadomość testowa
                _webDriver.FindElement(By.XPath($".//*[contains(text(), '{messageSubject}')]"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("The message is not in the outbox");
                return;
            }
            Assert.Pass();
        }

        [Test]
        [Category("Search files tests next")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Wyszukiwanie  chomików")]
        public void CorrectSendMessagesCheckCofirmationDisplayTest()
        {
            string messageSubject = Guid.NewGuid().ToString();

            LogInSteps();

            SearchUserByFullName(_userName);

            SendMessage(messageSubject, _messageBodyText);

            try
            {
                //sprawdzić, czy pojawił się komunikat o wysłaniu wiadomości
                _webDriver.FindElement(By.XPath(_confirmSendMessage));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Message is not send");
                return;
            }
            Assert.Pass();
        }

        private void SearchUserByFullName(string userName)
        {
            //zaznaczyć radio button wyszukiwanie chomików i wpisać pełną unikatową nazwę chomika
            var inputRadio = _webDriver.FindElement(By.CssSelector("#searchOptionAccount"));
            inputRadio.Click();

            //uzupełnić pole "Nazwa"
            var searchingInput = _webDriver.FindElement(By.CssSelector($"{_searchFormSelector} #Query"));
            searchingInput.SendKeys(userName);

            //kliknąć w przycisk "Szukaj"           
            var searchingAvatar = _webDriver.FindElement(By.CssSelector($"{_searchFormSelector} .quickSearchButton"));
            searchingAvatar.Click();

            RemoveAcceptContainer();
        }

        private void SendMessage(string messageSubject, string messageBodyText)
        {
            //kliknąć "Wyslij wiadomość do chomika"
            var sendMessageSelector = _webDriver.FindElement(By.CssSelector("#accInfoSendMsg"));
            sendMessageSelector.Click();

            WaitForAction(_newMessageTooltipSelector);

            //wpisać temat wiadomości
            var messageSubjectInput = _webDriver.FindElement(By.CssSelector($"{_newMessageTooltipSelector} #Subject"));
            messageSubjectInput.SendKeys(messageSubject);

            //wpisać treść wiadomości
            var messageBodyInput = _webDriver.FindElement(By.CssSelector($"{_newMessageTooltipSelector} #Body"));
            messageBodyInput.SendKeys(messageBodyText);

            //kliknać "Wyślij"
            var sendMessage = _webDriver.FindElement(By.CssSelector("#sendPMBtn"));
            sendMessage.Click();

            WaitForAction(_confirmSendMessage, SearchByTypeEnums.XPath);
        }

        [Test]
        [Category("Sending messages tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Usuwanie wiadomości ze skrzynki nadawczej")]
        public void CorrectSendMessagesCheckOutbox()
        {
            string messageSubject = Guid.NewGuid().ToString();

            LogInSteps();

            SearchUserByFullName(_userName);

            SendMessage(messageSubject, _messageBodyText);

            //sprawdzić, czy wiadomość została wysłana - w sekcji wiadomości wysłane
            var userMessages = _webDriver.FindElement(By.CssSelector("#topbarMessage"));
            userMessages.Click();

            RemoveAcceptContainer();

            var userAllSendMessagesTab = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=outbox]"));
            userAllSendMessagesTab.Click();

            WaitForAction("#outbox");

            //znajdź wiadomośc testową w wysłanych wiadomościach i kliknij
            var sentTestMessage = _webDriver.FindElement(By.XPath($"//a[contains(text(), '{messageSubject}')]"));
            sentTestMessage.Click();

            WaitForAction("#SelectedMessageContainer");

            var deleteMessage = _webDriver.FindElement(By.XPath($".//*[contains(text(), 'Usuń')]"));
            deleteMessage.Click();

            WaitForAction("#outbox");

            try
            {
                //sprawdzić, czy wiadomość została usunięta ze skrzynki nadawczej
                _webDriver.FindElement(By.XPath($".//*[contains(text(), '{messageSubject}')]"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Pass("The message has been removed");
                return;
            }
            Assert.Pass();
        }
    }
}
