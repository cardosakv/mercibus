import type { PaginationParams } from './common'

export interface Product {
  id: number
  name: string
  description: string
  price: number
  stock: number
  brandId?: number
  brandName?: string
  categoryId?: number
  categoryName?: string
  images?: ProductImage[]
  reviews?: ProductReview[]
  averageRating?: number
  reviewCount?: number
  createdAt: string
  updatedAt: string
}

export interface ProductRequest {
  name: string
  description: string
  price: number
  stock: number
  brandId?: number
  categoryId?: number
}

export interface ProductQuery extends PaginationParams {
  name?: string
  brandId?: number
  categoryId?: number
  minPrice?: number
  maxPrice?: number
  sortBy?: string
  sortOrder?: 'asc' | 'desc'
}

export interface ProductImage {
  id: number
  productId: number
  imageUrl: string
  isPrimary: boolean
  displayOrder: number
  createdAt: string
}

export interface ProductImageRequest {
  imageUrl: string
  isPrimary?: boolean
  displayOrder?: number
}

export interface ProductReview {
  id: number
  productId: number
  userId: string
  userName: string
  rating: number
  comment: string
  createdAt: string
  updatedAt: string
}

export interface ProductReviewRequest {
  rating: number
  comment: string
}

export interface Brand {
  id: number
  name: string
  description?: string
  logoUrl?: string
  createdAt: string
  updatedAt: string
}

export interface BrandRequest {
  name: string
  description?: string
  logoUrl?: string
}

export interface Category {
  id: number
  name: string
  description?: string
  parentCategoryId?: number
  createdAt: string
  updatedAt: string
}

export interface CategoryRequest {
  name: string
  description?: string
  parentCategoryId?: number
}
