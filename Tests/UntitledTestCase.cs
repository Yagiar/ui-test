using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTests
{
    [TestFixture]
    public class OdnoklassnikiLoginTest
    {
        private IWebDriver driver = null!;
        private StringBuilder verificationErrors = null!;
        private string baseURL = null!;
        private bool acceptNextAlert = true;
        private WebDriverWait wait = null!;
        // Задержка между действиями в миллисекундах (значение по умолчанию)
        private int stepDelay = 1500;
        
        [SetUp]
        public void SetupTest()
        {
            // Получаем значение задержки из настроек, если оно указано
            if (TestContext.Parameters.Exists("StepDelayMs") && 
                int.TryParse(TestContext.Parameters["StepDelayMs"], out int delay))
            {
                stepDelay = delay;
                Console.WriteLine($"Используется задержка: {stepDelay} мс");
            }

            // Инициализация драйвера Chrome вместо Firefox
            var options = new ChromeOptions();
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            
            driver = new ChromeDriver(options);
            
            // Максимизируем окно для лучшего отображения элементов
            driver.Manage().Window.Maximize();
            
            // Устанавливаем базовый URL 
            baseURL = "https://ok.ru/";
            
            // Инициализация объекта для сбора возможных ошибок верификации
            verificationErrors = new StringBuilder();
            
            // Создаем объект ожидания с таймаутом 10 секунд
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        
        [TearDown]
        public void TeardownTest()
        {
            try
            {
                // Закрываем драйвер после выполнения теста
                driver.Quit();
            }
            catch (Exception)
            {
                // Игнорируем ошибки при закрытии браузера
            }
            
            // Проверяем, что не было ошибок верификации в процессе теста
            Assert.AreEqual("", verificationErrors.ToString());
        }
        
        [Test]
        public void LoginWithInvalidCredentials()
        {
            try
            {
                // Открываем главную страницу
                driver.Navigate().GoToUrl(baseURL);
                Thread.Sleep(stepDelay);
                
                // Проверяем, что открылась страница OK.ru
                Assert.IsTrue(driver.Title.Contains("Одноклассники"), "Главная страница не загрузилась!");
                
                // Нажимаем на кнопку логина
                IWebElement loginButton = wait.Until(
                    ExpectedConditions.ElementToBeClickable(By.XPath("//input[@value='Войти в Одноклассники']")));
                loginButton.Click();
                Thread.Sleep(stepDelay);
                
                // Вводим невалидный email
                IWebElement emailField = wait.Until(
                    ExpectedConditions.ElementToBeClickable(By.Id("field_email")));
                emailField.Click();
                Thread.Sleep(stepDelay / 2);
                emailField.Clear();
                Thread.Sleep(stepDelay / 2);
                emailField.SendKeys("Afdsfdsf@mail.ru");
                Thread.Sleep(stepDelay);
                
                // Нажимаем на кнопку для перехода к вводу пароля
                driver.FindElement(By.XPath("//input[@value='Войти в Одноклассники']")).Click();
                Thread.Sleep(stepDelay);
                
                // Вводим невалидный пароль
                IWebElement passwordField = wait.Until(
                    ExpectedConditions.ElementToBeClickable(By.Id("field_password")));
                passwordField.Click();
                Thread.Sleep(stepDelay / 2);
                passwordField.Clear();
                Thread.Sleep(stepDelay / 2);
                passwordField.SendKeys("fsdfsdfdfs");
                Thread.Sleep(stepDelay);
                
                // Отправляем форму авторизации
                driver.FindElement(By.XPath("//input[@value='Войти в Одноклассники']")).Click();
                Thread.Sleep(stepDelay * 2); // Увеличенная задержка для проверки результата
                
                // Проверяем, что появилось сообщение об ошибке (невалидные учетные данные)
                bool hasError = wait.Until(d => {
                    try {
                        return IsElementPresent(By.XPath("//*[contains(text(), 'Неправильно указан логин и/или пароль')]")) ||
                               IsElementPresent(By.XPath("//*[contains(@class, 'input-e')]"));
                    } catch {
                        return false;
                    }
                });
                
                // Делаем скриншот для наглядности результата
                SeleniumUtils.TakeScreenshot(driver, "LoginError");
                Thread.Sleep(stepDelay);
                
                Assert.IsTrue(hasError, "Сообщение об ошибке аутентификации не появилось!");
            }
            catch (Exception e)
            {
                // В случае ошибки делаем скриншот для диагностики
                SeleniumUtils.TakeScreenshot(driver, "TestError");
                verificationErrors.Append(e.Message);
                throw;
            }
        }
        
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }
        
        private string CloseAlertAndGetItsText() {
            try {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert) {
                    alert.Accept();
                } else {
                    alert.Dismiss();
                }
                return alertText;
            } finally {
                acceptNextAlert = true;
            }
        }
    }
}
