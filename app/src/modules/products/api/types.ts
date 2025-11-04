export interface ProductRequest {
  name: string;
  description?: string;
  sku: string;
  price: number;
  stockQuantity: number;
  attributes: Record<string, string>;
  categoryId: number;
  brandId: number;
}

export interface ProductResponse {
  id: number;
  name: string;
  description: string;
  price: number;
  sku: string;
  stockQuantity: number;
  brandId: number;
  categoryId: number;
  images: ProductImageResponse[];
  attributes: Record<string, string>;
  createdAt: string;
}

export interface ProductQuery {
  categoryId?: number;
  brandId?: number;
  minPrice?: number;
  maxPrice?: number;
  sortBy?: string;
  sortDirection?: string;
  page?: number;
  pageSize?: number;
}

export interface ProductImageRequest {
  image: File;
  isPrimary?: boolean;
  altText?: string;
}

export interface ProductImageResponse {
  id: number;
  imageUrl: string;
  isPrimary: boolean;
  altText?: string;
}

export interface ProductReviewRequest {
  rating: number;
  comment?: string;
}

export interface ProductReviewResponse {
  id: number;
  productId: number;
  userId: string;
  rating: number;
  comment?: string;
  createdAt: string;
}

export interface ProductReviewQuery {
  userId?: string;
  minRating?: number;
  maxRating?: number;
  page?: number;
  pageSize?: number;
}
