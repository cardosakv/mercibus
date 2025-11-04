export const ORDERS_API = {
  LIST: '/orders',
  ADD: '/orders',
  GET: (id: number) => `/orders/${id}`,
  UPDATE: (id: number) => `/orders/${id}`,
} as const;
