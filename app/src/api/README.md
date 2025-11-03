# API Service Layer

This directory contains the HTTP client configuration and API service layer for the Mercibus application.

## Structure

```
api/
├── client.ts              # Axios HTTP client with interceptors
├── types/                 # TypeScript interfaces for API requests/responses
│   ├── common.ts         # Common types (ApiResponse, Pagination)
│   ├── auth.ts           # Auth service types
│   ├── products.ts       # Products/Catalog service types
│   ├── orders.ts         # Orders service types
│   ├── payments.ts       # Payments service types
│   └── index.ts          # Export all types
├── services/             # API service modules
│   ├── authService.ts    # Auth API calls
│   ├── productsService.ts# Products API calls
│   ├── ordersService.ts  # Orders API calls
│   ├── paymentsService.ts# Payments API calls
│   └── index.ts          # Export all services
├── utils/                # Utility functions
│   └── tokenStorage.ts   # JWT token storage utilities
└── index.ts              # Main export file
```

## Features

- ✅ Axios instance properly configured with interceptors
- ✅ JWT tokens automatically injected in requests
- ✅ Automatic token refresh on 401 errors
- ✅ Error responses properly handled
- ✅ TypeScript interfaces for all API request/response types
- ✅ API services for each microservice (auth, products, orders, payments)

## Configuration

Configure the API base URL in your `.env` file:

```env
VITE_API_URL=http://localhost:9000/api
```

## Usage Examples

### Authentication

```typescript
import { authService, tokenStorage } from '@/api'

// Register a new user
const registerUser = async () => {
  try {
    const response = await authService.register({
      username: 'john_doe',
      email: 'john@example.com',
      password: 'SecurePassword123!',
    })
    console.log('User registered:', response.data)
  } catch (error) {
    console.error('Registration failed:', error)
  }
}

// Login
const loginUser = async () => {
  try {
    const response = await authService.login({
      username: 'john_doe',
      password: 'SecurePassword123!',
    })
    // Tokens are automatically stored
    console.log('Login successful:', response.data)
  } catch (error) {
    console.error('Login failed:', error)
  }
}

// Get user info
const getUserInfo = async () => {
  try {
    const response = await authService.getUserInfo()
    console.log('User info:', response.data)
  } catch (error) {
    console.error('Failed to get user info:', error)
  }
}

// Logout
const logoutUser = async () => {
  const refreshToken = tokenStorage.getRefreshToken()
  if (refreshToken) {
    try {
      await authService.logout({ refreshToken })
      // Tokens are automatically cleared
      console.log('Logout successful')
    } catch (error) {
      console.error('Logout failed:', error)
    }
  }
}
```

### Products/Catalog

```typescript
import { productsService } from '@/api'

// Get products with filters
const getProducts = async () => {
  try {
    const response = await productsService.getProducts({
      page: 1,
      pageSize: 20,
      minPrice: 10,
      maxPrice: 100,
      sortBy: 'price',
      sortOrder: 'asc',
    })
    console.log('Products:', response.data)
  } catch (error) {
    console.error('Failed to get products:', error)
  }
}

// Get product by ID
const getProduct = async (id: number) => {
  try {
    const response = await productsService.getProductById(id)
    console.log('Product:', response.data)
  } catch (error) {
    console.error('Failed to get product:', error)
  }
}

// Create a new product
const createProduct = async () => {
  try {
    const response = await productsService.createProduct({
      name: 'New Product',
      description: 'Product description',
      price: 29.99,
      stock: 100,
      brandId: 1,
      categoryId: 2,
    })
    console.log('Product created:', response.data)
  } catch (error) {
    console.error('Failed to create product:', error)
  }
}

// Get brands
const getBrands = async () => {
  try {
    const response = await productsService.getBrands()
    console.log('Brands:', response.data)
  } catch (error) {
    console.error('Failed to get brands:', error)
  }
}

// Add product review
const addReview = async (productId: number) => {
  try {
    const response = await productsService.addProductReview(productId, {
      rating: 5,
      comment: 'Great product!',
    })
    console.log('Review added:', response.data)
  } catch (error) {
    console.error('Failed to add review:', error)
  }
}
```

### Orders

