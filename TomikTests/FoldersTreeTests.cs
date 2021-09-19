using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;


namespace TomikTests
{
    public class FoldersTreeTests : BaseTest
    {
        private static string _folderName = "identyczny";

        private string getFolderSelector(string newFolderName = null)
        {
            string folderName = string.IsNullOrEmpty(newFolderName) ? _folderName : newFolderName;
            return $"#TreeContainer > div > table > tbody > tr > td > table > tbody > tr > td > [title='{folderName}']";
        }

        private string getFolderPassSelector()
        {
            return $"{getFolderSelector()} .pass";
        }

        private static string _tooltipContentSelector = "#ui-tooltip-editFolderWindow-content";
        private static string _tooltipOkButtonSelector = $"{_tooltipContentSelector} .greenButtonCSS[value=OK]";
        private static string _tooltiPremovePasswordButtonSelector = $"{_tooltipContentSelector} .removePasswordBtn";

        private static string _treeActionButtonsSelector = "#foldersTreeActionButtons";
        private static string _addFolderButtonSelector = $"{_treeActionButtonsSelector} .treeAddFolder";
        private static string _passwordButtonSelector = $"{_treeActionButtonsSelector} .treeChangePassword";
        private static string _deleteButtonSelector = $"{_treeActionButtonsSelector} .treeDeleteFolder";       

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Dodawanie nowego folderu")]
        public void Test1()
        {
            //Arrange
            _folderName = "folder u Darka";

            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                //Jeżeli już istnieje, to go usuń
                var folderToRemove = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
                folderToRemove.Click();

                Thread.Sleep(500);

                var deleteButton = _webDriver.FindElement(By.CssSelector(_deleteButtonSelector));
                deleteButton.Click();

                Thread.Sleep(1000);

                var submitButton = _webDriver.FindElement(By.CssSelector("#ui-tooltip-editFolderWindow-content input.greenButtonCSS.left"));
                submitButton.Click();

                Thread.Sleep(1000);
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nie rób, nie ma takiego folderu
            }

            //Act
            //dodać nowy folder
            var newFolder = _webDriver.FindElement(By.CssSelector(_addFolderButtonSelector));
            newFolder.Click();

            Thread.Sleep(1000);

            //wpisać nazwę folderu
            var inputFoldername = _webDriver.FindElement(By.CssSelector("#FolderName"));
            inputFoldername.SendKeys(_folderName);

            //zaznaczyć checkbox 18+
            var adultCheckbox = _webDriver.FindElement(By.CssSelector("#AdultContent"));
            adultCheckbox.Click();

            //zaznaczyć hasło
            var setFolder1Password = _webDriver.FindElement(By.CssSelector("#NewFolderSetPassword"));
            setFolder1Password.Click();

            //wpisać hasło
            var inputFolder1Password = _webDriver.FindElement(By.CssSelector("#Password"));
            inputFolder1Password.SendKeys("111111");

            //kliknąć "OK"
            var OKbutton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
            OKbutton.Click();

            Thread.Sleep(1000);

