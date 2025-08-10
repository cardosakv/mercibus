# Mercibus

**Mercibus** is a **personal practice project** for building an **e-commerce backend** using a **microservices
architecture**.

---

## 🗂 Services Overview

### 🔐 Auth Service
[![Auth Service CI](https://github.com/cardosakv/mercibus/actions/workflows/auth-ci.yml/badge.svg)](https://github.com/cardosakv/mercibus/actions/workflows/auth-ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mercibus_auth&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mercibus_auth)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=mercibus_auth&metric=coverage)](https://sonarcloud.io/summary/new_code?id=mercibus_auth)

Manages:

- User registration & login
- Role-based access control
- JWT authentication & token refresh
- Email confirmation & password resets

---

### 🛍️ Catalog Service

[![Catalog Service CI](https://github.com/cardosakv/mercibus/actions/workflows/catalog-ci.yml/badge.svg)](https://github.com/cardosakv/mercibus/actions/workflows/catalog-ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mercibus_catalog&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mercibus_catalog)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=mercibus_catalog&metric=coverage)](https://sonarcloud.io/summary/new_code?id=mercibus_catalog)

Handles:

- Product, brand & category management
- Product image and attribute definitions
- Product reviews

---

## 📌 Goals of the Project

- Learn and apply **microservice design patterns**.
- Practice **test-driven development** with unit and integration tests.
- Explore **asynchronous communication** using message brokers.
- Integrate **CI/CD pipelines** and **code quality checks**.

---

## 📦 Tech Stack

- **.NET 8** (C#) for service implementation
- **Entity Framework Core** for data access
- **PostgreSQL** as the main database
- **RabbitMQ** for message-based communication
- **Docker & Docker Compose** for containerization
- **xUnit + Moq + Testcontainers** for testing
- **SonarCloud** for static analysis & code coverage
- **GitHub Actions** for CI/CD

---

## 🚀 Getting Started

### 1️⃣ Clone the repository

```bash
git clone https://github.com/cardosakv/mercibus.git
```

### 2️⃣ Navigate to a service directory

Example:

```bash
cd mercibus/services/auth
```

### 3️⃣ Build and run using Docker Compose in root directory

```bash
docker-compose up -d
```

### 4️⃣ Access service APIs

- Auth Service: `http://localhost:<auth_port>/swagger`
- Catalog Service: `http://localhost:<catalog_port>/swagger`