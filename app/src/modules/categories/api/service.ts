import axiosInstance from '@/lib/axios';
import { CATEGORIES_API } from './routes';
import type { CategoryQuery, CategoryRequest, CategoryResponse } from './types';

export const categoryService = {
  list: async (query: CategoryQuery): Promise<CategoryResponse[]> => {
    return axiosInstance.get(CATEGORIES_API.LIST, { params: query });
  },

  add: async (data: CategoryRequest): Promise<void> => {
    return axiosInstance.post(CATEGORIES_API.ADD, data);
  },

  get: async (id: number): Promise<CategoryResponse> => {
    return axiosInstance.get(CATEGORIES_API.GET(id));
  },

  update: async (id: number, data: CategoryRequest): Promise<void> => {
    return axiosInstance.put(CATEGORIES_API.UPDATE(id), data);
  },

  delete: async (id: number): Promise<void> => {
    return axiosInstance.delete(CATEGORIES_API.DELETE(id));
  },
};
