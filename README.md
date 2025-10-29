# ⚙️ MetaBlog Backend (ASP.NET Core Clean Architecture) 🚧 In Progress

MetaBlog is a **modern, scalable, and maintainable blogging platform backend** built with **ASP.NET Core Web API**, following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.  
It powers the MetaBlog full-stack app (with Angular frontend) and focuses on clean code, reusability, and performance.

---

## 🚀 Overview

The MetaBlog backend provides APIs for:
- 🧑‍💻 User management & authentication (JWT-based)
- 📰 Blog post creation, editing, and comments
- 🧩 Role-based authorization & versioned APIs
- ⚙️ High performance with **caching**, **rate limiting**, and **structured logging**
- 🧠 Built with **CQRS**, **MediatR pipelines**, and **FluentValidation**

---

## 🧠 Core Architecture & Design Patterns

| Pattern / Concept | Description |
|--------------------|-------------|
| **Clean Architecture** | Clear separation between API, Application, Domain, and Infrastructure layers |
| **DDD (Domain-Driven Design)** | Domain entities and value objects encapsulate business rules |
| **CQRS (Command Query Responsibility Segregation)** | Splits reads and writes for better scalability and maintainability |
| **MediatR + Pipeline Behaviors** | Handles commands/queries via mediator with pre-/post-processing behaviors |
| **FluentValidation** | Declarative, reusable validation for DTOs |
| **Result Pattern** | Wraps operation outcomes to indicate success or failure without throwing exceptions |
| **Caching (Hybrid)** | Improves performance using in-memory + distributed caching |
| **Rate Limiting** | Protects API from abuse and excessive traffic |
| **Logging (Serilog)** | Structured, contextual logging to file/console for monitoring and debugging |
| **EF Core Interceptors** | Automatically track and log entity changes in the database layer |
| **API Versioning** | Allows multiple versions of API endpoints to coexist gracefully |
| **Dependency Injection** | Built-in DI for cleaner service registration and decoupled modules |

---

## 🛠️ Tech Stack

| Category | Technologies |
|-----------|---------------|
| **Language** | C# |
| **Framework** | ASP.NET Core 8 Web API |
| **ORM** | Entity Framework Core |
| **Architecture** | Clean Architecture, CQRS, DDD |
| **Validation** | FluentValidation |
| **Mediator** | MediatR |
| **Logging** | Serilog |
| **Caching** | Hybrid Cache |
| **Database** | SQL Server |
| **Auth** | JWT Authentication |
| **Tools** | Swagger, Postman, Git |

---

## 📂 Project Structure
```bash
📁 MetaBlog/
│
├─ 📂 src/
│  ├─ 🧩 MetaBlog.Api/ — API Layer (controllers, middlewares, configuration)
│  │  ├─ Connected Services/        — External service references
│  │  ├─ Dependencies/              — NuGet packages and project references
│  │  ├─ Properties/                — Assembly info and launch settings
│  │  ├─ Common/                    — Shared utilities or constants
│  │  ├─ Controllers/               — API endpoints
│  │  ├─ Extensions/                — Contains helper and configuration extensions for the API layer
│  │  ├─ Infrastructure/            — API-specific infrastructure (filters, middlewares)
│  │  ├─ OpenApi/                   — Swagger and API documentation setup
│  │  ├─ appsettings.json           — Application configuration file
│  │  ├─ DependencyInjection.cs     — Central DI setup for the API layer
│  │  ├─ MetaBlog.Api.http          — HTTP test file (for VS or REST Client)
│  │  └─ Program.cs                 — Application entry point
│  │
│  ├─ ⚙️ MetaBlog.Application/ — Application Layer (CQRS, DTOs, Behaviors)
│  │  ├─ Common/                     — Shared utilities, interfaces, and pipeline behaviors
│  │  ├─ Features/                   — Each feature is self-contained (commands, queries, DTOs, interfaces)
│  │  └─ DependencyInjection.cs      — Registers application-layer services
│  │
│  ├─ 🧱 MetaBlog.Domain/ — Domain Layer (core business logic)
│  │  ├─ Common/                     — Shared domain utilities
│  │  └─ [Entities Folders]/         — Each entity has its own folder (with related logic)
│  │
│  └─ 🏗️ MetaBlog.Infrastructure/ — Infrastructure Layer (data, caching, logging)
│     ├─ Common/                    — Shared utilites,Interfaces and extensions
│     ├─ Data/                      — EF Core DbContext and migrations
│     ├─ Identity/                  — Authentication (ASP.NET Identity, JWT)
│     ├─ QueryServices/             — Query services that return DTOs from the database
│     ├─ Repositories/              — CRUD operations on entities
│     ├─ Services/                  — Implemented external or infrastructure services
│     ├─ Settings/                  — App settings (e.g., EmailSettings) used in configuration
│     └─ DependencyInjection.cs     — Registers infrastructure services
│
├─ 🧩 MetaBlog.sln                   — Solution file
├─ ⚙️ .gitignore                     — Git ignore rules
└─ 📝 README.md                      — Project documentation
```


## 🔄 Request Flow

1️⃣ **Controller (API Layer)** — Receives the HTTP request and maps it to a command or query.  
2️⃣ **MediatR (Application Layer)** — Dispatches the request to the corresponding handler.  
3️⃣ **Handler (CQRS)** — Executes the business logic using repositories or services.  
4️⃣ **Domain Layer** — Contains core entities, value objects, and domain rules.  
5️⃣ **Infrastructure Layer** — Handles data persistence, caching, logging, and external APIs.  
6️⃣ **Response** — Returns a structured `Result<T>` or HTTP `ProblemDetails` back to the API.


## 🧩 MediatR Pipeline Behaviors

| Behavior | Responsibility |
|-----------|----------------|
| **ValidationBehavior** | Runs FluentValidation before executing handlers |
| **PerformanceBehavior  | Logs slow requests or performance bottlenecks |
| **ExceptionBehavior** | Catches and logs unhandled exceptions in the request pipeline |
| **CachingBehavior** | Applies caching for queries implementing `ICachedQuery` |



---

## 🚧 Future Enhancements

Planned improvements to make MetaBlog production-ready:

* 🔗 **Frontend Integration:** Connect fully with the Angular app (CORS, Swagger, auth flow).
* 🖼️ **Static File Storage:** Support user uploads (post images, profile pictures) locally and in cloud storage.
* ⚡ **Enhanced Caching:** Use Redis-backed HybridCache with smarter invalidation.
* 🧪 **Testing:** Add unit tests using xUnit.
* ☁️ **Deployment & CI/CD**

---


