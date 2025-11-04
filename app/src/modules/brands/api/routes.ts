export const BRANDS_API = {
  LIST: '/brands',
  ADD: '/brands',
  GET: (id: number) => `/brands/${id}`,
  UPDATE: (id: number) => `/brands/${id}`,
  DELETE: (id: number) => `/brands/${id}`,
} as const;
