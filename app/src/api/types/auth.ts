export interface LoginRequest {
  username: string
  password: string
}

export interface RegisterRequest {
  username: string
  email: string
  password: string
}

export interface AuthTokenResponse {
  tokenType: string
  accessToken: string
  expiresIn: number
  refreshToken: string
}

export interface RefreshTokenRequest {
  refreshToken: string
}

export interface LogoutRequest {
  refreshToken: string
}

export interface ForgotPasswordRequest {
  email: string
}

export interface ResetPasswordRequest {
  token: string
  newPassword: string
}

export interface ChangePasswordRequest {
  currentPassword: string
  newPassword: string
}

export interface SendConfirmationEmailRequest {
  email: string
}

export interface UserInfo {
  id: string
  username: string
  email: string
  emailConfirmed: boolean
  profilePictureUrl?: string
  firstName?: string
  lastName?: string
  phoneNumber?: string
}

export interface UpdateUserInfoRequest {
  firstName?: string
  lastName?: string
  phoneNumber?: string
}
