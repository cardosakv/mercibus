import axiosInstance from '@/lib/axios';
import type { BrandQuery, BrandRequest, BrandResponse } from './types';
import { BRANDS_API } from './routes';

export const brandService = {
  list: async (query: BrandQuery): Promise<BrandResponse[]> => {
    return axiosInstance.get(BRANDS_API.LIST, { params: query });
  },

  add: async (data: BrandRequest): Promise<void> => {
    return axiosInstance.post(BRANDS_API.ADD, data);
  },

  get: async (id: number): Promise<BrandResponse> => {
    return axiosInstance.get(BRANDS_API.GET(id));
  },

  update: async (id: number, data: BrandRequest): Promise<void> => {
    return axiosInstance.put(BRANDS_API.UPDATE(id), data);
  },

  delete: async (id: number): Promise<void> => {
    return axiosInstance.delete(BRANDS_API.DELETE(id));
  },
};
