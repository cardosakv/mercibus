export interface OrderItemRequest {
  productId: number;
  productName: string;
  quantity: number;
  price: number;
}

export interface OrderItemResponse {
  id: number;
  productId: number;
  productName: string;
  price: number;
  quantity: number;
}

export interface OrderRequest {
  currency: string;
  items: OrderItemRequest[];
}

export interface OrderResponse {
  id: number;
  userId: string;
  createdAt: string;
  status: string;
  currency: string;
  items: OrderItemResponse[];
}

export interface OrderUpdateRequest {
  status: string;
}
