import { httpClient } from '../client'
import { tokenStorage } from '../utils/tokenStorage'
import type {
  LoginRequest,
  RegisterRequest,
  AuthTokenResponse,
  RefreshTokenRequest,
  LogoutRequest,
  ForgotPasswordRequest,
  ResetPasswordRequest,
  ChangePasswordRequest,
  SendConfirmationEmailRequest,
  UserInfo,
  UpdateUserInfoRequest,
  ApiResponse,
} from '../types'

export const authService = {
  async register(data: RegisterRequest): Promise<ApiResponse<AuthTokenResponse>> {
    const response = await httpClient.post<ApiResponse<AuthTokenResponse>>(
      '/auth/register',
      data
    )
    return response.data
  },

  async login(data: LoginRequest): Promise<ApiResponse<AuthTokenResponse>> {
    const response = await httpClient.post<ApiResponse<AuthTokenResponse>>(
      '/auth/login',
      data
    )
    
    if (response.data.data) {
      const { accessToken, refreshToken, expiresIn } = response.data.data
      tokenStorage.setAccessToken(accessToken)
      tokenStorage.setRefreshToken(refreshToken)
      tokenStorage.setTokenExpiry(expiresIn)
    }
    
    return response.data
  },

  async logout(data: LogoutRequest): Promise<ApiResponse> {
    const response = await httpClient.post<ApiResponse>('/auth/logout', data)
    tokenStorage.clearTokens()
    return response.data
  },

  async refreshToken(data: RefreshTokenRequest): Promise<ApiResponse<AuthTokenResponse>> {
    const response = await httpClient.post<ApiResponse<AuthTokenResponse>>(
      '/auth/refresh-token',
      data
    )
    
    if (response.data.data) {
      const { accessToken, refreshToken, expiresIn } = response.data.data
      tokenStorage.setAccessToken(accessToken)
      tokenStorage.setRefreshToken(refreshToken)
      tokenStorage.setTokenExpiry(expiresIn)
    }
    
    return response.data
  },

  async sendConfirmationEmail(
    data: SendConfirmationEmailRequest
  ): Promise<ApiResponse> {
    const response = await httpClient.post<ApiResponse>(
      '/auth/send-confirmation-email',
      data
    )
    return response.data
  },

  async forgotPassword(data: ForgotPasswordRequest): Promise<ApiResponse> {
    const response = await httpClient.post<ApiResponse>(
      '/auth/forgot-password',
      data
    )
    return response.data
  },

  async resetPassword(data: ResetPasswordRequest): Promise<ApiResponse> {
    const response = await httpClient.post<ApiResponse>(
      '/auth/reset-password',
      data
    )
    return response.data
  },

  async changePassword(data: ChangePasswordRequest): Promise<ApiResponse> {
    const response = await httpClient.post<ApiResponse>(
      '/auth/change-password',
      data
    )
    return response.data
  },

  async getUserInfo(): Promise<ApiResponse<UserInfo>> {
    const response = await httpClient.get<ApiResponse<UserInfo>>('/auth/info')
    return response.data
  },

  async updateUserInfo(
    data: UpdateUserInfoRequest
  ): Promise<ApiResponse<UserInfo>> {
    const response = await httpClient.post<ApiResponse<UserInfo>>(
      '/auth/info',
      data
    )
    return response.data
  },

  async uploadProfilePicture(file: File): Promise<ApiResponse<{ url: string }>> {
    const formData = new FormData()
    formData.append('image', file)

    const response = await httpClient.post<ApiResponse<{ url: string }>>( 
      '/auth/upload-profile-picture',
      formData,
      {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      }
    )
    return response.data
  },
}
