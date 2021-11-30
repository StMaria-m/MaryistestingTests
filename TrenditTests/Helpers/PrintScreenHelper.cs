using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TrenditTests.Models;

namespace TrenditTests.Helpers
{

    public class PrintScreenHelper
    {
        private AppSettingsModel _appSettings { get; set; }
        private IWebDriver _webDriver { get; set; }

        public PrintScreenHelper(AppSettingsModel appSettings, IWebDriver webDriver)
        {
            _appSettings = appSettings;
            _webDriver = webDriver;
        }

        public void TakeScreenShot(string methodName)
        {
            string folderPath = $"{AppDomain.CurrentDomain.BaseDirectory}//{_appSettings.ScreensFolder}";

            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            Screenshot ss = ((ITakesScreenshot)_webDriver).GetScreenshot();
            ss.SaveAsFile($"{folderPath}//{methodName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.png");
        }
    }
}
