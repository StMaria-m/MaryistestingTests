using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace TomikTests
{
    [Description("Edycja ustawień konta użytkownika")]
    public class AccountEditTests : BaseTest
    {
        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'O Tobie' poprawny zapis danych i sprawdzenie ich wyświetlenia w profilu użytkownika")]
        public void Test3()
        {
            LogInSteps();

            ClickOptionStep();

            ClickAboutMeTabStep();

            //Uzupełnić pole "Imię"            
            var name = _webDriver.FindElement(By.CssSelector("#Name"));
            string exampleName = $"{name.Text}a";
            name.Clear();
            name.SendKeys(exampleName);

            //Wybrać płeć
            var sexField = _webDriver.FindElement(By.CssSelector("#female"));
            sexField.Click();

            //Kliknąć "Zapisz zmiany"
            var saveButton = _webDriver.FindElement(By.CssSelector("#ownerInfoForm .greenButtonCSS"));
            saveButton.Click();

            Thread.Sleep(1000);

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
        [Description("Zakładka 'O Tobie' niepoprawnie uzupełniony e-mail")]
        public void Test33()
        {
            LogInSteps();

            ClickOptionStep();

            ClickAboutMeTabStep();

            //Kliknąć link "zmień adres"
            var changeEmail = _webDriver.FindElement(By.CssSelector(".btnChangeEmail"));
            changeEmail.Click();

            //Uzupełnić pole "e-mail"            
            var newEmail = _webDriver.FindElement(By.CssSelector("#Email"));
            newEmail.SendKeys("aaa");

            _webDriver.FindElement(By.TagName("body")).Click();

            //Sprawdzenie, czy pojawi się komunikat "błędny adres e-mail"
            var emailError = _webDriver.FindElement(By.CssSelector("#divEmail span[for=Email]"));
            StringAssert.Contains("błędny adres e-mail", emailError.Text);
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
             

        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Hasła dostępu' poprawne ustawienie hasłą dostępu innych użytkowników podczas przeglądania plików użytkownika")]
        public void Test11()
        {
            string password = "abc123";

            LogInSteps();

            ClickOptionStep();

            ClickAccessTabStep();

            try
            {
                //Jeżeli hasło jest już ustawione to przywracamy do opcji "Dla wszystkich"
                var optionByPasswordSelected = _webDriver.FindElement(By.CssSelector("#cbAccessRead .OptionByPassword[selected]"));
                var accessType2 = _webDriver.FindElement(By.CssSelector("#cbAccessRead"));
                new SelectElement(accessType2).SelectByValue("ForAll");
                _webDriver.Navigate().Refresh();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nie rób wybrana była opcja "Dla wszystkich"
            }

            // wybrać z listy rozwijanej przeglądanie "Na hasło"
            var accessType = _webDriver.FindElement(By.CssSelector("#cbAccessRead"));
            new SelectElement(accessType).SelectByValue("ByPassword");

            //wpisać "Nowe hasło"
            var previewPassword = _webDriver.FindElement(By.CssSelector("#read_Passowrd"));
            previewPassword.SendKeys(password);

            //powtórzyć nowe hasło
            var confirmPreviewPassword = _webDriver.FindElement(By.CssSelector("#read_ConfirmPassword"));
            confirmPreviewPassword.SendKeys(password);

            //kliknąć "Zapisz"
            var submitButton = _webDriver.FindElement(By.CssSelector("#passwordReadForm input[type=submit].greenButtonCSS"));
            submitButton.Click();

            try
            {
                //Sprawdzenie czy hasło do przeglądania plików zostało utworzone
                _webDriver.FindElement(By.CssSelector("#changeReadPassBtn"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Creating access password is failed");
                return;
            }

            Assert.Pass();

        }

        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Hasła dostępu' poprawne ustawienie haseł dostępu innych użytkowników do modyfikacji zawartości")]
        public void Test12()
        {
            string password = "abc123";

            LogInSteps();

            ClickOptionStep();

            ClickAccessTabStep();

            try
            {
                //Jeżeli hasło jest już ustawione to przywracamy do opcji "Tylko właściciel"
                var optionByPasswordSelected = _webDriver.FindElement(By.CssSelector("#cbAccessControl .OptionByPassword[selected]"));
                var accessType2 = _webDriver.FindElement(By.CssSelector("#cbAccessControl"));
                new SelectElement(accessType2).SelectByValue("OwnerOnly");
                _webDriver.Navigate().Refresh();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nie rób wybrana była opcja "Tylko właściciel"
            }

            // wybrać z listy rozwijanej modyfikację zawartości "Na hasło"
            var accessType = _webDriver.FindElement(By.CssSelector("#cbAccessControl"));
            new SelectElement(accessType).SelectByValue("ByPassword");

            //wpisać "Nowe hasło"
            var previewPassword = _webDriver.FindElement(By.CssSelector("#control_Passowrd"));
            previewPassword.SendKeys(password);

            //powtórzyć nowe hasło
            var confirmPreviewPassword = _webDriver.FindElement(By.CssSelector("#control_ConfirmPassword"));
            confirmPreviewPassword.SendKeys(password);

            //kliknąć "Zapisz"
            var submitButton = _webDriver.FindElement(By.CssSelector("#passwordControlForm input[type=submit].greenButtonCSS"));
            submitButton.Click();

            try
            {
                //Sprawdzenie czy hasło do modyfikowania plików zostało utworzone
                _webDriver.FindElement(By.CssSelector("#changeControlPassBtn"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Creating access password is failed");
                return;
            }

            Assert.Pass();

        }

        [Test]
        [Category("User account operations tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zakładka 'Hasła dostępu' niepoprawne ustawienie haseł dostępu innych użytkowników do modyfikacji zawartości i weryfikacja komunikatów")]
        public void Test13()
        {
            LogInSteps();

            ClickOptionStep();

            //kliknąć zakładkę "Hasła dostępu"
            var accessPassword = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=passwords]"));
            accessPassword.Click();

            try
            {
                //Jeżeli hasło jest już ustawione to przywracamy do opcji "Tylko właściciel"
                var optionByPasswordSelected = _webDriver.FindElement(By.CssSelector("#cbAccessControl .OptionByPassword[value=ByPassword]"));
                var accessType2 = _webDriver.FindElement(By.CssSelector("#cbAccessControl"));
                new SelectElement(accessType2).SelectByValue("OwnerOnly");
                _webDriver.Navigate().Refresh();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nie rób wybrana była opcja "Tylko właściciel"
            }

            // wybrać z listy rozwijanej modyfikację zawartości "Na hasło"
            var accessType = _webDriver.FindElement(By.CssSelector("#cbAccessControl"));
            new SelectElement(accessType).SelectByValue("ByPassword");

            //wpisać "Nowe hasło"
            var previewPassword = _webDriver.FindElement(By.CssSelector("#control_Passowrd"));
            previewPassword.SendKeys(":");
            _webDriver.FindElement(By.TagName("body")).Click();

            //Sprawdzenie, czy pojawi się komunikat "Hasło musi posiadać przynajmniej 6 znaków, w tym jedną dużą literę lub cyfrę albo znak specjalny."
            var shortPassword = _webDriver.FindElement(By.CssSelector("#passwordControlForm span[for=control_Passowrd]"));
            StringAssert.Contains("Hasło musi posiadać przynajmniej 6 znaków, w tym jedną dużą literę lub cyfrę albo znak specjalny.", shortPassword.Text);
        }

        private void ClickOptionStep()
        {
            var optionButton = _webDriver.FindElement(By.CssSelector("#topbarOptions"));
            optionButton.Click();
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

        private void ClickAccessTabStep()
        {
            var accessButton = _webDriver.FindElement(By.CssSelector("#tabMenu a[href$=passwords]"));
            accessButton.Click();
        }
    }
}