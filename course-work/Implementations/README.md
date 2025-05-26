# Видео библиотека за индексиране на файлове в BLOB Storage с ASP.NET API и Blazor Server Webapp
### Факултетен номер: 2301321010

## Описание
- Автентикация чрез Microsoft EntraID
- Извличане на ключове за API ауторизация
- CRUD операции за създаване на видеа, жанрове, автори и др
- Филтрация, странициране и подреждане
- Възможност за влизане и излизане от приложението чрез Microsoft акаунти

## Необходими стъпки за работа с приложението

- Заване на Connection string към базата, която ще се използва в Common\Repositories\AppDbContext.cs
- Регистрация на API и App приложения в Azure portal
- Промяна на Authority и Audience полетата за JWT Token в VideoLibraryAPI\Program.cs
- Промяна на AzureAd и DownstreamApi полетата w VideoLibraryBlazorFrontend\appsettings.json
- Промяна на _scopes в VideoLibraryBlazorFrontend\ApiAuthorizationHandler.cs

