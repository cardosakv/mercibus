export interface Order {
  id: number
  userId: string
  orderNumber: string
  status: OrderStatus
  totalAmount: number
  shippingAddress: Address
  billingAddress: Address
  items: OrderItem[]
  paymentId?: number
  createdAt: string
  updatedAt: string
}

export type OrderStatus =
  | 'Pending'
  | 'Confirmed'
  | 'Processing'
  | 'Shipped'
  | 'Delivered'
  | 'Cancelled'

export interface OrderItem {
  id: number
  orderId: number
  productId: number
  productName: string
  quantity: number
  unitPrice: number
  totalPrice: number
}

export interface Address {
  street: string
  city: string
  state: string
  postalCode: string
  country: string
}

export interface OrderRequest {
  items: OrderItemRequest[]
  shippingAddress: Address
  billingAddress: Address
}

export interface OrderItemRequest {
  productId: number
  quantity: number
}

export interface OrderUpdateRequest {
  status?: OrderStatus
}
