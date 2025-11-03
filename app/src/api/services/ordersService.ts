import { httpClient } from '../client'
import type {
  Order,
  OrderRequest,
  OrderUpdateRequest,
  ApiResponse,
} from '../types'

export const ordersService = {
  async getOrders(): Promise<ApiResponse<Order[]>> {
    const response = await httpClient.get<ApiResponse<Order[]>>('/orders')
    return response.data
  },

  async getOrderById(id: number): Promise<ApiResponse<Order>> {
    const response = await httpClient.get<ApiResponse<Order>>(`/orders/${id}`)
    return response.data
  },

  async createOrder(data: OrderRequest): Promise<ApiResponse<Order>> {
    const response = await httpClient.post<ApiResponse<Order>>('/orders', data)
    return response.data
  },

  async updateOrder(
    id: number,
    data: OrderUpdateRequest
  ): Promise<ApiResponse<Order>> {
    const response = await httpClient.patch<ApiResponse<Order>>(
      `/orders/${id}`,
      data
    )
    return response.data
  },
}
