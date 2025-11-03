export interface Payment {
  id: number
  orderId: number
  amount: number
  currency: string
  status: PaymentStatus
  paymentMethod: string
  externalPaymentId?: string
  paymentUrl?: string
  createdAt: string
  updatedAt: string
}

export type PaymentStatus =
  | 'Pending'
  | 'Processing'
  | 'Completed'
  | 'Failed'
  | 'Refunded'
  | 'Cancelled'

export interface PaymentRequest {
  orderId: number
  amount: number
  currency: string
  paymentMethod: string
  returnUrl?: string
}

export interface PaymentWebhookRequest {
  externalPaymentId: string
  status: string
  data?: Record<string, unknown>
}
