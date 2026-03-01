# 🛒 E-Commerce Multi-Vendor Platform API

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![EF Core](https://img.shields.io/badge/Entity_Framework_Core-0078D4?style=flat&logo=dotnet)
![Architecture](https://img.shields.io/badge/Architecture-Clean_Architecture-success)

## 📖 Overview
A robust, scalable, and secure Multi-Vendor E-Commerce RESTful API built using **.NET** and **Clean Architecture** principles. This platform is designed to handle the complete lifecycle of online shopping, from product browsing and shopping carts to secure checkout, multi-vendor wallet management, and automated shipping zone calculations.

## 🚀 Key Features
* **Multi-Vendor System**: Sellers can manage their products, track sales, and request withdrawals via dedicated digital wallets.
* **Robust Authentication & Authorization**: 
  * JWT-based authentication.
  * Role-based access control (Admin, Seller, Customer, Shipping Company).
  * Secure email confirmation and OTP-based password resets.
* **Order & Cart Management**: Active shopping carts, wishlists, and order processing with automated total calculations including shipping.
* **Smart Logistics**: Automated shipping cost calculations based on `ShippingZones`.
* **Verified Reviews**: Customers can only review products they have successfully purchased and received.

## 🏗️ Architecture
This project strictly follows the **Clean Architecture** (N-Tier) pattern to ensure separation of concerns, testability, and maintainability:
1. **Domain Layer (`Web.Core`)**: Contains enterprise logic, Entities, and Enums. (No dependencies).
2. **Application Layer (`Web.App`)**: Contains business logic, Use Cases, Services, Interfaces, and DTOs.
3. **Infrastructure Layer (`Web.Infra`)**: Handles external concerns like Database access (EF Core), Repositories implementation, and DbContext.
4. **Presentation Layer (`Web.API`)**: The entry point, containing Controllers, API Routes, and Dependency Injection setups.

## 💻 Tech Stack
* **Framework**: .NET (C#)
* **Architecture**: Clean Architecture
* **Database**: SQL Server & Entity Framework Core
* **Identity & Security**: ASP.NET Core Identity, JWT (JSON Web Tokens), OTP implementations
* **Documentation**: Swagger / OpenAPI

## 👥 Meet The Team
This project is being developed and maintained by:
* **Mahmoud Mostafa** (Team Lead / Backend Developer)
* **Mahmoud Alaa** (Backend Developer)
* **Mahmoud Diab** (Backend Developer)

## ⚙️ How to Run Locally
1. Clone the repository:
   ```bash
   git clone [https://github.com/the-Mahmoudzzz/E-commerce.git](https://github.com/the-Mahmoudzzz/E-commerce.git)
2 .Navigate to the API project directory.
3. Update the appsettings.json with your SQL Server Connection String and SMTP (Email) credentials. Ensure your JWT keys (Issuar, Audiance, Key) match your configuration.
4. Run EF Core Migrations to set up the database dotnet ef database update --project Web.Infra --startup-project Web.API
   ```bash
dotnet ef database update --project Web.Infra --startup-project Web.API



