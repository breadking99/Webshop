import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { finalize, Subject, takeUntil } from 'rxjs';
import { OrderService } from '../../core/services/order.service';
import { DataStoreService } from '../../core/state/data-store.service';
import { OrderProduct } from '../../shared/models/order-product';
import { Order } from '../../shared/models/order';
import { createLoadingResponse, Response } from '../../shared/responses/response';
import { isSuccessStatus, ResponseStatus } from '../../shared/enums/response-status';

@Component({
  selector: 'app-order',
  standalone: false,
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrderComponent implements OnDestroy {
  order: Order;
  response: Response<unknown> | null = null;
  isSubmitting = false;

  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly orderService: OrderService,
    private readonly store: DataStoreService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef
  ) {
    this.order = this.store.currentOrder;

    if (!this.store.token) {
      this.router.navigate(['/login'], { replaceUrl: true });
      return;
    }

    this.store.order$
      .pipe(takeUntil(this.destroy$))
      .subscribe(order => {
        this.order = order;
        this.cdr.markForCheck();
      });
  }

  get items(): OrderProduct[] {
    return this.order.orderProducts ?? [];
  }

  get hasItems(): boolean {
    return this.items.length > 0;
  }

  updateCount(productId: number, value: string): void {
    const parsed = Number(value);
    if (!Number.isNaN(parsed)) {
      this.store.setProductCount(productId, parsed);
    }
  }

  removeProduct(productId: number): void {
    this.store.removeProduct(productId);
  }

  clearOrder(): void {
    this.store.resetOrder();
  }

  submit(): void {
    if (!this.hasItems) {
      this.response = {
        statusCode: ResponseStatus.BadRequest,
        message: 'Your basket is empty.'
      };
      return;
    }

    this.response = createLoadingResponse<unknown>();
    this.isSubmitting = true;
  this.cdr.markForCheck();

    const request = this.store.createOrderRequest();

    this.orderService.submitOrder(request).pipe(
      takeUntil(this.destroy$),
      finalize(() => {
        this.isSubmitting = false;
        this.cdr.markForCheck();
      })
    ).subscribe(response => {
      this.response = response;
      if (isSuccessStatus(response.statusCode)) {
        if (!response.message) {
          this.response = {
            ...response,
            message: 'Order submitted successfully.'
          };
        }
        this.store.resetOrder();
      }
      this.cdr.markForCheck();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
