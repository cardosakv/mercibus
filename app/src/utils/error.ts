import axios from 'axios';
import type { ApiError } from '@/types/api';
import { errorMap } from './error-map';

export function isApiError(err: any): err is ApiError {
  return (
    err && typeof err === 'object' && typeof err.code === 'string' && typeof err.type === 'string'
  );
}

export function getErrorMessage(error: unknown): string {
  // Axios network error (no response)
  if (axios.isAxiosError(error) && !error.response) {
    return 'Network error. Please check your connection.';
  }

  // API error from backend
  if (isApiError(error)) {
    if (error.type === 'validation' && Array.isArray(error.params)) {
      const first = error.params[0];
      const mapped = errorMap[first.code];
      return mapped ?? `${first.field}: ${first.code}`;
    }

    // Direct error code mapping
    if (errorMap[error.code]) {
      return errorMap[error.code];
    }

    // Fallback API error
    return error.code;
  }

  // Other unknown errors
  if (error instanceof Error) {
    return error.message || 'Unexpected error occurred.';
  }

  return 'Unexpected error occurred.';
}
