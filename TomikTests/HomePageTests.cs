using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;

namespace TomikTests
{
   public class HomePageTests: BaseTest
    {

        [Test]
        [Category("Home page tests")]
        [Author("Maria", "http://maryistesting.com")]
        [Description("Sprawdzenie przejścia na stronę przypomnienie hasła")]
        public void Test6()
        {
            //1. Kliknąć w przycisk "zapomniałem"
            var loginButton = _webDriver.FindElement(By.CssSelector(".forgotPass"));
            loginButton.Click();

            //2. Sprawdzenie przekierowania na stronę do przypomnienia hasła
            var url = _webDriver.Url;
            StringAssert.Contains("LostPassword.aspx", url);
        }
    }
}
