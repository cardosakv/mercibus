import { AUTH_TOKEN_KEY, REFRESH_TOKEN_KEY } from './constants';

export const getAuthToken = () => localStorage.getItem(AUTH_TOKEN_KEY);
export const getRefreshToken = () => localStorage.getItem(REFRESH_TOKEN_KEY);

export const setTokens = (authToken: string, refreshToken: string) => {
  localStorage.setItem(AUTH_TOKEN_KEY, authToken);
  localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
};

export const clearTokens = () => {
  localStorage.removeItem(AUTH_TOKEN_KEY);
  localStorage.removeItem(REFRESH_TOKEN_KEY);
};
