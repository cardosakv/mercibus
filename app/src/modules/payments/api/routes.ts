export const PAYMENTS_API = {
  GET: (id: number) => `/payments/${id}`,
  INITIATE: '/payments/initiate',
} as const;
