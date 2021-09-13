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
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(" ");

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(" ");

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            Thread.Sleep(1000);

            //4. Kliknąć przycisk "Opcje"
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();

            RemoveAcceptContainer();

            //5. Kliknąć zakłądkę "O Tobie"
            var aboutButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=about]"));
            aboutButton.Click();

            //6. Uzupełnić pole "Imię"            
            var name = _webDriver.FindElement(By.CssSelector("#Name"));
            string exampleName =  $"{name.Text}a";
            name.Clear();
            name.SendKeys(exampleName);

            //7. Wybrać płeć
            var sexField = _webDriver.FindElement(By.CssSelector("#female"));
            sexField.Click();

            //8. Kliknąć "Zapisz zmiany"
            var saveButton = _webDriver.FindElement(By.CssSelector("#ownerInfoForm .greenButtonCSS"));
            saveButton.Click();

            Thread.Sleep(1000);

            RemoveAcceptContainer();
                        
            try
            {
                //9. Sprawdzenie płci - jeśli pojawi się symbol wybranej powyżej płci
                _webDriver.FindElement(By.CssSelector("#chomikInfo [alt=Kobieta]"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Gender not found");
                return;
            }

            try
            {
                //10. Sprawdzenie imienia - jeśli wyświetli się nazwa użytkowinka wpisana powyżej
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
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(" ");

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(" ");

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            Thread.Sleep(1000);

            //4. Kliknąć przycisk "Opcje"
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();

            RemoveAcceptContainer();

            //5. Kliknąć zakładkę "Ustawienia"
            var settingsButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=settings]"));
            settingsButton.Click();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector("#ChangeAdultAllow[checked=checked]"));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }            

            //7. Kliknąć przycisk 'jestem pełnoletni i chcę domyślnie widzieć także treści przeznaczone tylko dla takich osób'
            var checkbox = _webDriver.FindElement(By.CssSelector("#ChangeAdultAllow"));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector("#ChangeAdultAllow[checked=checked]"));
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
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(" ");

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(" ");

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            Thread.Sleep(1000);

            //4. Kliknąć przycisk "Opcje"
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();

            RemoveAcceptContainer();

            //5. Kliknąć zakładkę "Ustawienia"
            var settingsButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=settings]"));
            settingsButton.Click();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector("#DownloadManagerAllow[checked=checked]"));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }            

            //7. Kliknąć przycisk 'pokazuj dodatkowe okno z linkiem przyjaznym akceleratorom pobierania dla plików'
            var checkbox = _webDriver.FindElement(By.CssSelector("#DownloadManagerAllow"));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector("#DownloadManagerAllow[checked=checked]"));
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
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(" ");

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(" ");

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            Thread.Sleep(1000);

            //4. Kliknąć przycisk "Opcje"
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();

            RemoveAcceptContainer();

            //5. Kliknąć zakładkę "Ustawienia"
            var settingsButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=settings]"));
            settingsButton.Click();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector("#ChomikBoxDownloadAllow[checked=checked]"));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }


            //7. Kliknąć przycisk 'zawsze pobieraj pliki z serwisu przy pomocy ChomikBox'
            var checkbox = _webDriver.FindElement(By.CssSelector("#ChomikBoxDownloadAllow"));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector("#ChomikBoxDownloadAllow[checked=checked]"));
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
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(" ");

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(" ");

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            Thread.Sleep(1000);

            //4. Kliknąć przycisk "Opcje"
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();

            RemoveAcceptContainer();

            //5. Kliknąć zakładkę "Ustawienia"
            var settingsButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=settings]"));
            settingsButton.Click();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector("#ChangeAllowCopyFiles"));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }


            //7. Kliknąć przycisk 'pozwól na chomikowanie plików z mojego Chomika:'
            var checkbox = _webDriver.FindElement(By.CssSelector("#ChangeAllowCopyFiles"));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector("#ChangeAllowCopyFiles[checked=checked]"));
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
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(" ");

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(" ");

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            Thread.Sleep(1000);

            //4. Kliknąć przycisk "Opcje"
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();

            RemoveAcceptContainer();

            //5. Kliknąć zakładkę "Ustawienia"
            var settingsButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=settings]"));
            settingsButton.Click();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector("#ChangeChatEnabled[checked=checked]"));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }


            //7. Kliknąć przycisk 'pokaż na stronie Chomika ShoutBox Chomikowe Rozmowy'
            var checkbox = _webDriver.FindElement(By.CssSelector("#ChangeChatEnabled"));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector("#ChangeChatEnabled[checked=checked]"));
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
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(" ");

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(" ");

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            Thread.Sleep(1000);

            //4. Kliknąć przycisk "Opcje"
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();

            RemoveAcceptContainer();

            //5. Kliknąć zakładkę "Ustawienia"
            var settingsButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=settings]"));
            settingsButton.Click();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector("#ChangeContentFilterEnabled[checked=checked]"));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }

            //7. Kliknąć przycisk 'nie pozwól na umieszczanie w moim chomiku wpisów zawierających obraźliwe słowa'
            var checkbox = _webDriver.FindElement(By.CssSelector("#ChangeContentFilterEnabled"));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector("#ChangeContentFilterEnabled[checked=checked]"));
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
            //1. Uzupełnić pole "Chomik"
            var login = _webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(" ");

            //2. Uzupełnić pole "Hasło"
            var password = _webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(" ");

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = _webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            Thread.Sleep(1000);

            //4. Kliknąć przycisk "Opcje"
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();

            RemoveAcceptContainer();

            //5. Kliknąć zakładkę "Ustawienia"
            var settingsButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=settings]"));
            settingsButton.Click();

            try
            {
                //6. Jeżeli checkbox jest już zaznaczony to odznacz i przeładuj stronę
                var checkedChecbox = _webDriver.FindElement(By.CssSelector("#SimpleUploadEnabled[checked=checked]"));
                checkedChecbox.Click();
                _webDriver.Navigate().Refresh();
                RemoveAcceptContainer();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nierób checkbox nie był zaznaczony
            }

            //7. Kliknąć przycisk 'włącz uproszczony system dodawania plików'
            var checkbox = _webDriver.FindElement(By.CssSelector("#SimpleUploadEnabled"));
            checkbox.Click();

            _webDriver.Navigate().Refresh();

            try
            {
                //8. Sprawdzenie zaznaczenia zgody
                _webDriver.FindElement(By.CssSelector("#SimpleUploadEnabled[checked=checked]"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Simple Upload is off");
                return;
            }

            Assert.Pass();
        }

    }
}