export interface AuthTokenResponse {
  tokenType: string;
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

export interface GetUserInfoResponse {
  id: string;
  username: string;
  name?: string;
  email?: string;
  isEmailVerified: boolean;
  street?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: number;
  phoneNumber?: string;
  birthDate?: string;
  profileImageUrl?: string;
  lastLoginAt?: string;
  createdAt: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface RefreshRequest {
  refreshToken: string;
}

export interface LogoutRequest {
  refreshToken: string;
}

export interface ChangePasswordRequest {
  userId: string;
  currentPassword: string;
  newPassword: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  userId: string;
  token: string;
  newPassword: string;
}

export interface SendConfirmationEmailRequest {
  email: string;
}

export interface ConfirmEmailQuery {
  userId: string;
  token: string;
}

export interface UpdateUserInfoRequest {
  name?: string;
  street?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: number;
  phoneNumber?: string;
  birthDate?: string;
  profileImageUrl?: string;
}
