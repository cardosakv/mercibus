import axiosInstance from '@/lib/axios';
import { AUTH_API } from './routes';
import type {
  AuthTokenResponse,
  LoginRequest,
  RegisterRequest,
  LogoutRequest,
  ForgotPasswordRequest,
  ResetPasswordRequest,
  SendConfirmationEmailRequest,
  UpdateUserInfoRequest,
  GetUserInfoResponse,
  ChangePasswordRequest,
} from './types';

export const authService = {
  register: async (data: RegisterRequest): Promise<AuthTokenResponse> => {
    return axiosInstance.post(AUTH_API.REGISTER, data);
  },

  login: async (data: LoginRequest): Promise<AuthTokenResponse> => {
    return axiosInstance.post(AUTH_API.LOGIN, data);
  },

  logout: async (data: LogoutRequest): Promise<void> => {
    return axiosInstance.post(AUTH_API.LOGOUT, data);
  },

  sendConfirmationEmail: async (data: SendConfirmationEmailRequest): Promise<void> => {
    return axiosInstance.post(AUTH_API.SEND_CONFIRMATION_EMAIL, data);
  },

  forgotPassword: async (data: ForgotPasswordRequest): Promise<void> => {
    return axiosInstance.post(AUTH_API.FORGOT_PASSWORD, data);
  },

  resetPassword: async (data: ResetPasswordRequest): Promise<void> => {
    return axiosInstance.post(AUTH_API.RESET_PASSWORD, data);
  },

  changePassword: async (data: ChangePasswordRequest): Promise<void> => {
    return axiosInstance.post(AUTH_API.CHANGE_PASSWORD, data);
  },

  getUserInfo: async (): Promise<GetUserInfoResponse> => {
    return axiosInstance.get(AUTH_API.INFO);
  },

  updateUserInfo: async (data: UpdateUserInfoRequest): Promise<void> => {
    return axiosInstance.put(AUTH_API.INFO, data);
  },

  uploadProfilePicture: async (image: File): Promise<{ url: string }> => {
    const formData = new FormData();
    formData.append('image', image);

    return axiosInstance.post(AUTH_API.UPLOAD_PROFILE_PICTURE, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
  },
};
