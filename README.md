# CRUD - Person Management

A lightweight **.NET 10 MVC web application** that demonstrates basic CRUD operations for managing persons using an in-memory data store.

The solution is organized into multiple projects following a simple layered architecture, including the web application, services, DTOs/contracts, entities, and unit tests.

## 🚀 Features

* ASP.NET Core MVC
* .NET 10
* CRUD operations for persons
* In-memory data storage
* Seeded sample persons and countries
* Server-side model validation
* DataAnnotations validation
* Search and sorting
* Dependency Injection
* Service layer with interfaces
* Unit testing support
* Clean separation between entities, services, contracts, and web layer

## 🏗️ Solution Structure

```text
CRUD/
│
├── CRUD/
│   ├── Controllers/
│   │   └── PersonsController.cs
│   ├── Views/
│   ├── wwwroot/
│   └── Program.cs
│
├── Services/
│   ├── PersonService.cs
│   └── CountryService.cs
│
├── ServiceContracts/
│   ├── DTOs/
│   └── Interfaces/
│
├── Entities/
│   ├── Person.cs
│   └── Country.cs
│
└── CRUDTests/
    └── Unit Tests
```

### Projects

| Project              | Description                                                                       |
| -------------------- | --------------------------------------------------------------------------------- |
| **CRUD**             | ASP.NET Core MVC web application containing controllers, views, and static assets |
| **Services**         | Contains service implementations such as `PersonService` and `CountryService`     |
| **ServiceContracts** | Contains DTOs and service interfaces                                              |
| **Entities**         | Contains domain models such as `Person` and `Country`                             |
| **CRUDTests**        | Contains unit tests for the application                                           |

## 📂 Key Files

### `CRUD/Program.cs`

Application startup and Dependency Injection configuration.

The application registers:

* `IPersonService`
* `ICountryService`

as singleton services.

### `CRUD/Controllers/PersonsController.cs`

Handles person-related HTTP requests, including:

* Listing persons
* Searching
* Sorting
* Creating persons

### `Services/PersonService.cs`

Provides the in-memory implementation for person operations and contains seeded sample data.

### `Services/CountryService.cs`

Provides an in-memory list of countries used by the application.

## 🛠️ Technologies

* **C#**
* **.NET 10**
* **ASP.NET Core MVC**
* **Razor Views**
* **Dependency Injection**
* **DataAnnotations**
* **xUnit** *(if tests are included)*

## 📋 Prerequisites

Before running the project, make sure you have:

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* Visual Studio 2026 or VS Code *(optional)*

## ▶️ Getting Started

### 1. Clone the repository

```bash
git clone <your-repository-url>
cd <your-repository-folder>
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Build the solution

```bash
dotnet build
```

### 4. Run the web application

From the solution root:

```bash
dotnet run --project CRUD
```

### 5. Open the application

By default, you can access:

```text
http://localhost:5000/
```

or:

```text
http://localhost:5000/persons/index
```

The root route `/` maps to the persons index page.

> **Note:** The actual port may differ depending on your ASP.NET Core configuration.

## 👤 Person Management

The application supports the following operations:

| Method | Endpoint          | Description                             |
| ------ | ----------------- | --------------------------------------- |
| `GET`  | `/`               | Display all persons                     |
| `GET`  | `/persons/index`  | Display persons with search and sorting |
| `GET`  | `/persons/create` | Display the create person form          |
| `POST` | `/persons/create` | Create a new person                     |

## 🔍 Search & Sorting

The persons index page supports:

* Searching persons
* Sorting the displayed data
* Displaying seeded persons from the in-memory data store

## ✅ Validation

The application uses **DataAnnotations** for server-side validation.

Examples include:

* `Required`
* `EmailAddress`

The `PersonService` also validates business rules such as **unique email addresses**.

If a duplicate email is detected, the service throws an `ArgumentException`.

## 🌱 Seeded Data

The application includes sample data for:

* Countries
* Persons

This allows the application to be used immediately after launching without requiring a database setup.

Because the application uses an **in-memory data store**, all changes are lost when the application restarts.

## 🧪 Testing

I
