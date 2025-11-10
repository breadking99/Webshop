export interface Product {
  id: number;
  name: string;
  store: number;
  orderedCount?: number;
  isAvailable?: boolean;
}
