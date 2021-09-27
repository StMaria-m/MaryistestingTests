using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
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

        private string getFolderPassSelector(string newFolderName = null)
        {
            return $"{getFolderSelector(newFolderName)} .pass";
        }

        private string getSelectedFolderSelector(string newFolderName = null)
        {
            return $"{getFolderSelector(newFolderName)} .T_selected";
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
        public void AddNewFolderTest()
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
            //Sprawdzenie czy utworzono nowy folder
            Assert.DoesNotThrow(() => _webDriver.FindElement(By.CssSelector(getFolderSelector())));           
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Usuwanie folderu")]
        public void DeleteFolderTest()
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
            // Sprawdzenie czy element został usunięty
            Assert.Throws<NoSuchElementException>(() => _webDriver.FindElement(By.CssSelector(getFolderSelector())));
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Próba dodawania nowego folderu z niedozwoloną nazwą")]
        public void IncorrectAddFolder_wrongFolderNameTest()
        {
            //Arrange
            _folderName = ". ?//";

            LogInSteps();

            ClickUserAvatarStep();

            //Act
            //utwórz folder z niedozwolonymi znakami /";--
            var addFolder = _webDriver.FindElement(By.CssSelector(_addFolderButtonSelector));
            addFolder.Click();

            WaitForAction(_tooltipFolderNameInputSelector);

            var inputFoldername = _webDriver.FindElement(By.CssSelector(_tooltipFolderNameInputSelector));
            inputFoldername.SendKeys(_folderName);

            var submitButton = _webDriver.FindElement(By.CssSelector($"{_tooltipContentSelector} input.greenButtonCSS:not(.right)"));
            submitButton.Click();

            WaitForAction($"{_tooltipContentSelector} .errorText b");

            //Assert
            //Sprawdzenie, czy pojawi się komunikat "Nazwa nie może zawierać znaków: \ / : * ? " < > |."
            var errorElement = _webDriver.FindElement(By.CssSelector($"{_tooltipContentSelector} .errorText"));
            StringAssert.Contains("Nazwa nie może zawierać znaków: \\ / : * ? \" < > |.", errorElement.Text);
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zmiana nazwy folderu")]
        public void RenameFolderTest()
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
            //znajdź folder i w niego kliknij
            var fileToRename = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            fileToRename.Click();

            WaitForAction(getSelectedFolderSelector());

            //kliknąć przycisk "Nazwa"
            var renameButton = _webDriver.FindElement(By.CssSelector($"{_treeActionButtonsSelector} .treeChangeName"));
            renameButton.Click();

            WaitForAction(_tooltipFolderNameInputSelector);

            //wpisać nową nazwę
            var inputNewName = _webDriver.FindElement(By.CssSelector(_tooltipFolderNameInputSelector));
            inputNewName.Clear();
            inputNewName.SendKeys(newFolderName);

            //kliknąć "OK"
            var sumbitButton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
            sumbitButton.Click();

            WaitForAction(getFolderSelector(newFolderName));

            //Assert
            //Sprawdzenie czy zmieniono nazwę folderu
            Assert.DoesNotThrow(() => _webDriver.FindElement(By.CssSelector(getFolderSelector(newFolderName))));
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Ustawienie hasła dostępu do folderu")]
        public void SetAccessPasswordTest()
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

                WaitForAction(getSelectedFolderSelector());

                var removePassword = _webDriver.FindElement(By.CssSelector(_tooltipRemovePasswordButtonSelector));
                removePassword.Click();

                Thread.Sleep(1000);
            }
            catch
            {
                //nic nie rób
            }

            //Act
            //znajdź i kliknij folder do usuniecia
            var currentFolder = _webDriver.FindElement(By.CssSelector(getFolderSelector()));
            currentFolder.Click();

            WaitForAction(getSelectedFolderSelector());

            //kliknąć przycisk "Hasło"
            var passwordButton1 = _webDriver.FindElement(By.CssSelector(_passwordButtonSelector));
            passwordButton1.Click();

            WaitForAction("#Password");

            //wpisać hasło
            var inputFolder1Password = _webDriver.FindElement(By.CssSelector("#Password"));
            inputFolder1Password.SendKeys("111111");

            //kliknąć "OK"
            var confirmButton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
            confirmButton.Click();

            WaitForAction(getFolderPassSelector());

            //Assert
            //sprawdzenie dodania hasła - czy obok folderu pojawia się symbol kłódki
            Assert.DoesNotThrow(() => _webDriver.FindElement(By.CssSelector(getFolderPassSelector())));
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Usunięcie hasła dostępu do folderu")]
        public void DeleteAccessPasswordTest()
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

            WaitForAction(getSelectedFolderSelector());

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

                WaitForAction("#Password");

                //wpisać hasło
                var inputFolder1Password = _webDriver.FindElement(By.CssSelector("#Password"));
                inputFolder1Password.SendKeys("111111");

                //kliknąć "OK"
                var confirmButton = _webDriver.FindElement(By.CssSelector(_tooltipOkButtonSelector));
                confirmButton.Click();

                WaitForAction(getFolderPassSelector());//nie wiem co tu czeka?
            }

            //Act
            //klinkąć "hasło"
            var passwordButton1 = _webDriver.FindElement(By.CssSelector(_passwordButtonSelector));
            passwordButton1.Click();

            WaitForAction(_tooltipRemovePasswordButtonSelector);

            //kliknąć symbol usuwania hasła
            var removePassword = _webDriver.FindElement(By.CssSelector(_tooltipRemovePasswordButtonSelector));
            removePassword.Click();

            Thread.Sleep(1000);

            //Assert
            //sprawdzenie czy jest symbol kłódki obok folderu, jeśli nie hasło usunięto
            Assert.Throws<NoSuchElementException>(() => _webDriver.FindElement(By.CssSelector(getFolderPassSelector())));
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Dodanie folderu o takiej samej nazwie w tej samej lokalizacji")]
        public void IncorrectAddFolder_doubleFolderNameTest()
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

            Thread.Sleep(1000);

            //Assert
            //sprawdzić komunikat walidacyjny "Folder o tej nazwie już istnieje."
            var errorText = _webDriver.FindElement(By.CssSelector($"{_tooltipContentSelector} .errorText"));
            StringAssert.Contains("Folder o tej nazwie już istnieje.", errorText.Text);
        }

        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Zmiana nazwy folderu tak by utworzyć dubla istniejącej już nazwy innego folderu - weryfikacja komunikatu walidacyjnego")]
        public void IncorrectRenameFolder_doubleFolderNameTest()
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

            //czeka na zaznaczenie folderu
            WaitForAction(getSelectedFolderSelector(doubleFolderName));


            //kliknąć przycisk "Nazwa"
            var renameButton = _webDriver.FindElement(By.CssSelector($"{_treeActionButtonsSelector} .treeChangeName"));
            renameButton.Click();

            WaitForAction(_tooltipFolderNameInputSelector);

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

        private void AddNewFolder(string newFolderName)
        {
            var addSameFolder = _webDriver.FindElement(By.CssSelector($"{_addFolderButtonSelector}"));
            addSameFolder.Click();

            WaitForAction(_tooltipFolderNameInputSelector);

            var inputFolderName = _webDriver.FindElement(By.CssSelector(_tooltipFolderNameInputSelector));
            inputFolderName.SendKeys(newFolderName);

            var OKbutton = _webDriver.FindElement(By.CssSelector($"{_tooltipOkButtonSelector}"));
            OKbutton.Click();

            WaitForAction(getFolderSelector(newFolderName));
        }

        private void DeleteFolder(string folderName)
        {
            string allFoldersSelector = $"#TreeContainer > div > table > tbody > tr > td > table > tbody > tr";
            var allFoldersElements = _webDriver.FindElements(By.CssSelector(allFoldersSelector));

            var fileToRmove = _webDriver.FindElement(By.CssSelector(getFolderSelector(folderName)));
            fileToRmove.Click();

            WaitForAction(getSelectedFolderSelector(folderName));

            var deleteButton = _webDriver.FindElement(By.CssSelector(_deleteButtonSelector));
            deleteButton.Click();

            WaitForAction(_tooltipContentSelector);

            var submitButton = _webDriver.FindElement(By.CssSelector($"{_tooltipContentSelector} input.greenButtonCSS.left"));
            submitButton.Click();

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            wait.Until(webDriver => {
                var elementsAfter = webDriver.FindElements(By.CssSelector(allFoldersSelector));
                return elementsAfter.Count < allFoldersElements.Count;
            });
        }
    }
}

