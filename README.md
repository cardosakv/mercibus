# Mercibus

**Mercibus** is a **personal practice project** for building an **e-commerce backend** using a **microservices
architecture**.

## Services

### 🔐 Auth Service
[![Auth Service CI](https://github.com/cardosakv/mercibus/actions/workflows/auth-ci.yml/badge.svg)](https://github.com/cardosakv/mercibus/actions/workflows/auth-ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mercibus_auth&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mercibus_auth)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=mercibus_auth&metric=coverage)](https://sonarcloud.io/summary/new_code?id=mercibus_auth)

Manages:

- User registration & login
- Role-based access control
- JWT authentication & token refresh
- Email confirmation & password resets

### 🛍️ Catalog Service

[![Catalog Service CI](https://github.com/cardosakv/mercibus/actions/workflows/catalog-ci.yml/badge.svg)](https://github.com/cardosakv/mercibus/actions/workflows/catalog-ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mercibus_catalog&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mercibus_catalog)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=mercibus_catalog&metric=coverage)](https://sonarcloud.io/summary/new_code?id=mercibus_catalog)

Handles:

- Product, brand & category management
- Product image and attribute definitions
- Product reviews

### Order Service

[![Order Service CI](https://github.com/cardosakv/mercibus/actions/workflows/order-ci.yml/badge.svg)](https://github.com/cardosakv/mercibus/actions/workflows/order-ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mercibus_order&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mercibus_order)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=mercibus_order&metric=coverage)](https://sonarcloud.io/summary/new_code?id=mercibus_order)

Handles:

- Order creation & management
- Order updates via events

### Payment Service

[![Payment Service CI](https://github.com/cardosakv/mercibus/actions/workflows/payment-ci.yml/badge.svg)](https://github.com/cardosakv/mercibus/actions/workflows/payment-ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mercibus_payment&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mercibus_payment)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=mercibus_payment&metric=coverage)](https://sonarcloud.io/summary/new_code?id=mercibus_payment)

Handles:

- Payment processing with Xendit
- Payment status via webhooks

## 📦 Tech Stack

- **.NET 8** (C#) for service implementation
- **Entity Framework Core** for data access
- **PostgreSQL** as the main database
- **RabbitMQ** for message-based communication
- **Docker & Docker Compose** for containerization
- **xUnit + Moq + Testcontainers** for testing
- **SonarCloud** for static analysis & code coverage
- **GitHub Actions** for CI/CD

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/cardosakv/mercibus.git
```

### 2. Navigate to a service directory

Example:

```bash
cd mercibus/services/auth
```

### 3. Build and run using Docker Compose in root directory

```bash
docker-compose up -d
```

### 4. Access service APIs

- Gateway: `http://localhost:9000`
- Auth Service: `http://localhost:9000/swagger-auth/index.html`
- Catalog Service: `http://localhost:9000/swagger-catalog/index.html`
- Order Service: `http://localhost:9000/swagger-order/index.html`
- Payment Service: `http://localhost:9000/swagger-payment/index.html`