```typescript
import { ordersService } from '@/api'

// Create an order
const createOrder = async () => {
  try {
    const response = await ordersService.createOrder({
      items: [
        { productId: 1, quantity: 2 },
        { productId: 3, quantity: 1 },
      ],
      shippingAddress: {
        street: '123 Main St',
        city: 'New York',
        state: 'NY',
        postalCode: '10001',
        country: 'USA',
      },
      billingAddress: {
        street: '123 Main St',
        city: 'New York',
        state: 'NY',
        postalCode: '10001',
        country: 'USA',
      },
    })
    console.log('Order created:', response.data)
  } catch (error) {
    console.error('Failed to create order:', error)
  }
}

// Get user's orders
const getUserOrders = async () => {
  try {
    const response = await ordersService.getOrders()
    console.log('Orders:', response.data)
  } catch (error) {
    console.error('Failed to get orders:', error)
  }
}

// Get order by ID
const getOrder = async (id: number) => {
  try {
    const response = await ordersService.getOrderById(id)
    console.log('Order:', response.data)
  } catch (error) {
    console.error('Failed to get order:', error)
  }
}

// Update order status
const updateOrder = async (id: number) => {
  try {
    const response = await ordersService.updateOrder(id, {
      status: 'Shipped',
    })
    console.log('Order updated:', response.data)
  } catch (error) {
    console.error('Failed to update order:', error)
  }
}
```

### Payments

```typescript
import { paymentsService } from '@/api'

// Initiate a payment
const initiatePayment = async (orderId: number) => {
  try {
    const response = await paymentsService.initiatePayment({
      orderId,
      amount: 99.99,
      currency: 'USD',
      paymentMethod: 'credit_card',
      returnUrl: 'https://example.com/payment/return',
    })
    console.log('Payment initiated:', response.data)
    // Redirect user to payment URL if provided
    if (response.data?.paymentUrl) {
      window.location.href = response.data.paymentUrl
    }
  } catch (error) {
    console.error('Failed to initiate payment:', error)
  }
}

// Get payment details
const getPayment = async (id: number) => {
  try {
    const response = await paymentsService.getPaymentById(id)
    console.log('Payment:', response.data)
  } catch (error) {
    console.error('Failed to get payment:', error)
  }
}
```

### Direct HTTP Client Usage

For custom API calls not covered by the service layers:

```typescript
import { httpClient } from '@/api'

// Make a custom GET request
const customGet = async () => {
  try {
    const response = await httpClient.get('/custom-endpoint')
    console.log('Response:', response.data)
  } catch (error) {
    console.error('Request failed:', error)
  }
}

// Make a custom POST request
const customPost = async () => {
  try {
    const response = await httpClient.post('/custom-endpoint', {
      data: 'value',
    })
    console.log('Response:', response.data)
  } catch (error) {
    console.error('Request failed:', error)
  }
}
```

## Token Storage

The `tokenStorage` utility provides methods for managing JWT tokens:

```typescript
import { tokenStorage } from '@/api'

// Get access token
const token = tokenStorage.getAccessToken()

// Get refresh token
const refreshToken = tokenStorage.getRefreshToken()

// Check if token is expired
const isExpired = tokenStorage.isTokenExpired()

// Clear all tokens (useful for logout)
tokenStorage.clearTokens()
```

## Interceptors

### Request Interceptor

The request interceptor automatically:
- Injects JWT access token in the `Authorization` header for all requests
- Formats: `Authorization: Bearer <token>`

### Response Interceptor

The response interceptor automatically:
- Handles 401 Unauthorized errors
- Attempts to refresh the access token using the refresh token
- Retries the failed request with the new access token
- Redirects to login page if token refresh fails
- Formats error messages for easier handling

## Error Handling

All service methods throw errors that can be caught and handled:

```typescript
try {
  await authService.login(credentials)
} catch (error) {
  // error.message contains the error message
  // error.status contains the HTTP status code (if available)
  // error.errors contains validation errors (if available)
  console.error('Login failed:', error)
}
```

## Type Safety

All API methods are fully typed with TypeScript interfaces. Import types as needed:

```typescript
import type { 
  LoginRequest, 
  AuthTokenResponse,
  Product,
  Order,
  Payment
} from '@/api'
```
