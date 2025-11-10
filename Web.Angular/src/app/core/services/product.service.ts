import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';
import { Response } from '../../shared/responses/response';
import { Product } from '../../shared/models/product';
import { ProductFilter } from '../../shared/queries/product-filter';
import { toQueryString } from '../../shared/extensions/query-string';

@Injectable({ providedIn: 'root' })
export class ProductService extends BaseApiService {
  constructor(http: HttpClient) {
    super(http);
  }

  getProducts(filter?: ProductFilter | null): Observable<Response<Product[]>> {
    const query = filter ? toQueryString(filter) : '';
    const params = query ? new HttpParams({ fromString: query }) : undefined;
    const request = this.http.get<Product[]>(`${this.baseUrl}/products`, {
      params,
      observe: 'response'
    });

    return this.toResponse(request);
  }

  getProductById(id: number): Observable<Response<Product>> {
    const request = this.http.get<Product>(`${this.baseUrl}/products/${id}`, {
      observe: 'response'
    });

    return this.toResponse(request);
  }
}
