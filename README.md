# 🐱 PokemonReviewApp

A RESTful Web API built with ASP.NET Core (.NET 8), Entity Framework Core, and SQL Server.
This project demonstrates clean backend architecture, including DTOs, Repositories, AutoMapper, and proper database relationships.

## 🚀 Tech Stack

.NET 8

ASP.NET Core Web API

Entity Framework Core

SQL Server

AutoMapper

Swagger / OpenAPI

## ✨ Features

Full CRUD operations

Repository + Interface pattern

DTO-based API design

AutoMapper for clean mapping

1-to-Many & Many-to-Many relationships

Swagger UI for testing

## 🧩 Domain Model

### Entities

Pokemon

Category

Order

Review

Reviewer

### Join Tables

PokemonCategories (Pokemon ↔ Category)

PokemonOrders (Pokemon ↔ Order)

## 🔗 Relationships

Pokemon → Reviews (1-to-Many)

Reviewer → Reviews (1-to-Many)

Pokemon ↔ Category (Many-to-Many)

Pokemon ↔ Order (Many-to-Many)

## 📐 ER Diagram

```text
Pokemon ─────< Review >───── Reviewer
   │
   ├──< PokemonCategory >── Category
   │
   └──< PokemonOrder >──── Order
```




## 🔄 API Endpoints

| Method | Endpoint              | Description            |
| ------ | -----------------     | -----------------      |
| GET    | /api/pokemon          |      Get all Pokémon   |
| GET    | /api/pokemon/{id}     |      Get Pokémon by ID |
| GET    | /api/pokemon{ownerId} | Get Pokémon by OwnerID |
| POST   | /api/pokemon          |      Create Pokémon    |
| PUT    | /api/pokemon/{id}     |      Update Pokémon    |
| DELETE | /api/pokemon/{id}     |      Delete Pokémon    |


## 🔄 AutoMapper

AutoMapper is used to map Entities ↔ DTOs, reducing boilerplate code and keeping controllers clean.


## 🔐 Configuration

Connection strings are stored securely using User Secrets.

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your_SQL_Server_Connection_String"
```

## 🧪 Swagger

Test endpoints via Swagger:

https://localhost:{port}/swagger


## 🛠 Database Setup

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```


## 👨‍💻 Author

PokemonReviewApp — A portfolio-ready backend API built with ASP.NET Core & EF Core.