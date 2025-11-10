import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
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
    console.log('[ProductService] getProducts filter', filter);
    const request = this.http.get<Product[]>(`${this.baseUrl}/products`, {
      params,
      observe: 'response'
    });

    return this.toResponse(request).pipe(
      tap(response => console.log('[ProductService] getProducts response', response))
    );
  }

  getProductById(id: number): Observable<Response<Product>> {
    console.log('[ProductService] getProductById id', id);
    const request = this.http.get<Product>(`${this.baseUrl}/products/${id}`, {
      observe: 'response'
    });

    return this.toResponse(request).pipe(
      tap(response => console.log('[ProductService] getProductById response', response))
    );
  }
}
