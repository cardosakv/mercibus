import { httpClient } from '../client'
import type {
  Product,
  ProductRequest,
  ProductQuery,
  ProductImage,
  ProductImageRequest,
  ProductReview,
  ProductReviewRequest,
  Brand,
  BrandRequest,
  Category,
  CategoryRequest,
  ApiResponse,
  PaginatedResponse,
} from '../types'

export const productsService = {
  // Products
  async getProducts(
    query?: ProductQuery
  ): Promise<ApiResponse<PaginatedResponse<Product>>> {
    const response = await httpClient.get<ApiResponse<PaginatedResponse<Product>>>(
      '/products',
      { params: query }
    )
    return response.data
  },

  async getProductById(id: number): Promise<ApiResponse<Product>> {
    const response = await httpClient.get<ApiResponse<Product>>(`/products/${id}`)
    return response.data
  },

  async createProduct(data: ProductRequest): Promise<ApiResponse<Product>> {
    const response = await httpClient.post<ApiResponse<Product>>('/products', data)
    return response.data
  },

  async updateProduct(
    id: number,
    data: ProductRequest
  ): Promise<ApiResponse<Product>> {
    const response = await httpClient.put<ApiResponse<Product>>(
      `/products/${id}`,
      data
    )
    return response.data
  },

  async deleteProduct(id: number): Promise<ApiResponse> {
    const response = await httpClient.delete<ApiResponse>(`/products/${id}`)
    return response.data
  },

  // Product Images
  async getProductImages(productId: number): Promise<ApiResponse<ProductImage[]>> {
    const response = await httpClient.get<ApiResponse<ProductImage[]>>(
      `/products/${productId}/images`
    )
    return response.data
  },

  async addProductImage(
    productId: number,
    data: ProductImageRequest
  ): Promise<ApiResponse<ProductImage>> {
    const response = await httpClient.post<ApiResponse<ProductImage>>(
      `/products/${productId}/images`,
      data
    )
    return response.data
  },

  async updateProductImage(
    productId: number,
    imageId: number,
    data: ProductImageRequest
  ): Promise<ApiResponse<ProductImage>> {
    const response = await httpClient.put<ApiResponse<ProductImage>>(
      `/products/${productId}/images/${imageId}`,
      data
    )
    return response.data
  },

  async deleteProductImage(productId: number, imageId: number): Promise<ApiResponse> {
    const response = await httpClient.delete<ApiResponse>(
      `/products/${productId}/images/${imageId}`
    )
    return response.data
  },

  // Product Reviews
  async getProductReviews(
    productId: number
  ): Promise<ApiResponse<ProductReview[]>> {
    const response = await httpClient.get<ApiResponse<ProductReview[]>>(
      `/products/${productId}/reviews`
    )
    return response.data
  },

  async addProductReview(
    productId: number,
    data: ProductReviewRequest
  ): Promise<ApiResponse<ProductReview>> {
    const response = await httpClient.post<ApiResponse<ProductReview>>(
      `/products/${productId}/reviews`,
      data
    )
    return response.data
  },

  async updateProductReview(
    productId: number,
    reviewId: number,
    data: ProductReviewRequest
  ): Promise<ApiResponse<ProductReview>> {
    const response = await httpClient.put<ApiResponse<ProductReview>>(
      `/products/${productId}/reviews/${reviewId}`,
      data
    )
    return response.data
  },

  async deleteProductReview(
    productId: number,
    reviewId: number
  ): Promise<ApiResponse> {
    const response = await httpClient.delete<ApiResponse>(
      `/products/${productId}/reviews/${reviewId}`
    )
    return response.data
  },

  // Brands
  async getBrands(): Promise<ApiResponse<Brand[]>> {
    const response = await httpClient.get<ApiResponse<Brand[]>>('/brands')
    return response.data
  },

  async getBrandById(id: number): Promise<ApiResponse<Brand>> {
    const response = await httpClient.get<ApiResponse<Brand>>(`/brands/${id}`)
    return response.data
  },

  async createBrand(data: BrandRequest): Promise<ApiResponse<Brand>> {
    const response = await httpClient.post<ApiResponse<Brand>>('/brands', data)
    return response.data
  },

  async updateBrand(id: number, data: BrandRequest): Promise<ApiResponse<Brand>> {
    const response = await httpClient.put<ApiResponse<Brand>>(`/brands/${id}`, data)
    return response.data
  },

  async deleteBrand(id: number): Promise<ApiResponse> {
    const response = await httpClient.delete<ApiResponse>(`/brands/${id}`)
    return response.data
  },

  // Categories
  async getCategories(): Promise<ApiResponse<Category[]>> {
    const response = await httpClient.get<ApiResponse<Category[]>>('/categories')
    return response.data
  },

  async getCategoryById(id: number): Promise<ApiResponse<Category>> {
    const response = await httpClient.get<ApiResponse<Category>>(`/categories/${id}`)
    return response.data
  },

  async createCategory(data: CategoryRequest): Promise<ApiResponse<Category>> {
    const response = await httpClient.post<ApiResponse<Category>>(
      '/categories',
      data
    )
    return response.data
  },

  async updateCategory(
    id: number,
    data: CategoryRequest
  ): Promise<ApiResponse<Category>> {
    const response = await httpClient.put<ApiResponse<Category>>(
      `/categories/${id}`,
      data
    )
    return response.data
  },

  async deleteCategory(id: number): Promise<ApiResponse> {
    const response = await httpClient.delete<ApiResponse>(`/categories/${id}`)
    return response.data
  },
}
