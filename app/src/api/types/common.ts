export interface ApiResponse<T = unknown> {
  data?: T
  message?: string
  errors?: Record<string, string[]>
}

export interface PaginationParams {
  page?: number
  pageSize?: number
}

export interface PaginatedResponse<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}
