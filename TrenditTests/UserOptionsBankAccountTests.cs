using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using System.Linq;
using System;
using System.Collections.Generic;

namespace TrenditTests
{
    [Author("Maria", "http://maryistesting.com")]
    [Category("Update user bank account")]
    public class UserOptionsBankAccountTests : BaseTest
    {
        private string _popUpSelector = "#lightBoxWhiteBox";
        private string _submitButtonSelector = "input[value=Zapisz]";

        private string newBankAccountNumberSelectorButton = "a[href='javascript:settingsCompanyBankAccountAdd();']";
        private string _accountNameInput = "przyjaznaNazwa";
        private string accountNameSelector = "[name='bankaccount[alias]']";
        private string accountNumberSelector = "[name='bankaccount[number]']";

        [Test]
        [Description("Dodanie konta do listy kont w sekcji 'Pobrania'")]
        public void AddAccountNumberTest()
        {
            string newAccountNumber = new Random().Next().ToString();

            LogInUserAccount();
            _webDriver.Navigate().GoToUrl(GetPageUrl());

            newBankAccountNumberSelectorButton = "a[href='javascript:settingsCompanyBankAccountAdd();']";

            AddNewBankAccount(newAccountNumber, _accountNameInput);

            var accountNumberInputList = _webDriver.FindElements(By.CssSelector("input[name^=bankaccount_list]"));
            var accountNumberInput = accountNumberInputList.FirstOrDefault(x => x.GetAttribute("value") == newAccountNumber);

            Assert.IsNotNull(accountNumberInput);
        }

        [Test]
        [Description("Edycja numeru konta z listy kont w sekcji'Pobrania'")]
        public void UpdateAccountNumberTest()
        {
            string currentAccountNumber = "999900008888777766665555";
            string newAccountNumber = "999900008888777766668888";

            LogInUserAccount();
            _webDriver.Navigate().GoToUrl(GetPageUrl());

            AddNewBankAccountIfNull(currentAccountNumber, _accountNameInput);

            DeleteBankAccountIfExists(newAccountNumber);

            //kliknąć przycisk Edytuj
            FindAndClick("a[href='javascript:settingsCompanyBankAccountEdit();']");

            WaitForElementDisplayed(".popupKsiazka");

            //kliknąć pole z numerem konta, który chcemy edytować, wyczyścić input i wpisać nowy nr konta
            var trtList = _webDriver.FindElements(By.CssSelector("#lightBoxWhiteBox tr"));
            foreach (var tr in trtList)
            {
                var input = GetAccountNumberInput(currentAccountNumber, tr);
                if (input != null)
                {
                    input.Clear();
                    input.SendKeys(newAccountNumber);

                    tr.FindElement(By.CssSelector("[name='bankaccount_list[default]']")).Click();
                    break;
                }
            }

            FindAndClick(_submitButtonSelector);

            Thread.Sleep(1000);

            //kliknąć przycisk Edytuj
            FindAndClick("a[href='javascript:settingsCompanyBankAccountEdit();']");

            WaitForElementDisplayed(".popupKsiazka");

            Assert.IsNotNull(GetAccountNumberInput(newAccountNumber));
        }

        private string GetPageUrl() => $"{_appSettings.UserPanelUrl}/ustawienia.php";     

        private void AddNewBankAccount(string accountNumber, string accountNameInput)
        {
            //kliknąć przycisk Dodaj w sekcji "Pobrania"
            _webDriver.FindElement(By.CssSelector(newBankAccountNumberSelectorButton)).Click();

            WaitForElementDisplayed(_popUpSelector);

            //wpisać nazwę nowego konta bankowego
            _webDriver.FindElement(By.CssSelector(accountNameSelector)).SendKeys(accountNameInput);

            //wpisać nr rachunku
            _webDriver.FindElement(By.CssSelector(accountNumberSelector)).SendKeys(accountNumber);

            //kliknąć przycisk "Dodaj"
            _webDriver.FindElement(By.CssSelector("input[value=Dodaj]")).Click();

            WaitForElementDisappeared(_popUpSelector);
        }

        private void AddNewBankAccountIfNull(string accountNumber, string accountNameInput)
        {
            FindAndClick("a[href='javascript:settingsCompanyBankAccountEdit();']");

            WaitForElementDisplayed(".popupKsiazka");

            var accountNumberInput = GetAccountNumberInput(accountNumber);

            if (accountNumberInput == null)
            {
                FindAndClick("a[href='javascript:closeLightBox()']");
                AddNewBankAccount(accountNumber, accountNameInput);
            }
            else
            {
                FindAndClick("a[href='javascript:closeLightBox()']");
            }
        }

        private void DeleteBankAccount(string accountNumber)
        {
            var trtList = _webDriver.FindElements(By.CssSelector("#lightBoxWhiteBox tr"));
            foreach (var tr in trtList)
            {
                var input = GetAccountNumberInput(accountNumber, tr);

                if (input != null)
                {
                    tr.FindElement(By.CssSelector("[href^='./ustawienia.php?bankaccount_delete']")).Click();
                }
            }
        }

        private void DeleteBankAccountIfExists(string accountNumber)
        {
            FindAndClick("a[href='javascript:settingsCompanyBankAccountEdit();']");

            WaitForElementDisplayed(".popupKsiazka");

            var accountNumberInput = GetAccountNumberInput(accountNumber);
            if (accountNumberInput != null)
            {
                DeleteBankAccount(accountNumber);
            }
            else
            {
                FindAndClick("a[href='javascript:closeLightBox()']");
            }
        }

        private IWebElement GetAccountNumberInput(string accountNumber, IWebElement rootElement = null)
        {
            IReadOnlyCollection<IWebElement> accountNumberInputList;
            if (rootElement == null)
            {
                accountNumberInputList = _webDriver.FindElements(By.CssSelector("input[name^=bankaccount_list]"));
            }
            else
            {
                accountNumberInputList = rootElement.FindElements(By.CssSelector("input[name^=bankaccount_list]"));
            }
            return accountNumberInputList.FirstOrDefault(x => x.GetAttribute("value") == accountNumber);
        }       
    }
}

