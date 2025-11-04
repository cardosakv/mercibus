export const PRODUCTS_API = {
  LIST: '/api/products',
  ADD: '/api/products',
  GET: (id: number) => `/api/products/${id}`,
  UPDATE: (id: number) => `/api/products/${id}`,
  DELETE: (id: number) => `/api/products/${id}`,
  ADD_IMAGE: (id: number) => `/api/products/${id}/images`,
  DELETE_IMAGE: (id: number, imageId: number) => `/api/products/${id}/images/${imageId}`,
  LIST_REVIEWS: (id: number) => `/api/products/${id}/reviews`,
  ADD_REVIEW: (id: number) => `/api/products/${id}/reviews`,
  GET_REVIEW: (id: number, reviewId: number) => `/api/products/${id}/reviews/${reviewId}`,
  UPDATE_REVIEW: (id: number, reviewId: number) => `/api/products/${id}/reviews/${reviewId}`,
  DELETE_REVIEW: (id: number, reviewId: number) => `/api/products/${id}/reviews/${reviewId}`,
};
