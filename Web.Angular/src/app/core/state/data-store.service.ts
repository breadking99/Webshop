import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { Order } from '../../shared/models/order';
import { OrderProduct } from '../../shared/models/order-product';
import { Product } from '../../shared/models/product';
import { AuthData, hasValidToken } from '../../shared/responses/auth-data';

const STORAGE_AUTH_KEY = 'webshop-auth';
const EMPTY_AUTH: AuthData = {
  success: false,
  message: null,
  username: null,
  email: null,
  password: null,
  token: '',
  validTo: null
};

@Injectable({ providedIn: 'root' })
export class DataStoreService {
  private readonly authSubject = new BehaviorSubject<AuthData>(this.readInitialAuth());
  private readonly orderSubject = new BehaviorSubject<Order>(this.createEmptyOrder());

  readonly auth$ = this.authSubject.asObservable();
  readonly token$ = this.auth$.pipe(map(data => data.token?.trim() ?? ''));
  readonly order$ = this.orderSubject.asObservable();
  readonly isLoggedIn$ = this.auth$.pipe(map(hasValidToken));

  get token(): string {
    return this.authSubject.value.token?.trim() ?? '';
  }

  get authData(): AuthData {
    return this.authSubject.value;
  }

  set authData(value: AuthData | null) {
    const next = this.normaliseAuth(value);
    this.authSubject.next(next);
    this.persistAuth(next);
  }

  get currentOrder(): Order {
    return this.orderSubject.value;
  }

  set currentOrder(order: Order) {
    this.orderSubject.next(order);
  }

  resetAuth(): void {
    this.authData = null;
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

  private normaliseAuth(value: AuthData | null): AuthData {
    if (!value) {
      return { ...EMPTY_AUTH };
    }

    const validTo = value.validTo instanceof Date
      ? value.validTo.toISOString()
      : value.validTo ?? null;

    return {
      success: Boolean(value.success),
      message: value.message ?? null,
      username: value.username ?? null,
      email: value.email ?? null,
      password: null,
      token: value.token?.trim() ?? '',
      validTo
    };
  }

  private persistAuth(auth: AuthData): void {
    try {
      if (!auth.token) {
        localStorage.removeItem(STORAGE_AUTH_KEY);
      } else {
        localStorage.setItem(STORAGE_AUTH_KEY, JSON.stringify(auth));
      }
    } catch {
      // Ignore storage failures (e.g. SSR, private mode)
    }
  }

  private readInitialAuth(): AuthData {
    try {
      const raw = localStorage.getItem(STORAGE_AUTH_KEY);
      if (!raw) {
        return { ...EMPTY_AUTH };
      }

      const parsed = JSON.parse(raw) as AuthData;
      return this.normaliseAuth(parsed);
    } catch {
      return { ...EMPTY_AUTH };
    }
  }
}
