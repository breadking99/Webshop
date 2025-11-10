import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';
import { Response } from '../../shared/responses/response';
import { Product } from '../../shared/models/product';
import { ProductFilter } from '../../shared/queries/product-filter';

@Injectable({ providedIn: 'root' })
export class ProductService extends BaseApiService {
  constructor(http: HttpClient) {
    super(http);
  }

  getProducts(filter?: ProductFilter | null): Observable<Response<Product[]>> {
    const params = this.createFilterParams(filter);
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

  private createFilterParams(filter?: ProductFilter | null): HttpParams | undefined {
    if (!filter) {
      return undefined;
    }

    let params = new HttpParams();

    if (filter.number > 0) {
      params = params.set('Number', filter.number.toString());
    }

    if (filter.size > 0) {
      params = params.set('Size', filter.size.toString());
    }

    if (filter.nameContains) {
      params = params.set('NameContains', filter.nameContains);
    }

    if (typeof filter.onlyAvailable === 'boolean') {
      params = params.set('OnlyAvailable', filter.onlyAvailable ? 'true' : 'false');
    }

    return params.keys().length > 0 ? params : undefined;
  }
}
