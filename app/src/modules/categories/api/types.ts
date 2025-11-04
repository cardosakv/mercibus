export interface CategoryQuery {
  parentCategoryId?: number;
  page?: number;
  pageSize?: number;
}

export interface CategoryRequest {
  parentCategoryId?: number | null;
  name: string;
  description?: string | null;
}

export interface CategoryResponse {
  id: number;
  parentCategoryId?: number | null;
  name: string;
  description?: string | null;
}
