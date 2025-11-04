export interface PaymentRequest {
  orderId: number;
}

export interface BillingRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string | null;
  streetLine1: string;
  streetLine2?: string | null;
  city: string;
  state: string;
  postalCode: number;
  country: string;
}

export interface PaymentResponse {
  id: number;
  orderId: number;
  customerId: string;
  amount: number;
  currency: string;
  status: 'AwaitingUserAction' | 'Processing' | 'Completed' | 'Failed';
  updatedAt: string;
}