            //Assert
            try
            {
                //Sprawdzenie czy utworzono nowy folder
                var newFolderName = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Folder not found");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Usuwanie folderu")]
        public void Test2()
        {
            //Arrange
            _folderName = "folder do usunięcia";

            LogInSteps();

            ClickUserAvatarStep();

            IWebElement folderToRemove;
            IWebElement deleteButton;
            IWebElement submitButton;

            try
            {
                //znajdź 'folder_do_usunięcia',jeśli go nie ma to stwórz
               _webDriver.FindElement(By.CssSelector(_folderName));               
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //jeśli nie ma folderu folder_do_usunięcia to utwórz
                var addFolder = _webDriver.FindElement(By.CssSelector(_addFolderButtonSelector));
                addFolder.Click();

                Thread.Sleep(500);

                var inputFoldername = _webDriver.FindElement(By.CssSelector("#FolderName"));
                inputFoldername.SendKeys(_folderName);

                var OKbutton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
                OKbutton.Click();

                Thread.Sleep(1000);
            }           

            //Act
            folderToRemove = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            folderToRemove.Click();

            Thread.Sleep(500);

            deleteButton = _webDriver.FindElement(By.CssSelector(_deleteButtonSelector));
            deleteButton.Click();

            Thread.Sleep(1000);

            submitButton = _webDriver.FindElement(By.CssSelector("#ui-tooltip-editFolderWindow-content input.greenButtonCSS.left"));
            submitButton.Click();

            Thread.Sleep(1000);

            //Assert
            try
            {
                // Sprawdzenie czy element został usunięty
                _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Pass("Folder is removed");
                return;
            }

            Assert.Fail();
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Dodawanie nowego folderu z niedozwoloną nazwą")]
        public void Test3()
        {
            //Arrange
            LogInSteps();

            ClickUserAvatarStep();

            //Act
            var addFolder = _webDriver.FindElement(By.CssSelector(_addFolderButtonSelector));
            addFolder.Click();

            Thread.Sleep(1000);

            var inputFoldername = _webDriver.FindElement(By.CssSelector("#FolderName"));
            inputFoldername.SendKeys(". ?//");

            var submitButton = _webDriver.FindElement(By.CssSelector($"{_tooltipContentSelector} input.greenButtonCSS:not(.right)"));
            submitButton.Click();

            Thread.Sleep(1000);

            //Assert
            //Sprawdzenie, czy pojawi się komunikat "Nazwa nie może zawierać znaków: \ / : * ? " < > |."
            var errorElement = _webDriver.FindElement(By.CssSelector($"{_tooltipContentSelector} .errorText"));
            StringAssert.Contains("Nazwa nie może zawierać znaków: \\ / : * ? \" < > |.", errorElement.Text);
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zmiana nazwy istniejącego folderu")]
        public void Test4()
        {
            //Arrange
            _folderName = "stara nazwa";
            string newFolderName = "nowa nazwa";
            
            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                //sprawdzić, czy istnieje folder testowy na którego chcemy zmienić nazwę, jeżeli istnieje to go usuń
                var renamedFile = _webDriver.FindElement(By.CssSelector(getFolderSelector(newFolderName)));
                renamedFile.Click();

                Thread.Sleep(500);

                var deleteButton = _webDriver.FindElement(By.CssSelector(_deleteButtonSelector));
                deleteButton.Click();

                Thread.Sleep(1000);

                var submitButton = _webDriver.FindElement(By.CssSelector("#ui-tooltip-editFolderWindow-content input.greenButtonCSS.left"));
                submitButton.Click();

                Thread.Sleep(1000);
            }
            catch
            {
                // jeśli nie istnieje to nic nie rób
            }

            try
            {
                //znajdź folder testowy
                _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //jeśli nie ma to utwórz
                var addFolder = _webDriver.FindElement(By.CssSelector(_addFolderButtonSelector));
                addFolder.Click();

                Thread.Sleep(500);

                var inputFolderName = _webDriver.FindElement(By.CssSelector("#FolderName"));
                inputFolderName.SendKeys(_folderName);

                var OKbutton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
                OKbutton.Click();

                Thread.Sleep(1000);
            }

            //Act
            //znajdź folder
            var fileToRename = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            fileToRename.Click();

            Thread.Sleep(1000);

            //kliknąć przycisk "Nazwa"
            var renameButton = _webDriver.FindElement(By.CssSelector("#foldersTreeActionButtons li .treeChangeName"));
            renameButton.Click();

            Thread.Sleep(1000);

            //wpisać nową nazwę
            var inputNewName = _webDriver.FindElement(By.CssSelector("#FolderName"));
            inputNewName.Clear();
            inputNewName.SendKeys(newFolderName);

            //kliknąć "OK"
            var sumbitButton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
            sumbitButton.Click();

            Thread.Sleep(1000);

            //Assert
            try
            {
                //Sprawdzenie czy zmieniono nazwę folderu
                _webDriver.FindElement(By.CssSelector(getFolderSelector(newFolderName)));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Folder name is not changed");
                return;
            }

            Assert.Pass();

        }         

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Ustawienie hasła dostępu do folderu")]
        public void Test5()
        {
            //Arrange            
            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                // sprawdzić czy istnieje folder testowy
                var currentFolder = _webDriver.FindElement(By.CssSelector(_folderName));
                currentFolder.Click();
            }
            catch
            {
                //jeśli nie ma, to utwórz
                var addSameFolder = _webDriver.FindElement(By.CssSelector(_addFolderButtonSelector));
                addSameFolder.Click();

                Thread.Sleep(1000);

                var inputFolderName = _webDriver.FindElement(By.CssSelector("#FolderName"));
                inputFolderName.SendKeys("identyczny");

                var okButton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
                okButton.Click();

                Thread.Sleep(1000);
            }


            try
            {
                // sprawdzić czy jest ustawione hasło dostępu do folderu testowego, jeżeli jest to usunąć
                _webDriver.FindElement(By.CssSelector(getFolderPassSelector()));

                var passwordButton = _webDriver.FindElement(By.CssSelector(_passwordButtonSelector));
                passwordButton.Click();

                Thread.Sleep(1000);

                var removePassword = _webDriver.FindElement(By.CssSelector(_tooltiPremovePasswordButtonSelector));
                removePassword.Click();

                Thread.Sleep(1000);
            }
            catch
            {
                //nic nie rób
            }

            //Act

            //kliknąć przycisk "Hasło"
            var passwordButton1 = _webDriver.FindElement(By.CssSelector(_passwordButtonSelector));
            passwordButton1.Click();

            Thread.Sleep(3000);

            //wpisać hasło
            var inputFolder1Password = _webDriver.FindElement(By.CssSelector("#Password"));
            inputFolder1Password.SendKeys("111111");

            //kliknąć "OK"
            var confirmButton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
            confirmButton.Click();

            Thread.Sleep(1000);

            //Assert
            try
            {
                //sprawdzenie dodania hasła - czy obok folderu pojawia się symbol kłódki
                _webDriver.FindElement(By.CssSelector(getFolderPassSelector()));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Fail("Password doesn't exist");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Usunięcie hasła dostępu do folderu")]
        public void Test6()
        {
            //Arrange
            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                // sprawdzić czy istnieje folder o nazwie "identyczny"
                _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            }
            catch
            {
                //jeśli nie ma, to utwórz
                var addSameFolder = _webDriver.FindElement(By.CssSelector(_addFolderButtonSelector));
                addSameFolder.Click();

                Thread.Sleep(1000);

                var inputFolderName = _webDriver.FindElement(By.CssSelector("#FolderName"));
                inputFolderName.SendKeys("identyczny");

                var okButton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
                okButton.Click();

                Thread.Sleep(1000);
            }

            //Zaznacz folder testowy
            var currentFolder = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            currentFolder.Click();

            Thread.Sleep(1000);

            try
            {
                // sprawdzenie czy jest hasło dostępu do folderu testowego
                 _webDriver.FindElement(By.CssSelector(getFolderPassSelector()));
            }
            catch 
            {
                //dodawanie hasła dostępu do folderu - kliknąć przycisk "Hasło"
                var passwordButton2 = _webDriver.FindElement(By.CssSelector(_passwordButtonSelector));
                passwordButton2.Click();

                Thread.Sleep(3000);

                //wpisać hasło
                var inputFolder1Password = _webDriver.FindElement(By.CssSelector("#Password"));
                inputFolder1Password.SendKeys("111111");

                //kliknąć "OK"
                var confirmButton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
                confirmButton.Click();

                Thread.Sleep(1000);
            }

            //Act
            //klinkąć "hasło"
            var passwordButton1 = _webDriver.FindElement(By.CssSelector(_passwordButtonSelector));
            passwordButton1.Click();

            Thread.Sleep(3000);

            //kliknąć symbol usuwania hasła
            var removePassword = _webDriver.FindElement(By.CssSelector(_tooltiPremovePasswordButtonSelector));
            removePassword.Click();

            Thread.Sleep(1000);

            //Assert
            try
            {
                //sprawdzenie czy jest symbol kłódki obok folderu, jeśli nie hasło usunięto
                _webDriver.FindElement(By.CssSelector(getFolderPassSelector()));
            }

            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Pass("Password is removed");
                return;
            }

            Assert.Fail("Password is not removed");


        }

        private void ClickUserAvatarStep()
        {
            var userAccount = _webDriver.FindElement(By.CssSelector("#topbarAvatar .friendSmall"));
            userAccount.Click();

            RemoveAcceptContainer();
        }
    }
}

