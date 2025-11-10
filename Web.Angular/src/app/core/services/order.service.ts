import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { BaseApiService } from './base-api.service';
import { Response } from '../../shared/responses/response';
import { Order } from '../../shared/models/order';

@Injectable({ providedIn: 'root' })
export class OrderService extends BaseApiService {
  constructor(http: HttpClient) {
    super(http);
  }

  getMyOrders(): Observable<Response<Order[]>> {
    const request = this.http.get<Order[]>(`${this.baseUrl}/orders/my`, {
      observe: 'response'
    });

    return this.toResponse(request).pipe(
      tap(response => console.log('[OrderService] getMyOrders response', response))
    );
  }

  submitOrder(order: Order): Observable<Response<unknown>> {
    console.log('[OrderService] submitOrder payload', order);
    const request = this.http.post(`${this.baseUrl}/orders`, order, {
      observe: 'response'
    });

    return this.toResponse(request).pipe(
      tap(response => console.log('[OrderService] submitOrder response', response))
    );
  }
}
