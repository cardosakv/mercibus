import { AUTH_API } from '@/modules/auth/api/routes';
import type { ApiError, ApiErrorResponse, ApiSuccessResponse } from '@/types/api';
import type { AuthTokenResponse } from '@/modules/auth/api/types';
import { getAuthToken, getRefreshToken, setTokens } from '@/utils/token';
import axios, { AxiosError } from 'axios';
import type { AxiosResponse, InternalAxiosRequestConfig } from 'axios';
import createAuthRefreshInterceptor from 'axios-auth-refresh';
import { useNavigate } from 'react-router-dom';
import { ROUTE_PATHS } from '@/routes/paths';

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
    const token = getAuthToken();

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
    // Auth refresh interceptor should handle 401
    if (error.response?.status === 401) {
      return Promise.reject(error);
    }

    const payload = error.response?.data?.error;

    const apiError: ApiError = {
      type: payload?.type ?? 'unknown',
      code: payload?.code ?? 'internal',
      params: payload?.params ?? [],
    };

    return Promise.reject(apiError);
  }
);

/**
 * Auth refresh interceptor to handle token refresh on 401 responses.
 */
createAuthRefreshInterceptor(axiosInstance, async () => {
  const refreshToken = getRefreshToken();

  const response = await axios.post<ApiSuccessResponse<AuthTokenResponse>>(
    `${axiosInstance.defaults.baseURL}${AUTH_API.REFRESH_TOKEN}`,
    { refreshToken }
  );

  if (response.status !== 200) {
    const navigate = useNavigate();
    navigate(ROUTE_PATHS.LOGIN);
  }

  const tokens = response.data.data as AuthTokenResponse;
  setTokens(tokens.accessToken, tokens.refreshToken);

  return Promise.resolve();
});

export default axiosInstance;
