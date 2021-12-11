using OpenQA.Selenium;
using RosChemistTests.Models;
using System.Threading;

namespace RosChemistTests.Helpers
{
    public static class ProcessStepHeplers
    {
        public static void LogInSteps(IWebDriver webDriver, AppSettingsModel appSettings)
        {
            webDriver.FindElement(By.CssSelector(".login-form [name='login']"))
                .SendKeys(appSettings.Login);

            webDriver.FindElement(By.CssSelector(".login-form [name='password']"))
                .SendKeys(appSettings.Password);

            webDriver.FindElement(By.CssSelector(".login-form [type='submit']"))
                .Click();

            Thread.Sleep(1000);
        }
    }
}
