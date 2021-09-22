using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace TomikTests
{
    public class FriendsGroupTests: BaseTest
    {
        private string groupContainerSelector = "#favoriteTab";

        [Test]
        [Category("Search files tests next")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Utworzenie grupy znajomych w sekcji 'zaprzyjaźnione i polecane chomiki'")]
        public void CreateNewFriendsGroupTest()
        {
            string friendsGroupName = "nowaGrupa";
            var friendsGroupNameSelector = $"{groupContainerSelector} [title={friendsGroupName}]";

            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                //sprawdzenie czy nazwa grupy, którą chcemy dodać już istnieje, jeśli tak, to trzeba ją usunąć
                _webDriver.FindElement(By.CssSelector(friendsGroupNameSelector));

                DeleteFriendsGroup(friendsGroupName);
            }
            catch
            {
                //nic nie rób, nie ma takiej grupy
            }

            //kliknać w przycisk "utwórz nową grupę znajomych"
            var addFriendsGroupButton = _webDriver.FindElement(By.CssSelector("#FriendsGroupAdd"));
            addFriendsGroupButton.Click();

            var addFriendsGroupInput = _webDriver.FindElement(By.CssSelector("#FrGrpChangeName"));
            addFriendsGroupInput.SendKeys(friendsGroupName);

            //zapisz utworzoną grupę
            var confirmAddNewGroup = _webDriver.FindElement(By.CssSelector("#ConfirmGroupChangeNameButton"));
            confirmAddNewGroup.Click();

            WaitForAction(friendsGroupNameSelector);

            try
            {
                //sprawdzić czy utworzono grupę
                _webDriver.FindElement(By.CssSelector(friendsGroupNameSelector));
            }
            catch
            {
                Assert.Fail("Group is not added");
                return;
            }

            Assert.Pass();
        }

        [Test]
        [Category("Search files tests next")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Usunięcie grupy znajomych w sekcji 'zaprzyjaźnione i polecane chomiki'")]
        public void DeleteFriendsGroupTest()
        {
            string friendsGroupName = "kosz";
            var friendsGroupNameSelector = $"{groupContainerSelector} [title={friendsGroupName}]";

            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                //sprawdzenie czy nazwa grupy, którą chcemy usunąć już istnieje, jeśli nie ma to utworzyć
                _webDriver.FindElement(By.CssSelector(friendsGroupNameSelector));
            }
            catch
            {
                AddFriendsGroup(friendsGroupName);
            }

            DeleteFriendsGroup(friendsGroupName);

            //sprawdzenie, czy grupa została usunięta?????
            try
            {
                _webDriver.FindElement(By.CssSelector($"{groupContainerSelector} [title={friendsGroupName}]"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Assert.Pass();
                return;
            }

            Assert.Fail("Folder has not been removed.");
        }

        [Test]
        [Category("Search files tests next")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Utworzenie duplikatu nazwy grupy znajomych w sekcji 'zaprzyjaźnione i polecane chomiki'")]
        public void AddTheSameNameFriendsGroupTest()
        {
            string friendsGroupName = "duplikat";
            var friendsGroupNameSelector = $"{groupContainerSelector} [title={friendsGroupName}]";

            LogInSteps();

            ClickUserAvatarStep();

            try
            {
                //sprawdzenie czy nazwa grupy, którą chcemy dodać już istnieje, jeśli nie ma to utworzyć
                _webDriver.FindElement(By.CssSelector(friendsGroupNameSelector));
            }
            catch
            {
                AddFriendsGroup(friendsGroupName);
            }

            //dodać duplikat nazwy grupy
            AddFriendsGroup(friendsGroupName);

            WaitForAction("#GroupNoticeMessage");

            //sprawdzić komunikat walidacyjny
            var errorText = _webDriver.FindElement(By.CssSelector("#GroupNoticeMessage"));
            StringAssert.Contains("Taka grupa już istnieje", errorText.Text);
        }

        private void DeleteFriendsGroup(string groupName)
        {
            var friendsGroupNameSelector = $"{groupContainerSelector} [title={groupName}]";

            string allGroupsSelector = $"{groupContainerSelector} .FavTab";
            var allGroupsElements = _webDriver.FindElements(By.CssSelector(allGroupsSelector));

            var currentGroup = _webDriver.FindElement(By.CssSelector(friendsGroupNameSelector));
            currentGroup.Click();

            WaitForAction("#delGroup .delLink");

            var deleteGroupButton = _webDriver.FindElement(By.CssSelector("#delGroup .delLink"));
            deleteGroupButton.Click();

            var confirmDeleteGroup = _webDriver.FindElement(By.CssSelector("#delGroupYes"));
            confirmDeleteGroup.Click();

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            wait.Until(webDriver =>
            {
                var elementsAfterDelete = webDriver.FindElements(By.CssSelector(allGroupsSelector));
                return elementsAfterDelete.Count < allGroupsElements.Count;
            });
        }

        private void AddFriendsGroup(string groupName)
        {
            var friendsGroupNameSelector = $"{groupContainerSelector} [title={groupName}]";

            //utworzyć grupę
            var addFriendsGroupButton = _webDriver.FindElement(By.CssSelector("#FriendsGroupAdd"));
            addFriendsGroupButton.Click();

            var addFriendsGroupInput = _webDriver.FindElement(By.CssSelector("#FrGrpChangeName"));
            addFriendsGroupInput.SendKeys(groupName);

            //zapisz utworzoną grupę
            var confirmAddNewGroup = _webDriver.FindElement(By.CssSelector("#ConfirmGroupChangeNameButton"));
            confirmAddNewGroup.Click();

            WaitForAction(friendsGroupNameSelector);
        }
    }
}
