# TaskManagerApi

Aplikacja REST API do zarządzania projektami i zadaniami wykonana w ASP.NET Core .NET 8.

## Technologie

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server LocalDB
* Microsoft Identity
* JWT Authentication
* Swagger

## Funkcjonalności

* Rejestracja użytkowników
* Logowanie użytkowników
* Uwierzytelnianie JWT
* Zarządzanie projektami (CRUD)
* Zarządzanie zadaniami (CRUD)
* Przypisywanie zadań do użytkowników
* Dostęp do własnych projektów
* Dostęp do projektów, w których użytkownik ma przypisane zadania

## Endpointy

### Auth

* POST /api/Auth/register
* POST /api/Auth/login

### Projects

* GET /api/Projects
* GET /api/Projects/{id}
* POST /api/Projects
* PUT /api/Projects/{id}
* DELETE /api/Projects/{id}

### Tasks

* GET /api/Tasks
* POST /api/Tasks
* PUT /api/Tasks/{id}
* DELETE /api/Tasks/{id}

## Autor
Magda Szpakowska
