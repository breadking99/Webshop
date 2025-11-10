import { PagerQuery } from './pager-query';

export interface ProductFilter extends PagerQuery {
  nameContains?: string;
  onlyAvailable?: boolean;
}
