# Mercibus

This is a **practice project** for building an e-commerce backend using a **microservices architecture**. Each service
is responsible for a distinct domain and communicates via asynchronous messaging and/or HTTP where appropriate.

---

## Overview

### 🔐 Auth Service

[![Auth Service CI](https://github.com/cardosakv/mercibus/actions/workflows/auth-ci.yml/badge.svg)](https://github.com/cardosakv/mercibus/actions/workflows/auth-ci.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mercibus_auth&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=mercibus_auth)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=mercibus_auth&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=mercibus_auth)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=mercibus_auth&metric=coverage)](https://sonarcloud.io/summary/new_code?id=mercibus_auth)

Handles user registration, login, roles, and JWT-based authentication.

---

### 🛍️ Product Service

Manages product catalog, categories, and inventory.

---

### 📦 Order Service

Handles order placement, tracking, and order history.

---

### 💳 Payment Service

Integrates with mock payment processing and manages transactions.

---

### 🛒 Cart Service

Manages users' shopping carts and cart items.

---

### 📣 Notification Service

Sends email/SMS notifications (e.g., order confirmation).

---
