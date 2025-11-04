import { AUTH_API } from '@/modules/auth/api/routes';
import type { ApiErrorResponse, ApiSuccessResponse } from '@/types/api';
import type { AuthTokenResponse } from '@/modules/auth/api/types';
import { AUTH_TOKEN_KEY, REFRESH_TOKEN_KEY } from '@/utils/constants';
import { setTokens } from '@/utils/token';
import axios, { AxiosError } from 'axios';
import type { AxiosResponse, InternalAxiosRequestConfig } from 'axios';
import createAuthRefreshInterceptor from 'axios-auth-refresh';

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? 'http://localhost:3000/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Request interceptor to add the Authorization header to each request.
 */
axiosInstance.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = localStorage.getItem(AUTH_TOKEN_KEY);

    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error: AxiosError) => {
    return Promise.reject(error);
  }
);

/**
 * Response interceptor to handle API responses and errors.
 */
axiosInstance.interceptors.response.use(
  (response: AxiosResponse) => {
    if (response.data.data) {
      return response.data.data;
    }

    return response;
  },
  (error: AxiosError<ApiErrorResponse>) => {
    return Promise.reject(error.response?.data.error ?? error);
  }
);

/**
 * Auth refresh interceptor to handle token refresh on 401 responses.
 */
createAuthRefreshInterceptor(axiosInstance, async () => {
  const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY) ?? '';

  const response = await axios.post<ApiSuccessResponse<AuthTokenResponse>>(
    `${axiosInstance.defaults.baseURL}${AUTH_API.REFRESH_TOKEN}`,
    { refreshToken }
  );

  const tokens = response.data.data as AuthTokenResponse;
  setTokens(tokens.accessToken, tokens.refreshToken);

  return Promise.resolve();
});

export default axiosInstance;
