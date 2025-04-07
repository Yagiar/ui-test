# Selenium Test Project

Этот проект содержит автоматизированные тесты для сайта Одноклассники с использованием Selenium WebDriver.

## Структура проекта

- `Tests/` - директория с тестами
- `SeleniumTests.csproj` - файл проекта .NET

## Требования

- .NET 8.0 или выше
- Google Chrome браузер

## Установка

1. Клонируйте репозиторий:
   ```
   git https://github.com/Yagiar/ui-test
   cd ui-test
   ```

2. Восстановите пакеты NuGet:
   ```
   dotnet restore
   ```

## Запуск тестов

Для запуска всех тестов:
```
dotnet test
```

Для изменения скорости выполнения действий (в миллисекундах):
```
dotnet test -- TestRunParameters.Parameter\(name=\"StepDelayMs\",value=\"3000\"\)
```

## Настройка параметров

Параметры теста можно настроить в файле `.runsettings` или при запуске через командную строку:

- `StepDelayMs` - задержка между действиями в миллисекундах (по умолчанию: 1500)
- `HeadlessMode` - запуск в режиме без графического интерфейса (по умолчанию: false)
- `ImplicitWaitTimeoutSeconds` - время ожидания элементов в секундах (по умолчанию: 10)

## Сценарий теста

Тест выполняет следующие действия:
1. Открывает сайт ok.ru
2. Кликает на кнопку входа
3. Вводит тестовый email
4. Вводит тестовый пароль
5. Нажимает кнопку входа
6. Проверяет наличие сообщения об ошибке аутентификации

## Примечание

Обратите внимание, что этот тест использует фиктивные данные для входа и не пытается выполнить фактическую аутентификацию. 