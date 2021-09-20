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
        private static string _tooltipRemovePasswordButtonSelector = $"{_tooltipContentSelector} .removePasswordBtn";
        private static string _tooltipFolderNameInputSelector = $"{_tooltipContentSelector} #FolderName";

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
            //znajdź folder
            _folderName = "folder u Darka";

            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                //Jeżeli już istnieje, to go usuń
                DeleteFolder(_folderName);
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //nic nie rób, nie ma takiego folderu
            }

            //Act
            //dodać nowy folder
            AddNewFolder(_folderName);

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

            try
            {
                //znajdź 'folder do usunięcia',jeśli go nie ma to stwórz
                _webDriver.FindElement(By.CssSelector(_folderName));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //utworzenie nowego folderu
                AddNewFolder(_folderName);
            }

            //Act
            //Znaleźć folder, zaznaczyć i usunąć
            DeleteFolder(_folderName);

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
            _folderName = ". ?//";

            LogInSteps();

            ClickUserAvatarStep();

            //Act
            //utwórz folder z niedozwolonymi znakami /";--
            var addFolder = _webDriver.FindElement(By.CssSelector(_addFolderButtonSelector));
            addFolder.Click();

            Thread.Sleep(1000);

            var inputFoldername = _webDriver.FindElement(By.CssSelector(_tooltipFolderNameInputSelector));
            inputFoldername.SendKeys(_folderName);

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
                DeleteFolder(newFolderName);
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
                AddNewFolder(_folderName);
            }

            //Act
            //znajdź folder
            var fileToRename = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            fileToRename.Click();

            Thread.Sleep(1000);

            //kliknąć przycisk "Nazwa"
            var renameButton = _webDriver.FindElement(By.CssSelector($"{_treeActionButtonsSelector} .treeChangeName"));
            renameButton.Click();

            Thread.Sleep(1000);

            //wpisać nową nazwę
            var inputNewName = _webDriver.FindElement(By.CssSelector(_tooltipFolderNameInputSelector));
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
                _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            }
            catch
            {
                //jeśli nie ma, to utwórz
                AddNewFolder(_folderName);
            }

            try
            {
                // sprawdzić czy jest ustawione hasło dostępu do folderu testowego, jeżeli jest to usunąć
                _webDriver.FindElement(By.CssSelector(getFolderPassSelector()));

                var passwordButton = _webDriver.FindElement(By.CssSelector(_passwordButtonSelector));
                passwordButton.Click();

                Thread.Sleep(1000);

                var removePassword = _webDriver.FindElement(By.CssSelector(_tooltipRemovePasswordButtonSelector));
                removePassword.Click();

                Thread.Sleep(1000);
            }
            catch
            {
                //nic nie rób
            }

            //Act
            var currentFolder = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            currentFolder.Click();

            Thread.Sleep(1000);

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
                AddNewFolder(_folderName);
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
            var removePassword = _webDriver.FindElement(By.CssSelector(_tooltipRemovePasswordButtonSelector));
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

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Dodanie folderu o takiej samej nazwie w tej samej lokalizacji")]
        public void Test5555()
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
                AddNewFolder(_folderName);
            }

            //Act
            //dodać taki sam folder
            AddNewFolder(_folderName);

            //Assert
            //sprawdzić komunikat walidacyjny "Folder o tej nazwie już istnieje."
            var errorText = _webDriver.FindElement(By.CssSelector($"{_tooltipContentSelector} .errorText"));
            StringAssert.Contains("Folder o tej nazwie już istnieje.", errorText.Text);

        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zmiana nazwy folderu tak by utworzyć dubla istniejącej już nazwy innego folderu - weryfikacja komunikatu walidacyjnego")]
        public void Test666()
        {
            //Arrange
            _folderName = "identyczny";
            string doubleFolderName = "nowa nazwa";

            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                // sprawdzić czy istnieje folder o nazwie "identyczny"
                var currentFolder = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            }
            catch
            {
                //jeśli nie ma to utwórz
                AddNewFolder(_folderName);
            }

            try
            {
                // sprawdzić czy istnieje folder o nazwie "nowa nazwa"
                _webDriver.FindElement(By.CssSelector(getFolderSelector(doubleFolderName)));
            }

            catch
            {
                //jeśli nie ma to utwórz
                AddNewFolder(doubleFolderName);
            }

            //Act
            //znaleźć folder o innej nazwie, np. 'nowa nazwa' i zmienić nazwę na 'identyczny'
            var anotherFolder = _webDriver.FindElement(By.CssSelector(getFolderSelector(doubleFolderName)));
            anotherFolder.Click();

            Thread.Sleep(1000);

            //kliknąć przycisk "Nazwa"
            var renameButton = _webDriver.FindElement(By.CssSelector($"{_treeActionButtonsSelector} .treeChangeName"));
            renameButton.Click();

            Thread.Sleep(1000);

            //wpisać nową nazwę
            var inputNewName = _webDriver.FindElement(By.CssSelector(_tooltipFolderNameInputSelector));
            inputNewName.Clear();
            inputNewName.SendKeys(_folderName);

            //kliknąć "OK"
            var sumbitButton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
            sumbitButton.Click();

            Thread.Sleep(1000);

            //sprawdzić komunikat walidacyjny "Folder o tej nazwie już istnieje."
            var errorText = _webDriver.FindElement(By.CssSelector($"{_tooltipContentSelector} .errorText"));
            StringAssert.Contains("Folder o tej nazwie już istnieje.", errorText.Text);
        }

        private void ClickUserAvatarStep()
        {
            var userAccount = _webDriver.FindElement(By.CssSelector("#topbarAvatar .friendSmall"));
            userAccount.Click();

            RemoveAcceptContainer();
        }

        private void AddNewFolder(string newFolderName)
        {
            var addSameFolder = _webDriver.FindElement(By.CssSelector($"{_addFolderButtonSelector}"));
            addSameFolder.Click();

            Thread.Sleep(1000);

            var inputFolderName = _webDriver.FindElement(By.CssSelector(_tooltipFolderNameInputSelector));
            inputFolderName.SendKeys(newFolderName);

            var OKbutton = _webDriver.FindElement(By.CssSelector($"{_tooltipOkButtonSelector}"));
            OKbutton.Click();

            Thread.Sleep(1000);
        }

        private void DeleteFolder(string folderName)
        {
            var renamedFile = _webDriver.FindElement(By.CssSelector(getFolderSelector(folderName)));
            renamedFile.Click();

            Thread.Sleep(1000);

            var deleteButton = _webDriver.FindElement(By.CssSelector(_deleteButtonSelector));
            deleteButton.Click();

            Thread.Sleep(1000);

            var submitButton = _webDriver.FindElement(By.CssSelector($"{_tooltipContentSelector} input.greenButtonCSS.left"));
            submitButton.Click();

            Thread.Sleep(1000);
        }
    }
}

