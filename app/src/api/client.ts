import axios, { AxiosError } from 'axios'
import type {
  AxiosInstance,
  InternalAxiosRequestConfig,
  AxiosResponse,
} from 'axios'
import { tokenStorage } from './utils/tokenStorage'
import type { AuthTokenResponse } from './types'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:9000/api'

class HttpClient {
  private client: AxiosInstance
  private isRefreshing = false
  private refreshQueue: Array<{
    resolve: (token: string) => void
    reject: (error: unknown) => void
  }> = []

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    })

    this.setupInterceptors()
  }

  private setupInterceptors(): void {
    // Request interceptor: Inject JWT token
    this.client.interceptors.request.use(
      (config: InternalAxiosRequestConfig) => {
        const token = tokenStorage.getAccessToken()
        if (token && config.headers) {
          config.headers.Authorization = `Bearer ${token}`
        }
        return config
      },
      (error) => {
        return Promise.reject(error)
      }
    )

    // Response interceptor: Handle errors and token refresh
    this.client.interceptors.response.use(
      (response: AxiosResponse) => {
        return response
      },
      async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & {
          _retry?: boolean
        }

        // Handle 401 Unauthorized errors
        if (error.response?.status === 401 && !originalRequest._retry) {
          originalRequest._retry = true

          if (this.isRefreshing) {
            // Wait for the token refresh to complete
            return new Promise((resolve, reject) => {
              this.refreshQueue.push({ resolve, reject })
            })
              .then((token) => {
                if (originalRequest.headers) {
                  originalRequest.headers.Authorization = `Bearer ${token}`
                }
                return this.client(originalRequest)
              })
              .catch((err) => {
                return Promise.reject(err)
              })
          }

          this.isRefreshing = true

          try {
            const refreshToken = tokenStorage.getRefreshToken()
            if (!refreshToken) {
              throw new Error('No refresh token available')
            }

            // Call refresh token endpoint
            const response = await axios.post<{ data: AuthTokenResponse }>(
              `${API_BASE_URL}/auth/refresh-token`,
              { refreshToken }
            )

            const { accessToken, refreshToken: newRefreshToken, expiresIn } = response.data.data

            // Store new tokens
            tokenStorage.setAccessToken(accessToken)
            tokenStorage.setRefreshToken(newRefreshToken)
            tokenStorage.setTokenExpiry(expiresIn)

            // Retry all queued requests with new token
            this.refreshQueue.forEach(({ resolve }) => {
              resolve(accessToken)
            })
            this.refreshQueue = []

            // Retry the original request
            if (originalRequest.headers) {
              originalRequest.headers.Authorization = `Bearer ${accessToken}`
            }
            return this.client(originalRequest)
          } catch (refreshError) {
            // Refresh failed, clear tokens and reject all queued requests
            this.refreshQueue.forEach(({ reject }) => {
              reject(refreshError)
            })
            this.refreshQueue = []
            tokenStorage.clearTokens()
            
            // Redirect to login or dispatch logout action
            window.location.href = '/login'
            
            return Promise.reject(refreshError)
          } finally {
            this.isRefreshing = false
          }
        }

        // Handle other errors
        return Promise.reject(this.handleError(error))
      }
    )
  }

  private handleError(error: AxiosError): Error {
    if (error.response) {
      // Server responded with an error status
      const data = error.response.data as { message?: string; errors?: Record<string, string[]> }
      const message = data.message || `Error: ${error.response.status}`
      const enhancedError = new Error(message)
      Object.assign(enhancedError, {
        status: error.response.status,
        errors: data.errors,
      })
      return enhancedError
    } else if (error.request) {
      // Request was made but no response received
      return new Error('Network error: Unable to reach the server')
    } else {
      // Something else happened
      return new Error(error.message || 'An unexpected error occurred')
    }
  }

  public getClient(): AxiosInstance {
    return this.client
  }
}

// Export singleton instance
export const httpClient = new HttpClient().getClient()
