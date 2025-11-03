import { httpClient } from '../client'
import type { Payment, PaymentRequest, ApiResponse } from '../types'

export const paymentsService = {
  async getPaymentById(id: number): Promise<ApiResponse<Payment>> {
    const response = await httpClient.get<ApiResponse<Payment>>(`/payments/${id}`)
    return response.data
  },

  async initiatePayment(data: PaymentRequest): Promise<ApiResponse<Payment>> {
    const response = await httpClient.post<ApiResponse<Payment>>(
      '/payments/initiate',
      data
    )
    return response.data
  },
}
