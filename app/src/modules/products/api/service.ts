import axiosInstance from '@/lib/axios';
import { PRODUCTS_API } from './routes';
import type {
  ProductQuery,
  ProductRequest,
  ProductResponse,
  ProductImageRequest,
  ProductImageResponse,
  ProductReviewQuery,
  ProductReviewRequest,
  ProductReviewResponse,
} from './types';

export const productService = {
  list: async (query: ProductQuery): Promise<ProductResponse[]> => {
    return axiosInstance.get(PRODUCTS_API.LIST, { params: query });
  },

  add: async (data: ProductRequest): Promise<ProductResponse> => {
    return axiosInstance.post(PRODUCTS_API.ADD, data);
  },

  get: async (id: number): Promise<ProductResponse> => {
    return axiosInstance.get(PRODUCTS_API.GET(id));
  },

  update: async (id: number, data: ProductRequest): Promise<ProductResponse> => {
    return axiosInstance.put(PRODUCTS_API.UPDATE(id), data);
  },

  delete: async (id: number): Promise<void> => {
    return axiosInstance.delete(PRODUCTS_API.DELETE(id));
  },

  addImage: async (productId: number, data: ProductImageRequest): Promise<ProductImageResponse> => {
    const formData = new FormData();
    formData.append('image', data.image);
    if (data.isPrimary !== undefined) {
      formData.append('isPrimary', data.isPrimary.toString());
    }
    if (data.altText) {
      formData.append('altText', data.altText);
    }

    return axiosInstance.post(PRODUCTS_API.ADD_IMAGE(productId), formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
  },

  deleteImage: async (productId: number, imageId: number): Promise<void> => {
    return axiosInstance.delete(PRODUCTS_API.DELETE_IMAGE(productId, imageId));
  },

  listReviews: async (
    productId: number,
    query: ProductReviewQuery
  ): Promise<ProductReviewResponse[]> => {
    return axiosInstance.get(PRODUCTS_API.LIST_REVIEWS(productId), { params: query });
  },

  getReview: async (productId: number, reviewId: number): Promise<ProductReviewResponse> => {
    return axiosInstance.get(PRODUCTS_API.GET_REVIEW(productId, reviewId));
  },

  addReview: async (
    productId: number,
    data: ProductReviewRequest
  ): Promise<ProductReviewResponse> => {
    return axiosInstance.post(PRODUCTS_API.ADD_REVIEW(productId), data);
  },

  updateReview: async (
    productId: number,
    reviewId: number,
    data: ProductReviewRequest
  ): Promise<ProductReviewResponse> => {
    return axiosInstance.put(PRODUCTS_API.UPDATE_REVIEW(productId, reviewId), data);
  },

  deleteReview: async (productId: number, reviewId: number): Promise<void> => {
    return axiosInstance.delete(PRODUCTS_API.DELETE_REVIEW(productId, reviewId));
  },
};
