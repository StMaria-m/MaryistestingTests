using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;


namespace TomikTests
{
    public class FoldersTreeTests : BaseTest
    {                
        [Test]
        [Category("Folders tree tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Dodawanie nowego folderu")]
        public void Test1()
        {
            string folderName = "folder u Darka";
            string newDirSelector = $"#TreeContainer > div > table > tbody > tr > td > table > tbody > tr > td > [title='{folderName}']";

            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                //Jeżeli 'folder1' już istnieje, to go usuń
                var folderToRemove = _webDriver.FindElement(By.CssSelector(newDirSelector));
                folderToRemove.Click();

                Thread.Sleep(500);

                var deleteButton = _webDriver.FindElement(By.CssSelector("#foldersTreeActionButtons .treeDeleteFolder"));
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

            //dodać nowy folder
            var newFolder = _webDriver.FindElement(By.CssSelector("#foldersTreeActionButtons [alt=Nowy]"));
            newFolder.Click();

            Thread.Sleep(1000);

            //wpisać nazwę folderu
            var inputFoldername = _webDriver.FindElement(By.CssSelector("#FolderName"));
            inputFoldername.SendKeys(folderName);

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
            var OKbutton = _webDriver.FindElement(By.CssSelector("#ui-tooltip-editFolderWindow-content .greenButtonCSS[value=OK]"));
            OKbutton.Click();

            Thread.Sleep(1000);

            try
            {
                //Sprawdzenie czy utworzono nowy folder
                var newFolderName = _webDriver.FindElement(By.CssSelector(newDirSelector));
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
            string folderName = "folder do usunięcia";
            string removeDirSelector = $"#TreeContainer > div > table > tbody > tr > td > table > tbody > tr > td > a[title='{folderName}']";

            LogInSteps();

            ClickUserAvatarStep();

            IWebElement folderToRemove;
            IWebElement deleteButton;
            IWebElement submitButton;

            try
            {
                //znajdź 'folder_do_usunięcia',jeśli go nie ma to stwórz
               _webDriver.FindElement(By.CssSelector(removeDirSelector));               
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                //jeśli nie ma folderu folder_do_usunięcia to utwórz
                var addFolder = _webDriver.FindElement(By.CssSelector("#foldersTreeActionButtons [alt=Nowy]"));
                addFolder.Click();

                Thread.Sleep(500);

                var inputFoldername = _webDriver.FindElement(By.CssSelector("#FolderName"));
                inputFoldername.SendKeys(folderName);

                var OKbutton = _webDriver.FindElement(By.CssSelector("#ui-tooltip-editFolderWindow-content .greenButtonCSS[value=OK]"));
                OKbutton.Click();

                Thread.Sleep(1000);
            }           

            folderToRemove = _webDriver.FindElement(By.CssSelector(removeDirSelector));
            folderToRemove.Click();

            Thread.Sleep(500);

            deleteButton = _webDriver.FindElement(By.CssSelector("#foldersTreeActionButtons .treeDeleteFolder"));
            deleteButton.Click();

            Thread.Sleep(1000);

            submitButton = _webDriver.FindElement(By.CssSelector("#ui-tooltip-editFolderWindow-content input.greenButtonCSS.left"));
            submitButton.Click();

            Thread.Sleep(1000);

            try
            {
                // Sprawdzenie czy element został usunięty
                _webDriver.FindElement(By.CssSelector(removeDirSelector));
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
            LogInSteps();

            ClickUserAvatarStep();

            var addFolder = _webDriver.FindElement(By.CssSelector("#foldersTreeActionButtons [alt=Nowy]"));
            addFolder.Click();

            Thread.Sleep(1000);

            var inputFoldername = _webDriver.FindElement(By.CssSelector("#FolderName"));
            inputFoldername.SendKeys(". ?//");

            var submitButton = _webDriver.FindElement(By.CssSelector("#ui-tooltip-editFolderWindow-content input.greenButtonCSS:not(.right)"));
            submitButton.Click();

            Thread.Sleep(1000);

            //Sprawdzenie, czy pojawi się komunikat "Nazwa nie może zawierać znaków: \ / : * ? " < > |."
            var errorElement = _webDriver.FindElement(By.CssSelector("#ui-tooltip-editFolderWindow-content .errorText")); //////////////////sprawdzić ten element
            StringAssert.Contains("Nazwa nie może zawierać znaków: \\ / : * ? \" < > |.", errorElement.Text);
        }     

        private void ClickUserAvatarStep()
        {
            var userAccount = _webDriver.FindElement(By.CssSelector("#topbarAvatar .friendSmall"));
            userAccount.Click();

            RemoveAcceptContainer();
        }
    }
}

