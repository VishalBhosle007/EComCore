# EComCore.API

## 📌 Overview
**EComCore.API** is an **ASP.NET Core Web API** project designed as a **portfolio e-commerce backend**.  
It follows **Clean Architecture** principles and demonstrates my skills in **C#, EF Core, AutoMapper, API design, and layered architecture**.  

This project is currently **in progress** — starting with a **Product Management module (CRUD with multiple images)** and will grow into a full **E-Commerce system**.

---

## ✨ Features (Current)

- ✅ **Product Management**
  - Create, Read (single/all), Update, Delete products
  - Upload and manage **multiple images** per product
  - Keep/remove images on update
  - Delete images when product is deleted
- ✅ **Validation & Exception Handling**
  - Proper HTTP status codes
  - Custom `ValidationException` and `NotFoundException`
- ✅ **Clean Architecture**
  - **Domain**: Entities (`Product`, `ProductImage`)
  - **Application**: DTOs, Interfaces, Exceptions, AutoMapper profiles
  - **Infrastructure**: EF Core DbContext, Repositories, File Storage
  - **API**: Controllers, Middleware
- ✅ **AutoMapper** for DTO ↔ Entity mapping
- ✅ **Local File Storage** for product images
- ✅ **Seed Data** for initial testing

---

## 🛠️ Tech Stack

- **Language:** C#
- **Framework:** ASP.NET Core Web API (.NET 9)
- **Database:** SQL Server (Entity Framework Core)
- **Architecture:** Clean Architecture
- **Libraries & Tools:** AutoMapper, ILogger, Middleware, IFormFile
- **Planned:** ASP.NET Core Identity for authentication & authorization

  ---
