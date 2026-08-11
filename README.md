<div align="center">

# 🛒 Multi-Vendor E-Commerce Platform API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![EF Core](https://img.shields.io/badge/EF_Core-8.0-0078D4?style=for-the-badge&logo=nuget&logoColor=white)](https://docs.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
[![SignalR](https://img.shields.io/badge/SignalR-Real--Time-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/signalr)
[![Architecture](https://img.shields.io/badge/Architecture-Clean_N--Tier-28a745?style=for-the-badge)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![License](https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge)](LICENSE)

<p>
A robust, scalable, and production-ready <strong>Multi-Vendor E-Commerce RESTful API</strong> built with <strong>.NET 8</strong> and <strong>Clean Architecture</strong> principles. Supports the complete lifecycle of online commerce — from product browsing and cart management to secure multi-gateway payments, real-time notifications, seller wallet systems, and smart logistics.
</p>

[Getting Started](#-getting-started) •
[Architecture](#-architecture) •
[API Reference](#-api-reference) •
[Docker](#-docker-deployment) •
[Configuration](#-configuration-guide)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Getting Started](#-getting-started)
- [Configuration Guide](#-configuration-guide)
- [Docker Deployment](#-docker-deployment)
- [API Reference](#-api-reference)
  - [Authentication](#1--authentication--accountcontroller)
  - [Admin](#2--admin-management--admincontroller)
  - [Categories](#3--categories--categorycontroller)
  - [Discounts](#4--discounts--discountcontroller)
  - [Feedback](#5--feedback--feedbackcontroller)
  - [Notifications](#6--notifications--notificationcontroller)
  - [Orders](#7--orders--ordercontroller)
  - [Payments](#8--payments--paymentcontroller)
  - [Photos](#9--photos--photoscontroller)
  - [Products](#10--products--productcontroller)
  - [Product Reviews](#11--product-reviews--productreviewcontroller)
  - [Seller Dashboard](#12--seller-dashboard--sellercontroller)
  - [Shipments](#13--shipments--shipmentcontroller)
  - [Shipping Zones](#14--shipping-zones--shippingzonecontroller)
  - [Shopping Cart](#15--shopping-cart--shoppingcartcontroller)
  - [User Addresses](#16--user-addresses--useraddresscontroller)
  - [Wallet](#17--wallet--walletcontroller)
  - [Withdrawals](#18--withdrawals--withdrawalcontroller)
  - [Wishlist](#19--wishlist--wishlistcontroller)
- [Role-Based Access Matrix](#-role-based-access-matrix)
- [Entity Overview](#-entity-overview)
- [API Examples](#-api-request--response-examples)
- [Security Notes](#️-security-notes)
- [Production Checklist](#-production-checklist)
- [Team](#-team)
- [License](#-license)

---

## 🌟 Overview

This platform is designed to handle the complete lifecycle of multi-vendor online shopping. Whether you're a **customer** browsing and purchasing, a **seller** managing inventory and withdrawals, or an **admin** overseeing the marketplace — every workflow is covered with clean, role-secured API endpoints.

---

## 🚀 Key Features

- 🏪 **Multi-Vendor Marketplace** — Sellers manage their own product catalogs, sales analytics, and digital wallets
- 🔐 **JWT + Refresh Token Auth** — Secure, stateless authentication with automatic token rotation
- 📧 **Email Confirmation & OTP** — Full account verification and OTP-based password reset flow
- 🔑 **Google OAuth Login** — Social login via Google for frictionless onboarding
- 👥 **Role-Based Access Control** — Granular permissions for `Admin`, `Seller`, and `Customer` roles
- ✅ **Product Approval Workflow** — Admin must approve products before they go live
- 💳 **Paymob Payment Gateway** — Full integration including webhook callback handling
- 📡 **SignalR Real-Time Notifications** — Live notifications pushed to users via `/notificationHub`
- ☁️ **Cloudinary Image Uploads** — Cloud-managed product and asset images
- 💰 **Seller Wallet & Withdrawals** — Sellers accumulate earnings and request admin-approved withdrawals
- 🚚 **Shipping Zone Calculation** — Geographic zone-based shipping cost automation
- ⭐ **Verified Purchase Reviews** — Only customers who received their order can leave reviews
- 🐳 **Docker Ready** — Full `docker-compose` support for containerized deployment
- 🛡️ **Global Exception Middleware** — Standardized JSON error responses across all endpoints

---

## 🏗️ Architecture

This project strictly follows **Clean Architecture (N-Tier)** principles, ensuring separation of concerns, testability, and long-term maintainability.

```
┌─────────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                           │
│                    (e-commerce.api)                             │
│         Controllers │ Middleware │ Program.cs │ SignalR Hub     │
└──────────────────────────┬──────────────────────────────────────┘
                           │ Depends On
┌──────────────────────────▼──────────────────────────────────────┐
│                    APPLICATION LAYER                            │
│                    (e-commerce.app)                             │
│         Services │ Interfaces │ DTOs │ AutoMapper Profiles      │
└──────────────────────────┬──────────────────────────────────────┘
                           │ Depends On
┌──────────────────────────▼──────────────────────────────────────┐
│                  INFRASTRUCTURE LAYER                           │
│                    (e-commerce.infra)                           │
│         Repositories │ DbContext │ EF Core Migrations           │
└──────────────────────────┬──────────────────────────────────────┘
                           │ Depends On
┌──────────────────────────▼──────────────────────────────────────┐
│                      DOMAIN LAYER                               │
│                    (e-commerce.core)                            │
│                   Entities │ Enums                              │
│               (No external dependencies)                        │
└─────────────────────────────────────────────────────────────────┘
```

### Mermaid Architecture Diagram

```mermaid
graph TB
    Client(["🌐 Client (Browser / Mobile App)"])

    subgraph API ["🖥️ Presentation Layer — e-commerce.api"]
        Controllers["Controllers (19 Modules)"]
        Middleware["Global Exception Middleware"]
        SignalR["SignalR Notification Hub\n/notificationHub"]
        Auth["JWT Bearer Auth"]
    end

    subgraph APP ["⚙️ Application Layer — e-commerce.app"]
        Services["Business Services"]
        Interfaces["Service Interfaces"]
        DTOs["DTOs & ViewModels"]
        Mapper["AutoMapper Profiles"]
    end

    subgraph INFRA ["🗄️ Infrastructure Layer — e-commerce.infra"]
        Repos["Generic & Specific Repositories"]
        DbCtx["AppDbContext (EF Core)"]
        Identity["ASP.NET Core Identity"]
    end

    subgraph CORE ["🧱 Domain Layer — e-commerce.core"]
        Entities["Entities"]
        Enums["Enums (OrderStatus, etc.)"]
    end

    subgraph EXTERNAL ["☁️ External Services"]
        SQL[("SQL Server")]
        Cloudinary["Cloudinary\nImage Storage"]
        Paymob["Paymob\nPayment Gateway"]
        Email["SMTP\nEmail Service"]
        Google["Google\nOAuth 2.0"]
    end

    Client -->|"HTTPS Requests"| Controllers
    Client <-->|"WebSocket"| SignalR
    Controllers --> Middleware
    Controllers --> Services
    Services --> Interfaces
    Services --> Repos
    Services --> Mapper
    Repos --> DbCtx
    DbCtx --> SQL
    Services --> Cloudinary
    Services --> Paymob
    Services --> Email
    Auth --> Google
    DbCtx --> Identity
    INFRA --> CORE
    APP --> CORE
    API --> APP
```

---

## 💻 Tech Stack

| Category | Technology | Purpose |
|---|---|---|
| **Framework** | .NET 8 / C# 12 | Core runtime & language |
| **Architecture** | Clean Architecture (N-Tier) | Separation of concerns |
| **ORM** | Entity Framework Core 8 | Database access & migrations |
| **Database** | SQL Server | Relational data store |
| **Identity** | ASP.NET Core Identity | User management & roles |
| **Auth** | JWT Bearer + Refresh Tokens | Stateless authentication |
| **OAuth** | Google OAuth 2.0 | Social login |
| **Real-Time** | SignalR | Live notifications |
| **Mapping** | AutoMapper | DTO ↔ Entity mapping |
| **Images** | Cloudinary SDK | Cloud image upload & management |
| **Payments** | Paymob API | Payment processing & webhooks |
| **Email** | SMTP (MailKit / SmtpClient) | Transactional emails & OTP |
| **API Docs** | Swagger / OpenAPI | Interactive API documentation |
| **Containers** | Docker + Docker Compose | Containerized deployment |
| **Middleware** | Custom ExceptionHandlingMiddleware | Global error handling |

---

## ⚡ Getting Started

### Prerequisites

Ensure the following are installed on your machine:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express / LocalDB)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) *(optional, for containerized setup)*
- [Git](https://git-scm.com/)

### 1. Clone the Repository

```bash
git clone https://github.com/the-Mahmoudzzz/E-commerce.git
cd E-commerce
```

### 2. Configure the Application

Copy the example settings and populate your values:

```bash
cp e-commerce.api/appsettings.example.json e-commerce.api/appsettings.json
```

Edit `appsettings.json` — see the full [Configuration Guide](#-configuration-guide) below.

### 3. Apply Database Migrations

```bash
dotnet ef database update \
  --project e-commerce.infra \
  --startup-project e-commerce.api
```

> **First time setup?** If no migrations exist yet, create the initial migration first:
> ```bash
> dotnet ef migrations add InitialCreate \
>   --project e-commerce.infra \
>   --startup-project e-commerce.api
> ```

### 4. Run the Application

```bash
cd e-commerce.api
dotnet run
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`
- **SignalR Hub**: `https://localhost:5001/notificationHub`

---

## ⚙️ Configuration Guide

Populate your `appsettings.json` with the following sections:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },

  "JwtSettings": {
    "Key": "YOUR_SUPER_SECRET_JWT_KEY_MIN_32_CHARS",
    "Issuer": "ECommerceAPI",
    "Audience": "ECommerceClient",
    "AccessTokenExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  },

  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "E-Commerce Platform",
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "YOUR_APP_PASSWORD"
  },

  "CloudinarySettings": {
    "CloudName": "YOUR_CLOUD_NAME",
    "ApiKey": "YOUR_API_KEY",
    "ApiSecret": "YOUR_API_SECRET"
  },

  "PaymobSettings": {
    "ApiKey": "YOUR_PAYMOB_API_KEY",
    "IntegrationId": "YOUR_INTEGRATION_ID",
    "IframeId": "YOUR_IFRAME_ID",
    "HmacSecret": "YOUR_HMAC_SECRET"
  },

  "GoogleAuthSettings": {
    "ClientId": "YOUR_GOOGLE_CLIENT_ID",
    "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
  }
}
```

> ⚠️ **Security Note:** Never commit `appsettings.json` with real secrets to version control. Use **environment variables**, **Azure Key Vault**, or **Docker secrets** in production. Add `appsettings.json` to your `.gitignore` and use `appsettings.example.json` as a template.

---

## 🐳 Docker Deployment

### Using Docker Compose (Recommended)

The project includes a `docker-compose.yml` for a full-stack containerized setup:

```bash
# Build and start all services (API + SQL Server)
docker-compose up --build

# Run in detached mode
docker-compose up -d --build

# Stop all services
docker-compose down

# Stop and remove volumes (resets database)
docker-compose down -v
```

### Sample `docker-compose.yml`

```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "YourStrong!Passw0rd"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

  api:
    build:
      context: .
      dockerfile: e-commerce.api/Dockerfile
    ports:
      - "8080:80"
      - "8443:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=ECommerceDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True
      - JwtSettings__Key=YOUR_SUPER_SECRET_JWT_KEY_MIN_32_CHARS
      - JwtSettings__Issuer=ECommerceAPI
      - JwtSettings__Audience=ECommerceClient
      # Add other environment variables here
    depends_on:
      - sqlserver

volumes:
  sqldata:
```

### Applying Migrations in Docker

```bash
# Run migrations against the containerized database
docker-compose exec api dotnet ef database update \
  --project e-commerce.infra \
  --startup-project e-commerce.api
```

---

## 📚 API Reference

> **Base URL:** `https://localhost:5001`
> 
> 🔒 = Requires JWT Bearer Token in `Authorization: Bearer <token>` header  
> 🔑 = Admin only | 🏪 = Seller only | 👤 = Customer/User only

---

### 1. 🔐 Authentication — `AccountController`

**Base Route:** `api/account`

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `api/account/register` | Public | Register a new user account; triggers email confirmation |
| `POST` | `api/account/login` | Public | Authenticate and receive JWT + refresh token |
| `POST` | `api/account/logout` | 🔒 | Invalidate the current refresh token |
| `POST` | `api/account/refresh` | Public | Exchange refresh token for a new access token |
| `POST` | `api/account/forgot-password` | Public | Send OTP code to registered email |
| `POST` | `api/account/reset-password` | Public | Reset password using OTP from email |
| `POST` | `api/account/google-login` | Public | Authenticate via Google OAuth 2.0 |
| `GET`  | `api/account/confirm-email` | Public | Verify email address via confirmation link |
| `DELETE` | `api/account/delete-account` | 🔒 | Permanently delete the authenticated user's account |

</details>

---

### 2. 🛡️ Admin Management — `AdminController`

**Base Route:** `api/admin` | **Auth:** 🔑 Admin Role Required

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/admin/pending-sellers` | 🔑 | List all sellers awaiting platform approval |
| `POST` | `api/admin/approve-seller/{userId}` | 🔑 | Approve a seller's registration request |
| `GET` | `api/admin/dashboard/stats` | 🔑 | Retrieve platform-wide statistics and analytics |

</details>

---

### 3. 📂 Categories — `CategoryController`

**Base Route:** `api/category`

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/category` | Public | List all top-level categories |
| `GET` | `api/category/sub` | Public | List all sub-categories |
| `GET` | `api/category/type?id={id}` | Public | Get a specific category by ID |
| `GET` | `api/category/typesub?id={id}` | Public | Get a specific sub-category by ID |
| `POST` | `api/category` | 🔑 | Create a new top-level category |
| `POST` | `api/category/sub` | 🔑 | Create a new sub-category |
| `PUT` | `api/category` | 🔑 | Update an existing category |
| `PUT` | `api/category/sub` | 🔑 | Update an existing sub-category |
| `DELETE` | `api/category?id={id}` | 🔑 | Delete a category by ID |

</details>

---

### 4. 🏷️ Discounts — `DiscountController`

**Base Route:** `api/discount`

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/discount` | Public | List all discount/promo codes |
| `GET` | `api/discount/{id}` | Public | Get a specific discount by ID |
| `POST` | `api/discount/apply` | 🔒 | Apply a discount code to a cart or order |
| `POST` | `api/discount` | 🔒 Seller/Admin | Create a new discount code |
| `PUT` | `api/discount` | 🔒 Seller/Admin | Update an existing discount |
| `DELETE` | `api/discount/{id}` | 🔒 Admin/Seller | Delete a discount code |

</details>

---

### 5. 💬 Feedback — `FeedbackController`

**Base Route:** `api/feedback`

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `api/feedback` | 👤 Customer | Submit platform feedback |
| `GET` | `api/feedback` | 🔑 | View all submitted feedback |
| `GET` | `api/feedback/type/{type}` | 🔑 | Filter feedback by category/type |

</details>

---

### 6. 🔔 Notifications — `NotificationController`

**Base Route:** `api/notification`  
**Real-Time Hub:** `ws://host/notificationHub` (SignalR)

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/notification` | 🔑 | Get all platform notifications |
| `GET` | `api/notification/{id}` | 🔑 | Get a specific notification by ID |
| `GET` | `api/notification/my-notifications` | 🔒 | Get the current user's notifications |
| `PUT` | `api/notification/{id}/read` | 🔒 | Mark a specific notification as read |
| `PUT` | `api/notification/read-all` | 🔒 | Mark all current user notifications as read |
| `DELETE` | `api/notification/{id}` | 🔑 | Delete a notification |

</details>

---

### 7. 📦 Orders — `OrderController`

**Base Route:** `api/order`

> **Order Status Flow:** `Pending` → `Processing` → `Shipped` → *(Delivered)* | `Canceled` | `Refused`

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `api/order` | 👤 Customer | Place a new order from current cart |
| `GET` | `api/order/customer` | 👤 Customer | Get all orders for the authenticated customer |
| `GET` | `api/order/{orderId}` | 🔑 | Get full order details by ID |
| `GET` | `api/order/seller` | 🏪 Seller | Get all incoming orders for the authenticated seller |
| `PUT` | `api/order/cancedlorder?orderid={id}` | 👤 Customer | Cancel a pending order |

</details>

---

### 8. 💳 Payments — `PaymentController`

**Base Route:** `api/payment`

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `api/payment` | 🔒 | Initiate a new Paymob payment session for an order |
| `POST` | `api/payment/callback` | Public | Paymob webhook callback — validates HMAC signature and updates order status |

</details>

---

### 9. 📸 Photos — `PhotosController`

**Base Route:** `api/photos` | **Auth:** 🔒 Admin / Seller

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `api/photos/upload` | 🔒 Admin/Seller | Upload an image to Cloudinary; returns public URL |
| `DELETE` | `api/photos/delete/{publicId}` | 🔒 Admin/Seller | Delete an image from Cloudinary by public ID |

</details>

---

### 10. 🛍️ Products — `ProductController`

**Base Route:** `api/product`

> **Product Approval Flow:** Seller creates product (`IsApproved: false`) → Admin reviews → Admin approves (`IsApproved: true`) → Product goes live

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/product/{id}` | Public | Get product details by ID |
| `GET` | `api/product` | Public | List all approved, active products |
| `GET` | `api/product/seller/{sellerId}` | Public | Get all products from a specific seller |
| `POST` | `api/product` | 🏪 Seller | Create a new product (pending approval) |
| `PUT` | `api/product/{id}` | 🏪 Seller | Update an existing product |
| `PUT` | `api/product/approve/{id}` | 🔑 | Approve a product for public listing |
| `DELETE` | `api/product/{id}` | 🔒 Admin/Seller | Delete a product |
| `PUT` | `api/product/{id}/stock` | 🔒 Admin/Seller | Update product stock quantity |
| `GET` | `api/product/{id}/stock` | 🔒 Admin/Seller | Check current stock level for a product |
| `GET` | `api/product/low-stock?threshold=5` | 🔒 Admin/Seller | List all products at or below a stock threshold |

</details>

---

### 11. ⭐ Product Reviews — `ProductReviewController`

**Base Route:** `api/productreview`

> Reviews are restricted to **verified purchasers** (customers who completed and received an order containing that product).

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `api/productreview` | 🔒 | Submit a review for a purchased product |
| `DELETE` | `api/productreview/{id}` | 🔒 | Delete a review (own review or admin) |
| `GET` | `api/productreview/{id}` | Public | Get a specific review by ID |
| `GET` | `api/productreview/product/{productId}?onlyApproved=true` | Public | Get all reviews for a product; filter by approval status |
| `PUT` | `api/productreview/{reviewId}` | 🔒 | Update an existing review |

</details>

---

### 12. 📊 Seller Dashboard — `SellerController`

**Base Route:** `api/seller` | **Auth:** 🏪 Seller Role Required

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/seller/dashboard` | 🏪 Seller | Get seller KPIs: total sales, orders, revenue, top products |
| `GET` | `api/seller/earnings` | 🏪 Seller | Get detailed earnings breakdown and transaction history |

</details>

---

### 13. 🚚 Shipments — `ShipmentController`

**Base Route:** `api/shipment`

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/shipment/{id}` | 🔒 | Get shipment details and current tracking status |
| `POST` | `api/shipment` | 🔒 | Create a new shipment record for an order |
| `PUT` | `api/shipment/{id}/status` | 🔒 | Update the shipment's tracking status |

</details>

---

### 14. 🗺️ Shipping Zones — `ShippingZoneController`

**Base Route:** `api/shippingzone`

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/shippingzone` | 🔒 | List all defined geographic shipping zones and rates |
| `GET` | `api/shippingzone/type/{id}` | 🔒 | Get a specific shipping zone by ID |
| `POST` | `api/shippingzone` | 🔑 | Create a new shipping zone with pricing rules |
| `PUT` | `api/shippingzone?id={id}` | 🔒 | Update an existing shipping zone |
| `DELETE` | `api/shippingzone?id={id}` | 🔒 | Delete a shipping zone |

</details>

---

### 15. 🛒 Shopping Cart — `ShoppingCartController`

**Base Route:** `api/shoppingcart` | **Auth:** 👤 Customer Role Required

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/shoppingcart/basket` | 👤 Customer | Get the current user's cart with calculated totals |
| `POST` | `api/shoppingcart/add-item?productId={id}&quantity={n}` | 👤 Customer | Add a product to the cart |
| `PUT` | `api/shoppingcart` | 👤 Customer | Update item quantities in the cart |
| `DELETE` | `api/shoppingcart` | 👤 Customer | Clear all items from the cart |

</details>

---

### 16. 📍 User Addresses — `UserAddressController`

**Base Route:** `api/useraddress` | **Auth:** 🔒 Required

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/useraddress` | 🔒 | Get all addresses for the current user |
| `GET` | `api/useraddress/{id}` | 🔒 | Get a specific address by ID |
| `POST` | `api/useraddress` | 🔒 | Create a new delivery address |
| `PUT` | `api/useraddress/{id}` | 🔒 | Update an existing address |
| `DELETE` | `api/useraddress/{id}` | 🔒 | Delete an address |
| `PATCH` | `api/useraddress/{id}/set-default` | 🔒 | Set address as the default delivery address |

</details>

---

### 17. 💰 Wallet — `WalletController`

**Base Route:** `api/wallet` | **Auth:** 🏪 Seller Role Required

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/wallet/{sellerId}` | 🏪 Seller | Get current wallet balance and transaction summary |
| `POST` | `api/wallet/create/{sellerId}` | 🏪 Seller | Create a wallet for the seller if one does not exist |

</details>

---

### 18. 🏧 Withdrawals — `WithdrawalController`

**Base Route:** `api/withdrawal`

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `api/withdrawal` | 🏪 Seller | Request a withdrawal from wallet balance |
| `POST` | `api/withdrawal/{id}/approve` | 🔑 | Admin approves and processes a withdrawal request |

</details>

---

### 19. ❤️ Wishlist — `WishlistController`

**Base Route:** `api/wishlist` | **Auth:** 🔒 Required

<details>
<summary><strong>View Endpoints</strong></summary>

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `GET` | `api/wishlist` | 🔒 | Get all items in the current user's wishlist |
| `POST` | `api/wishlist` | 🔒 | Add a product to the wishlist |
| `DELETE` | `api/wishlist?productId={id}` | 🔒 | Remove a product from the wishlist |

</details>

---

## 🔑 Role-Based Access Matrix

| Controller / Feature | 👤 Customer | 🏪 Seller | 🛡️ Admin |
|---|:---:|:---:|:---:|
| Account (register, login) | ✅ | ✅ | ✅ |
| Browse Products | ✅ | ✅ | ✅ |
| Manage Own Products | ❌ | ✅ | ❌ |
| Approve Products | ❌ | ❌ | ✅ |
| Shopping Cart | ✅ | ❌ | ❌ |
| Place Orders | ✅ | ❌ | ❌ |
| View Own Orders | ✅ | ✅ | ✅ |
| Cancel Orders | ✅ | ❌ | ❌ |
| Submit Reviews | ✅ (verified) | ❌ | ❌ |
| Wishlist | ✅ | ❌ | ❌ |
| User Addresses | ✅ | ❌ | ❌ |
| Upload Photos | ❌ | ✅ | ✅ |
| Seller Dashboard | ❌ | ✅ | ❌ |
| Seller Wallet | ❌ | ✅ | ❌ |
| Request Withdrawal | ❌ | ✅ | ❌ |
| Approve Withdrawal | ❌ | ❌ | ✅ |
| Manage Categories | ❌ | ❌ | ✅ |
| Manage Discounts | ❌ | ✅ | ✅ |
| Manage Shipping Zones | ❌ | ❌ | ✅ |
| Platform Analytics | ❌ | ❌ | ✅ |
| Pending Seller Approval | ❌ | ❌ | ✅ |
| View All Feedback | ❌ | ❌ | ✅ |
| Manage Notifications | ❌ | ❌ | ✅ |
| My Notifications | ✅ | ✅ | ✅ |

---

## 🧩 Entity Overview

```
User ──────────────────┬─── Orders ──────────── OrderDetails ─── Product
  │                    │        │                                    │
  │                    │        └── Payment (Paymob)                 │
  │                    │        └── Shipment ─── ShippingZone        │
  ├── ShoppingCart      │                                             │
  │      └── CartItems ─── Product ─── Category / SubCategory        │
  │                                          │                       │
  ├── Wishlist ─────────────────── Product   │                       │
  │                                          │                       │
  ├── UserAddress                  ProductReview (verified only)     │
  │                                                                  │
  ├── Notifications (SignalR)      Seller ───── SellerWallet         │
  │                                    │             └── Withdrawals │
  └── Feedback                         └────────── Products ─────────┘
                                                        └── Discount
```

### Key Entities

| Entity | Key Fields | Notes |
|---|---|---|
| **User** | `Id`, `Email`, `Role`, `IsEmailConfirmed` | Polymorphic: Customer / Seller / Admin |
| **Product** | `Id`, `Price`, `Stock`, `IsApproved`, `IsActive`, `SellerId` | Requires admin approval before listing |
| **Order** | `Id`, `CustomerId`, `Status`, `TotalAmount`, `ShippingCost` | `Status` enum drives lifecycle |
| **OrderDetail** | `OrderId`, `ProductId`, `Quantity`, `UnitPrice` | Snapshot of price at time of purchase |
| **Payment** | `Id`, `OrderId`, `PaymobTransactionId`, `Status` | HMAC-verified Paymob webhook |
| **Shipment** | `Id`, `OrderId`, `TrackingNumber`, `Status`, `ZoneId` | Linked to ShippingZone for cost |
| **ShippingZone** | `Id`, `ZoneName`, `BaseCost`, `PricePerKg` | Geographic cost calculation |
| **SellerWallet** | `SellerId`, `Balance`, `TotalEarned`, `TotalWithdrawn` | Auto-credited on order completion |
| **Withdrawal** | `Id`, `SellerId`, `Amount`, `Status`, `ApprovedAt` | Admin approval flow |
| **ProductReview** | `Id`, `ProductId`, `CustomerId`, `Rating`, `IsVerified` | Gated by verified purchase check |
| **Category** | `Id`, `Name` | Has many SubCategories |
| **Discount** | `Id`, `Code`, `Type`, `Value`, `ExpiryDate`, `UsageLimit` | Applied at cart/checkout |
| **Notification** | `Id`, `UserId`, `Message`, `IsRead`, `CreatedAt` | Delivered via SignalR hub |
| **Wishlist** | `UserId`, `ProductId` | Simple product save for later |
| **Feedback** | `Id`, `UserId`, `Type`, `Message`, `CreatedAt` | Platform-level feedback |

---

## 📝 API Request / Response Examples

### 🔐 Login

**Request:**
```http
POST /api/account/login
Content-Type: application/json

{
  "email": "customer@example.com",
  "password": "SecurePassword123!"
}
```

**Response `200 OK`:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "dGhpcy1pcy1hLXJlZnJlc2gtdG9rZW4...",
    "expiresIn": 3600,
    "user": {
      "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
      "email": "customer@example.com",
      "fullName": "John Doe",
      "role": "Customer"
    }
  }
}
```

**Error Response `401 Unauthorized`:**
```json
{
  "success": false,
  "statusCode": 401,
  "message": "Invalid email or password.",
  "errors": null
}
```

---

### 📦 Create Order

**Request:**
```http
POST /api/order
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "addressId": "addr-uuid-here",
  "shippingZoneId": "zone-uuid-here",
  "discountCode": "SAVE20",
  "notes": "Please leave at the door"
}
```

**Response `201 Created`:**
```json
{
  "success": true,
  "data": {
    "orderId": "ord-uuid-here",
    "status": "Pending",
    "items": [
      {
        "productId": "prod-uuid-here",
        "productName": "Wireless Headphones",
        "quantity": 2,
        "unitPrice": 89.99,
        "subtotal": 179.98
      }
    ],
    "subtotal": 179.98,
    "shippingCost": 12.00,
    "discountAmount": 38.40,
    "totalAmount": 153.58,
    "estimatedDelivery": "2024-12-10T00:00:00Z",
    "createdAt": "2024-12-05T14:32:00Z"
  }
}
```

---

### 🔄 Refresh Token

**Request:**
```http
POST /api/account/refresh
Content-Type: application/json

{
  "refreshToken": "dGhpcy1pcy1hLXJlZnJlc2gtdG9rZW4..."
}
```

**Response `200 OK`:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "bmV3LXJlZnJlc2gtdG9rZW4tZ2VuZXJhdGVk...",
    "expiresIn": 3600
  }
}
```

---

### ❌ Global Error Response Format

All errors follow a consistent structure from `ExceptionHandlingMiddleware`:

```json
{
  "success": false,
  "statusCode": 400,
  "message": "Validation failed.",
  "errors": {
    "email": ["The Email field is required."],
    "password": ["Password must be at least 8 characters."]
  },
  "traceId": "0HN1234ABCD:00000001"
}
```

---

## 🛡️ Security Notes

> ⚠️ **The following security practices are required before any production deployment:**

- **Never** commit `appsettings.json` containing real secrets — use `.gitignore`
- Store all secrets (JWT key, DB password, API keys) in **environment variables** or a secrets manager (Azure Key Vault, AWS Secrets Manager, HashiCorp Vault)
- Rotate your `JwtSettings:Key` to a cryptographically random string of at least **32 characters**
- Set `ASPNETCORE_ENVIRONMENT=Production` in production — this disables Swagger UI by default
- Configure **CORS** to allow only trusted origins
- Enable **HTTPS only** and configure HSTS headers
- Validate Paymob webhook signatures using the `HmacSecret` — already handled in `PaymentController`
- Rate-limit authentication endpoints to prevent brute-force attacks
- Set `RefreshTokenExpiryDays` to the shortest acceptable value for your use case

---

## 🚀 Production Checklist

Before going live, verify the following:

- [ ] All secrets moved to environment variables or secrets manager
- [ ] Database connection string points to production SQL Server
- [ ] EF Core migrations applied to production database
- [ ] `ASPNETCORE_ENVIRONMENT` set to `Production`
- [ ] Swagger UI disabled or protected in production
- [ ] HTTPS configured with a valid TLS certificate
- [ ] CORS configured to allow only your frontend domain(s)
- [ ] JWT key is a strong, random 32+ character secret
- [ ] Paymob HMAC secret configured and webhook URL registered in Paymob dashboard
- [ ] Cloudinary credentials configured and image upload limits reviewed
- [ ] Email SMTP credentials working with transactional email provider
- [ ] Google OAuth redirect URIs configured in Google Cloud Console
- [ ] SignalR hub authenticated — only authorized users connect
- [ ] Logging configured (Application Insights / Serilog / ELK)
- [ ] Health check endpoint configured and monitored
- [ ] Docker image built in `Release` configuration
- [ ] Database backups scheduled
- [ ] Rate limiting enabled on auth and payment endpoints

---

## 👥 Team

This project is built and maintained by a dedicated backend engineering team:

| Name | Role |
|---|---|
| **Mahmoud Mostafa** | Team Lead & Backend Developer |
| **Mahmoud Alaa** | Backend Developer |
| **Mahmoud Diab** | Backend Developer |

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

```
MIT License

Copyright (c) 2024 E-Commerce Multi-Vendor Platform Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
```

---

<div align="center">

Built with ❤️ using **.NET 8** and **Clean Architecture**

⭐ Star this repo if you found it useful!

</div>
