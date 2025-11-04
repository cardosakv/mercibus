export interface ApiSuccessResponse<T = any> {
  data: T | null;
}

export interface ApiErrorResponse {
  error: ApiError;
}

export interface ApiError {
  type: string;
  code: string;
  params?: BadRequestParam[];
}

export interface BadRequestParam {
  field: string;
  code: string;
}
