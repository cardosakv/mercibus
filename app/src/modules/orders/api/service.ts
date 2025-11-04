import axiosInstance from '@/lib/axios';
import { ORDERS_API } from './routes';
import type { OrderRequest, OrderResponse, OrderUpdateRequest } from './types';

export const orderService = {
  list: async (): Promise<OrderResponse[]> => {
    return axiosInstance.get(ORDERS_API.LIST);
  },

  add: async (data: OrderRequest): Promise<OrderResponse> => {
    return axiosInstance.post(ORDERS_API.ADD, data);
  },

  get: async (id: number): Promise<OrderResponse> => {
    return axiosInstance.get(ORDERS_API.GET(id));
  },

  update: async (id: number, data: OrderUpdateRequest): Promise<OrderResponse> => {
    return axiosInstance.patch(ORDERS_API.UPDATE(id), data);
  },
};
