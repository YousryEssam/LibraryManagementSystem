# 📚 Library Management System

A production-ready Library Management System built with **ASP.NET Core Web API**, following **Clean Architecture** principles and modern backend development practices.

> This project was developed as part of the CODE81 technical assessment.

---

# Features

- Authentication using JWT
- ASP.NET Identity for user management
- Role-Based Authorization
    - Administrator
    - Librarian
    - Staff
- Complete CRUD APIs
    - Books
    - Authors
    - Members
    - Publishers
    - Categories
- Borrow / Return books
- Activity Logging
- Search APIs
- Global Exception Handling
- Validation
- Swagger Documentation
- SQL Server + Entity Framework Core

---

# Architecture

The solution follows **Clean Architecture** to separate business logic from infrastructure concerns.

```
LibraryManagementSystem
│
├── Library.API
│
├── Library.Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   └── Responses
│
├── Library.Domain
│   ├── Entities
│   ├── Enums
│   └── Constants
│
└── Library.Infrastructure
    ├── Persistence
    ├── Configurations
    ├── Services
    └── Repositories
```

This structure keeps the application maintainable, testable, and easy to extend.

---

# Design Choices

## 1. Clean Architecture

The project is divided into four layers:

- Domain
- Application
- Infrastructure
- API

This separation keeps business logic independent from database and framework implementations.

---

## 2. ASP.NET Identity

Instead of implementing authentication manually, ASP.NET Identity was used because it provides:

- Secure password hashing
- Lockout protection
- User management
- Role management
- Security best practices

---

## 3. JWT Authentication

JWT tokens are used for stateless authentication.

Benefits:

- Scalable
- Suitable for REST APIs
- Easy integration with frontend applications

---

## 4. Role-Based Authorization

Three roles are implemented:

- Administrator
- Librarian
- Staff

Authorization policies secure endpoints according to business requirements.

---

## 5. Repository Pattern

Repositories abstract database access from business logic.

Benefits:

- Easier testing
- Separation of concerns
- Cleaner service layer

---

## 6. Entity Framework Core

EF Core was selected because it provides:

- Strong LINQ support
- Migrations
- Change Tracking
- Fluent API
- SQL Server integration

---

## 7. Activity Logging

Every important user action (such as login) is stored in the ActivityLogs table.

This improves:

- Auditing
- Troubleshooting
- Security

---

## 8. API Response Wrapper

All endpoints return a consistent response format.

Example:

```json
{
  "success": true,
  "message": "Book created successfully.",
  "data": { }
}
```

This simplifies frontend integration.

---

## 9. Validation

Validation is performed before business logic execution to ensure only valid requests reach the application.

---

## 10. Soft Delete Support

Entities inherit from a common `AuditableEntity`, allowing future implementation of soft delete and auditing without changing entity structures.

---

# Database Design

The database supports:

- Books
- Multiple Authors per Book
- Categories (Hierarchical)
- Publishers
- Members
- Borrowing Transactions
- System Users
- Activity Logs

Relationships include:

- Many-to-Many (Books ↔ Authors)
- One-to-Many (Publisher → Books)
- Self-referencing Categories
- Borrowing History

---

# Security

Implemented security features include:

- Password Hashing
- JWT Authentication
- Role-Based Authorization
- Account Lockout
- User Activity Logging

---

# Technologies

- ASP.NET Core
- .NET 10
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- JWT
- Swagger / OpenAPI

---

# API Modules

- Authentication
- Books
- Authors
- Members
- Categories
- Publishers
- Borrowing Transactions

---

# Running the Project

## Clone

```bash
git clone https://github.com/YousryEssam/LibraryManagementSystem.git
```

## Restore packages

```bash
dotnet restore
```

## Update database

```bash
dotnet ef database update
```

## Run

```bash
dotnet run
```

Swagger will be available at

```
https://localhost:<port>/swagger
```

---

# Default Roles

- Administrator
- Librarian
- Staff

The application seeds these roles during startup.

---

# Future Improvements

- CQRS with MediatR
- Unit Tests
- Integration Tests
- Redis Caching
- Docker Support
- Serilog
- Health Checks
- Rate Limiting
- Refresh Tokens
- Pagination for all endpoints

---

# Deliverables

✔ RESTful API

✔ Clean Architecture

✔ SQL Server Database

✔ Authentication & Authorization

✔ Activity Logging

✔ Swagger Documentation

✔ SQL Migration Scripts

✔ ERD

---

# Author

**Yousry Essam**

ASP.NET Core Backend Developer
