# Animatch

Animatch е ASP.NET Core MVC платформа за обяви за животни, доброволчески събития и заявки между потребители. Проектът е разработен за учебен курс по Web приложение с реална архитектура, роли, валидации, API и тестове.

## Концепция

Платформата решава два проблема:

1. Хората да публикуват и намират обяви за животни по град и категория.
2. Организации и администратори да публикуват доброволчески събития и да координират участие.

## Технологии

- ASP.NET Core MVC (.NET 8)
- Razor Views + Areas + Partials + Sections
- Entity Framework Core (Code First)
- Microsoft SQL Server
- ASP.NET Core Identity (автентикация и роли)
- Bootstrap + custom CSS
- JavaScript (AJAX fetch за динамично зареждане)
- NUnit + Coverlet за unit тестове

## Архитектура

Проектът е организиран в отделни слоеве:

- `Controllers` - UI и API endpoints
- `Services` - бизнес логика (`AnimalService`, `CategoryService`, `EventService`, `WalkRequestService`, `UnitOfWork`)
- `Data` - `DbContext`, EF конфигурации, seeding и миграции
- `Models` - entity модели
- `ViewModels` - модели за конкретни екрани
- `Views` + `Areas` - Razor UI (вкл. Admin area)

Текущият контролен поток е:

`Controller -> Service -> DbContext/EF Core -> SQL Server`

## Entity модели

Проектът използва минимум 5 entity модела:

1. `Animal`
2. `Category`
3. `Event`
4. `WalkRequest`
5. `OrganizationProfile`

## Роли и права

Identity роли:

- `User` - създава обяви за животни, разглежда събития, карта и профил
- `Organizer` - всичко от `User` + създава събития
- `Administrator` - пълен достъп: управление на обяви, събития и заявки

## Основни функционалности

- Обяви за животни: CRUD, детайли, филтри и странициране
- Категории: CRUD
- Доброволчески събития (Admin Area): списък, детайли, създаване, редакция, изтриване
- Заявки за разходка/помощ (`WalkRequest`): създаване, преглед, одобрение/отхвърляне
- Профил страница с роля и потребителско съдържание
- Карта с маркери за животни и събития
  - OpenStreetMap по подразбиране
  - опционален Mapbox режим със switch. За mapbox картата е необходим mapbox api ключ, затова добавих възможността картата да се показва със стил от OpenStreetMap, ако няма mapbox api ключ. За да се покаже картата с mapbox стил, трябва да се добави mapbox api ключ в appsettings.Development.json или .env файл.
- Read-only API:
  - `GET /api/animals`
  - `GET /api/events`
- AJAX зареждане на събития в списъка

## Валидации и обработка на грешки

- DataAnnotations на моделите
- Client-side validation чрез `_ValidationScriptsPartial`
- Server-side validation във form actions
- Глобална anti-forgery защита (`AutoValidateAntiforgeryTokenAttribute`)
- Custom error views:
  - `404` - `Views/Shared/404.cshtml`
  - `500` - `Views/Shared/500.cshtml`
- `UseStatusCodePagesWithReExecute` + `UseExceptionHandler`

## Сигурност

- ASP.NET Identity за автентикация и авторизация
- Role-based достъп чрез `[Authorize]` и `[Authorize(Roles = "...")]`
- Защита срещу CSRF чрез anti-forgery tokens
- Използване на EF Core LINQ заявки (без raw SQL за вход от потребител)
- Razor encoding по подразбиране за защита срещу XSS

## Seeding

Seeding е реализиран чрез:

- EF конфигурации (`Data/Configuration`)
  - `CategoryEntityConfiguration`
  - `AnimalEntityConfiguration`
  - `WalkRequestEntityConfiguration`
  - `OrganizationProfileEntityConfiguration`
- Startup seeding (`Data/SeedData.cs`) за роли:
  - `User`
  - `Organizer`
  - `Administrator`

## Тестове

Unit тестовете са в отделен проект `UnitTestAnimatch` и покриват service слоя:

- `AnimalServiceTests`
- `CategoryServiceTests`
- `EventServiceTests`
- `WalkRequestServiceTests`
- `UnitOfWorkTests`

Последен локален резултат:

- 18/18 passing tests
- Service layer coverage: ~93.69% (по отчет от Coverlet)

## Setup инструкции

1. Изисквания:
   - .NET SDK 8.0+
   - SQL Server (локален или Docker)
2. Клониране:
   - `git clone <repo-url>`
   - `cd Animatch/Animatch`
3. Конфигурация:
   - редактирай `appsettings.Development.json` (`ConnectionToSQLServer`)
   - по избор добави `.env` с `MAPBOX_KEY=<your_key>`
4. Миграции:
   - `dotnet ef database update`
5. Стартиране:
   - `dotnet run`
6. Тестове:
   - `dotnet test`

## Структура на проекта

- `Animatch.csproj` - web приложение
- `UnitTestAnimatch/UnitTestAnimatch.csproj` - unit tests
- `Program.cs` - DI, middleware, routing, identity config
- `Data/AnimalManagerDbContext.cs` - EF контекст

## Допълнителни бележки

- Проектът е responsive и използва Bootstrap компоненти.
- API и AJAX функционалностите могат да се разширят с front-end framework при нужда.
- Има готова основа за cloud deployment и демонстрационно видео.
