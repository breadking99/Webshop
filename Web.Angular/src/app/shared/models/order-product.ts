import { Product } from './product';

export interface OrderProduct {
  id?: number;
  orderId?: number;
  productId: number;
  count: number;
  product?: Product;
}
