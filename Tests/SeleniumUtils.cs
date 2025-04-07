using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests
{
    /// <summary>
    /// Класс с вспомогательными методами для Selenium-тестов
    /// </summary>
    public static class SeleniumUtils
    {
        /// <summary>
        /// Делает скриншот экрана и сохраняет его по указанному пути
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="fileName">Имя файла без расширения</param>
        /// <returns>Полный путь к сохраненному файлу</returns>
        public static string TakeScreenshot(IWebDriver driver, string fileName)
        {
            // Создаем директорию для скриншотов, если она не существует
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Формируем имя файла с датой и временем
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fullPath = Path.Combine(directory, $"{fileName}_{timestamp}.png");

            // Делаем скриншот
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(fullPath, ScreenshotImageFormat.Png);

            return fullPath;
        }

        /// <summary>
        /// Ожидает загрузки страницы, проверяя состояние document.readyState
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="timeoutInSeconds">Таймаут в секундах</param>
        public static void WaitForPageLoaded(IWebDriver driver, int timeoutInSeconds = 30)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            
            // Ждем, пока document.readyState не станет 'complete'
            wait.Until(d => 
                ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            
            // Даем дополнительное время для отрисовки UI
            System.Threading.Thread.Sleep(500);
        }

        /// <summary>
        /// Выполняет JavaScript на странице
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="script">JavaScript-код</param>
        /// <returns>Результат выполнения скрипта</returns>
        public static object ExecuteJavaScript(IWebDriver driver, string script, params object[] args)
        {
            return ((IJavaScriptExecutor)driver).ExecuteScript(script, args);
        }

        /// <summary>
        /// Ожидает видимости элемента с логгированием ошибки
        /// </summary>
        /// <param name="driver">WebDriver</param>
        /// <param name="by">Локатор элемента</param>
        /// <param name="timeoutInSeconds">Таймаут в секундах</param>
        /// <returns>Найденный элемент или null, если элемент не найден</returns>
        public static IWebElement WaitForElementVisible(IWebDriver driver, By by, int timeoutInSeconds = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Элемент не найден: {by}");
                return null;
            }
        }
    }
} 