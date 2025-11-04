export const CATEGORIES_API = {
  LIST: '/api/categories',
  ADD: '/api/categories',
  GET: (id: number) => `/api/categories/${id}`,
  UPDATE: (id: number) => `/api/categories/${id}`,
  DELETE: (id: number) => `/api/categories/${id}`,
} as const;
