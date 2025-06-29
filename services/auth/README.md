# Auth API Documentation

## Overview

The **Auth API** provides endpoints for user authentication and account management functionalities, including registration, login, logout, password management, and email confirmation flows.

**Version:** v1

---

## Base URL

All API endpoints are available under the following base URL:

```
https://api.example.com/api/auth
```


## Authentication

This API uses JWT Bearer tokens for authentication. After a successful login or token refresh, include the returned `accessToken` in the `Authorization` header of subsequent requests:

```
Authorization: Bearer <accessToken>
```

## Endpoints

### Register

**Endpoint**: `POST /register`

Create a new user account.

**Request Body** (JSON):

```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "P@ssw0rd!"
}
```

**Responses**:

- `201 Created` – Registration successful.
- `400 Bad Request` – Invalid input data.
- `409 Conflict` – Username or email already exists.

### Login

**Endpoint**: `POST /login`

Authenticate a user and obtain JWT tokens.

**Request Body** (JSON):

```json
{
  "username": "johndoe",
  "password": "P@ssw0rd!"
}
```

**Responses**:

- `200 OK` – Returns `AuthToken` with `accessToken`, `refreshToken`, and `expiresIn`.
- `400 Bad Request` – Invalid credentials format.
- `403 Forbidden` – Incorrect password.
- `404 Not Found` – User not found.

### Logout

**Endpoint**: `POST /logout`

Invalidate a refresh token.

**Request Body** (JSON):

```json
{
  "refreshToken": "<refresh_token>"
}
```

**Responses**:

- `200 OK` – Successful process.
- `401 Unauthorized` – Missing or invalid token.
- `403 Forbidden` – Token already invalidated.

### Refresh Token

**Endpoint**: `POST /refresh-token`

Obtain a new access token using a valid refresh token.

**Request Body** (JSON):

```json
{
  "refreshToken": "<refresh_token>"
}
```

**Responses**:

- `200 OK` – Returns new `AuthToken`.
- `401 Unauthorized` – Invalid refresh token.
- `403 Forbidden` – Refresh token revoked.

### Send Confirmation Email

**Endpoint**: `POST /send-confirmation-email`

Send a confirmation email to verify the user's email address.

**Request Body** (JSON):

```json
{
  "email": "john@example.com"
}
```

**Responses**:

- `200 OK` – Successful process.
- `401 Unauthorized` – User not authenticated.
- `404 Not Found` – Email not associated with any account.
- `409 Conflict` – Email already verified.

### Confirm Email

**Endpoint**: `GET /confirm-email?userId={userId}&token={token}`

Confirm the user's email address. This is the link sent in the confirmation email. Redirects to the specified frontend pages in config.

**Responses**:

- `302 Found` – Successful process and redirects to the specified page.
- `400 Bad Request` – Invalid or expired token.
- `404 Not Found` – User or token not found.

### Forgot Password

**Endpoint**: `POST /forgot-password`

Send a password reset link to the email address.

**Request Body** (JSON):

```json
{
  "email": "john@example.com"
}
```

**Responses**:

- `200 OK` – Successful process.
- `404 Not Found` – Email not registered.

### Reset Password

**Endpoint**: `POST /reset-password`

Reset user password using reset token received from email.

**Request Body** (JSON):

```json
{
  "userId": "<user_id>",
  "token": "<reset_token>",
  "newPassword": "NewP@ssw0rd"
}
```

**Responses**:

- `200 OK` – Successful process.
- `400 Bad Request` – Invalid token or password requirements not met.
- `404 Not Found` – User or token not found.

### Change Password

**Endpoint**: `POST /change-password`

Change password for authenticated user.

**Request Body** (JSON):

```json
{
  "userId": "<user_id>",
  "currentPassword": "OldP@ssw0rd",
  "newPassword": "NewP@ssw0rd"
}
```

**Responses**:

- `200 OK` – Successful process.
- `400 Bad Request` – Missing fields or weak password.
- `401 Unauthorized` – Invalid authentication.
- `404 Not Found` – User not found.

### Get User Info

**Endpoint**: `GET /info`

Retrieve authenticated user's profile information.

**Responses**:

- `200 OK` – Returns `GetUserInfoResponse` with user details.
- `401 Unauthorized` – Missing or invalid token.
- `404 Not Found` – User not found.

### Update User Info

**Endpoint**: `POST /info`

Update authenticated user's profile information.

**Request Body** (JSON):

```json
{
  "name": "John Doe",
  "street": "123 Main St",
  "city": "Metropolis",
  "state": "CA",
  "country": "USA",
  "postalCode": 12345
}
```

**Responses**:

- `200 OK` – Update successful.
- `400 Bad Request` – Validation errors.
- `401 Unauthorized` – Invalid token.
- `404 Not Found` – User not found.