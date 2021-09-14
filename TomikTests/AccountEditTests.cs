using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace TomikTests
{
    public class AccountEditTests : BaseTest
    {
        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Edycja ustawień konta użytkownika - zakładka 'O Tobie' poprawny zapis danych i sprawdzenie ich wyświetlenia w profilu użytkownika")]
        public void Test3()
        {
            LogInSteps();

            ClickOptionStep();

            ClickAboutMeTabStep();

            //Uzupełnić pole "Imię"            
            var name = _webDriver.FindElement(By.CssSelector("#Name"));
            string exampleName =  $"{name.Text}a";
            name.Clear();
            name.SendKeys(exampleName);

            //Wybrać płeć
            var sexField = _webDriver.FindElement(By.CssSelector("#female"));
            sexField.Click();

            //Kliknąć "Zapisz zmiany"
            var saveButton = _webDriver.FindElement(By.CssSelector("#ownerInfoForm .greenButtonCSS"));
            saveButton.Click();

            Thread.Sleep(1000);

            RemoveAcceptContainer();
                        
            try
            {
                //Sprawdzenie płci - jeśli pojawi się symbol wybranej powyżej płci
                _webDriver.FindElement(By.CssSelector("#chomikInfo [alt=Kobieta]"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Gender not found");
                return;
            }

            try
            {
                //Sprawdzenie imienia - jeśli wyświetli się nazwa użytkowinka wpisana powyżej
                var displayName = _webDriver.FindElement(By.CssSelector("#chomikInfo span"));
                StringAssert.Contains(exampleName, displayName.Text);
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Name not found");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Ustawienia' poprawny zapis zgody - 'jestem pełnoletni i chcę domyślnie widzieć także treści przeznaczone tylko dla takich osób'")]
        public void Test4()
        {
            string checkboxSelector = "#ChangeAdultAllow";
            string checkboxCheckedSelector = $"{checkboxSelector}[checked=checked]";

            LogInSteps();

            ClickOptionStep();
            
            ClickSettingsTabStep();

            try
            {
                //Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }            

            //Kliknąć przycisk 'jestem pełnoletni i chcę domyślnie widzieć także treści przeznaczone tylko dla takich osób'
            var checkbox = _webDriver.FindElement(By.CssSelector(checkboxSelector));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("User is underage");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Ustawienia' poprawny zapis zgody - 'pokazuj dodatkowe okno z linkiem przyjaznym akceleratorom pobierania dla plików'")]
        public void Test55()
        {
            string checkboxSelector = "#DownloadManagerAllow";
            string checkboxCheckedSelector = $"{checkboxSelector}[checked=checked]";

            LogInSteps();

            ClickOptionStep();

            ClickSettingsTabStep();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }            

            //7. Kliknąć przycisk 'pokazuj dodatkowe okno z linkiem przyjaznym akceleratorom pobierania dla plików'
            var checkbox = _webDriver.FindElement(By.CssSelector(checkboxSelector));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Additional window is not allowed");
                return;
            }

            Assert.Pass();           
        }

        
        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Ustawienia' poprawny zapis zgody - 'zawsze pobieraj pliki z serwisu przy pomocy ChomikBox'")]
        public void Test6()
        {
            string checkboxSelector = "#ChomikBoxDownloadAllow";
            string checkboxCheckedSelector = $"{checkboxSelector}[checked=checked]";

            LogInSteps();

            ClickOptionStep();

            ClickSettingsTabStep();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }


            //7. Kliknąć przycisk 'zawsze pobieraj pliki z serwisu przy pomocy ChomikBox'
            var checkbox = _webDriver.FindElement(By.CssSelector(checkboxSelector));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Download is not allowed");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Ustawienia' poprawny zapis zgody - 'pozwól na chomikowanie plików z mojego Chomika:'")]
        public void Test7()
        {
            string checkboxSelector = "#ChangeAllowCopyFiles";
            string checkboxCheckedSelector = $"{checkboxSelector}[checked=checked]";

            LogInSteps();

            ClickOptionStep();

            ClickSettingsTabStep();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }


            //7. Kliknąć przycisk 'pozwól na chomikowanie plików z mojego Chomika:'
            var checkbox = _webDriver.FindElement(By.CssSelector(checkboxSelector));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Copy files is not allowed");
                return;
            }

            Assert.Pass();
        }

        
        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Ustawienia' poprawny zapis zgody - 'pokaż na stronie Chomika ShoutBox Chomikowe Rozmowy'")]
        public void Test8()
        {
            string checkboxSelector = "#ChangeChatEnabled";
            string checkboxCheckedSelector = $"{checkboxSelector}[checked=checked]";

            LogInSteps();

            ClickOptionStep();

            ClickSettingsTabStep();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }


            //7. Kliknąć przycisk 'pokaż na stronie Chomika ShoutBox Chomikowe Rozmowy'
            var checkbox = _webDriver.FindElement(By.CssSelector(checkboxSelector));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Chat is not allowed");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Ustawienia' poprawny zapis zgody - 'nie pozwól na umieszczanie w moim chomiku wpisów zawierających obraźliwe słowa'")]
        public void Test9()
        {
            string checkboxSelector = "#ChangeContentFilterEnabled";
            string checkboxCheckedSelector = $"{checkboxSelector}[checked=checked]";

            LogInSteps();

            ClickOptionStep();

            ClickSettingsTabStep();
            
            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }

            //7. Kliknąć przycisk 'nie pozwól na umieszczanie w moim chomiku wpisów zawierających obraźliwe słowa'
            var checkbox = _webDriver.FindElement(By.CssSelector(checkboxSelector));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Content filter is off");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Ustawienia' poprawny zapis zgody - 'włącz uproszczony system dodawania plików'")]
        public void Test10()
        {
            string checkboxSelector = "#SimpleUploadEnabled";
            string checkboxCheckedSelector = $"{checkboxSelector}[checked=checked]";

            LogInSteps();

            ClickOptionStep();

            ClickSettingsTabStep();

            try
            {
                //Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }

            //Kliknąć przycisk 'włącz uproszczony system dodawania plików'
            var checkbox = _webDriver.FindElement(By.CssSelector(checkboxSelector));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector(checkboxCheckedSelector));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Simple Upload is off");
                return;
            }

            Assert.Pass();
        }

        private void ClickOptionStep()
        {
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();

            RemoveAcceptContainer();
        }

        private void ClickAboutMeTabStep()
        {
            var aboutButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=about]"));
            aboutButton.Click();
        }

        private void ClickSettingsTabStep()
        {
            var settingsButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=settings]"));
            settingsButton.Click();
        }
    }
}