import { OrderProduct } from './order-product';
import { User } from './user';

export interface Order {
  id?: number;
  userId?: string | null;
  user?: User | null;
  orderProducts?: OrderProduct[];
}
