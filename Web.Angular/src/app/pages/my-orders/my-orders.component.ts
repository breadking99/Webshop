import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { finalize, Subject, takeUntil } from 'rxjs';
import { OrderService } from '../../core/services/order.service';
import { DataStoreService } from '../../core/state/data-store.service';
import { Order } from '../../shared/models/order';
import { createLoadingResponse, Response } from '../../shared/responses/response';
import { isSuccessStatus } from '../../shared/enums/response-status';

@Component({
  selector: 'app-my-orders',
  standalone: false,
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyOrdersComponent implements OnDestroy {
  orders: Order[] = [];
  response: Response<Order[]> | null = null;
  isLoading = false;

  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly orderService: OrderService,
    private readonly store: DataStoreService,
    private readonly router: Router
  ) {
    if (!this.store.token) {
      this.router.navigate(['/login'], { replaceUrl: true });
      return;
    }

    this.loadOrders();
  }

  trackByOrderId(index: number, order: Order): number | undefined {
    return order.id ?? index;
  }

  countItems(order: Order): number {
    return order.orderProducts?.reduce((total, item) => total + (item.count ?? 0), 0) ?? 0;
  }

  get hasSuccessResponse(): boolean {
    return !!this.response && isSuccessStatus(this.response.statusCode);
  }

  private loadOrders(): void {
    this.isLoading = true;
    this.response = createLoadingResponse<Order[]>();

    this.orderService.getMyOrders().pipe(
      takeUntil(this.destroy$),
      finalize(() => {
        this.isLoading = false;
      })
    ).subscribe(response => {
      this.response = response;
      this.orders = isSuccessStatus(response.statusCode) && response.value
        ? response.value
        : [];
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
