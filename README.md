# Mercibus

This is a **practice project** for building an e-commerce backend using a **microservices architecture**. Each service
is responsible for a distinct domain and communicates via asynchronous messaging and/or HTTP where appropriate.

---

## Overview

### 🔐 Auth Service

[![Build](https://github.com/cardosakv/mercibus/actions/workflows/auth-service-build.yml/badge.svg)](https://github.com/cardosakv/mercibus/actions/workflows/auth-service-build.yml)
[![Auth Service Test](https://github.com/cardosakv/mercibus/actions/workflows/auth-service.test.yml/badge.svg)](https://github.com/cardosakv/mercibus/actions/workflows/auth-service.test.yml)

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
