const ACCESS_TOKEN_KEY = 'access_token'
const REFRESH_TOKEN_KEY = 'refresh_token'
const TOKEN_EXPIRY_KEY = 'token_expiry'

export const tokenStorage = {
  getAccessToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN_KEY)
  },

  setAccessToken(token: string): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, token)
  },

  getRefreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN_KEY)
  },

  setRefreshToken(token: string): void {
    localStorage.setItem(REFRESH_TOKEN_KEY, token)
  },

  getTokenExpiry(): number | null {
    const expiry = localStorage.getItem(TOKEN_EXPIRY_KEY)
    return expiry ? parseInt(expiry, 10) : null
  },

  setTokenExpiry(expiresIn: number): void {
    const expiryTime = Date.now() + expiresIn
    localStorage.setItem(TOKEN_EXPIRY_KEY, expiryTime.toString())
  },

  clearTokens(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY)
    localStorage.removeItem(REFRESH_TOKEN_KEY)
    localStorage.removeItem(TOKEN_EXPIRY_KEY)
  },

  isTokenExpired(): boolean {
    const expiry = this.getTokenExpiry()
    if (!expiry) return true
    // Consider token expired 30 seconds before actual expiry
    return Date.now() >= expiry - 30000
  },
}
