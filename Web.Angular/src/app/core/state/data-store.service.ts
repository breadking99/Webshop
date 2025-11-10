import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { Order } from '../../shared/models/order';
import { OrderProduct } from '../../shared/models/order-product';
import { Product } from '../../shared/models/product';

const STORAGE_TOKEN_KEY = 'webshop-token';

@Injectable({ providedIn: 'root' })
export class DataStoreService {
  private readonly tokenSubject = new BehaviorSubject<string>(this.readInitialToken());
  private readonly orderSubject = new BehaviorSubject<Order>(this.createEmptyOrder());

  readonly token$ = this.tokenSubject.asObservable();
  readonly order$ = this.orderSubject.asObservable();
  readonly isLoggedIn$ = this.token$.pipe(map(token => token.trim().length > 0));

  get token(): string {
    return this.tokenSubject.value;
  }

  set token(value: string) {
    const normalized = value ?? '';
    if (normalized === this.tokenSubject.value) {
      return;
    }

    this.tokenSubject.next(normalized);
    this.persistToken(normalized);
  }

  get currentOrder(): Order {
    return this.orderSubject.value;
  }

  set currentOrder(order: Order) {
    this.orderSubject.next(order);
  }

  resetAuth(): void {
    this.token = '';
    this.resetOrder();
  }

  resetOrder(): void {
    this.orderSubject.next(this.createEmptyOrder());
  }

  incrementProduct(product: Product, count: number): void {
    if (!product || count <= 0) {
      return;
    }

    const order = this.clone(this.currentOrder);
    order.orderProducts = order.orderProducts ?? [];

    const existing = order.orderProducts.find(x => x.productId === product.id);

    if (existing) {
      existing.count += count;
      existing.product = existing.product ?? product;
    } else {
      order.orderProducts.push({
        productId: product.id,
        count,
        product
      });
    }

    this.orderSubject.next(order);
  }

  setProductCount(productId: number, count: number): void {
    const normalisedCount = Math.max(0, count);
    const order = this.clone(this.currentOrder);
    const items = order.orderProducts ?? [];
    const existing = items.find(x => x.productId === productId);

    if (!existing) {
      return;
    }

    if (normalisedCount === 0) {
      order.orderProducts = items.filter(x => x.productId !== productId);
    } else {
      existing.count = normalisedCount;
    }

    this.orderSubject.next(order);
  }

  removeProduct(productId: number): void {
    const order = this.clone(this.currentOrder);
    order.orderProducts = (order.orderProducts ?? []).filter(x => x.productId !== productId);
    this.orderSubject.next(order);
  }

  createOrderRequest(): Order {
    const items = this.currentOrder.orderProducts ?? [];
    return {
      orderProducts: items
        .filter(item => item.count > 0)
        .map<OrderProduct>(item => ({
          productId: item.productId,
          count: item.count
        }))
    };
  }

  private createEmptyOrder(): Order {
    return { orderProducts: [] };
  }

  private clone<T>(value: T): T {
    if (typeof structuredClone === 'function') {
      return structuredClone(value);
    }

    return JSON.parse(JSON.stringify(value)) as T;
  }

  private persistToken(token: string): void {
    try {
      if (!token) {
        localStorage.removeItem(STORAGE_TOKEN_KEY);
      } else {
        localStorage.setItem(STORAGE_TOKEN_KEY, token);
      }
    } catch {
      // Ignore storage failures (e.g. SSR, private mode)
    }
  }

  private readInitialToken(): string {
    try {
      return localStorage.getItem(STORAGE_TOKEN_KEY) ?? '';
    } catch {
      return '';
    }
  }
}
