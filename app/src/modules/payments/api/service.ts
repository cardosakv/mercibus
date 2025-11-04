import axiosInstance from '@/lib/axios';
import { PAYMENTS_API } from './routes';

export const paymentService = {
  get: async (id: number): Promise<PaymentResponse> => {
    return axiosInstance.get(PAYMENTS_API.GET(id));
  },

  initiate: async (data: PaymentRequest): Promise<void> => {
    return axiosInstance.post(PAYMENTS_API.INITIATE, data);
  },
};
