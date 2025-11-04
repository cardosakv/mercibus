export interface BrandQuery {
  region?: string;
  page?: number;
  pageSize?: number;
}

export interface BrandRequest {
  name: string;
  description?: string;
  logoUrl?: string;
  region?: string;
  website?: string;
  additionalInfo?: string;
}

export interface BrandResponse {
  id: number;
  name: string;
  description?: string;
  logoUrl?: string;
  region?: string;
  website?: string;
  additionalInfo?: string;
}
