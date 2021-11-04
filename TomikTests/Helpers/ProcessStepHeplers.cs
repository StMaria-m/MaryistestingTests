using OpenQA.Selenium;
using TomikTests.Models;
using TomikTests.Wrappers;

namespace TomikTests.Helpers
{
    public static class ProcessStepHeplers
    {
        public static void LogInSteps(BrowserWrapper browserUtility, AppSettingsModel appSettings)
        {
            var webDriver = browserUtility.GetWebDriver();

            //1. Uzupełnić pole "Chomik"
            var login = webDriver.FindElement(By.CssSelector("#topBarLogin"));
            login.SendKeys(appSettings.Login);

            //2. Uzupełnić pole "Hasło"
            var password = webDriver.FindElement(By.CssSelector("#topBarPassword"));
            password.SendKeys(appSettings.Password);

            //3. Kliknąć przycisk "Zaloguj"
            var createButton = webDriver.FindElement(By.CssSelector("#topBar_LoginBtn"));
            createButton.Click();

            browserUtility.WaitForAction("#logout");
        }
    }
}
