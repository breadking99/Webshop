import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize, Subject, takeUntil } from 'rxjs';
import { ProductService } from '../../core/services/product.service';
import { DataStoreService } from '../../core/state/data-store.service';
import { Product } from '../../shared/models/product';
import { createLoadingResponse, Response } from '../../shared/responses/response';
import { isSuccessStatus } from '../../shared/enums/response-status';

@Component({
  selector: 'app-product-detail',
  standalone: false,
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductDetailComponent implements OnInit, OnDestroy {
  product: Product | null = null;
  response: Response<Product> | null = null;
  isLoading = false;
  quantity = 1;
  infoMessage = '';

  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly route: ActivatedRoute,
    private readonly productService: ProductService,
    private readonly store: DataStoreService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef
  ) {
    if (!this.store.token) {
      this.router.navigate(['/login'], { replaceUrl: true });
    }
  }

  ngOnInit(): void {
    if (!this.store.token) {
      return;
    }

    this.route.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        const id = Number(params.get('id'));
        if (!Number.isInteger(id) || id <= 0) {
          this.router.navigate(['/products']);
          return;
        }

        this.loadProduct(id);
      });
  }

  addToBasket(): void {
    if (!this.product || !this.canAdd) {
      return;
    }

    const selected = Math.min(Math.max(1, this.quantity), this.maxSelectable);
    this.store.incrementProduct(this.product, selected);
    this.quantity = selected;
    this.infoMessage = selected === 1
      ? 'Added 1 item to your basket.'
      : `Added ${selected} items to your basket.`;
  }

  onQuantityChange(value: string): void {
    const parsed = Number(value);
    if (!Number.isNaN(parsed)) {
      this.quantity = Math.max(1, Math.floor(parsed));
    }
  }

  get canAdd(): boolean {
    return !!this.product && this.product.store > 0;
  }

  get maxSelectable(): number {
    return Math.max(1, this.product?.store ?? 1);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadProduct(id: number): void {
    this.isLoading = true;
    this.response = createLoadingResponse<Product>();
    this.infoMessage = '';
  this.cdr.markForCheck();

    this.productService.getProductById(id).pipe(
      takeUntil(this.destroy$),
      finalize(() => {
        this.isLoading = false;
        this.cdr.markForCheck();
      })
    ).subscribe(response => {
      this.response = response;
      const hasValue = isSuccessStatus(response.statusCode) && !!response.value;
      this.product = hasValue ? response.value ?? null : null;

      if (!this.product && (!response?.message || response.message.trim().length === 0)) {
        if (isSuccessStatus(response.statusCode) || response.statusCode === 404) {
          this.infoMessage = 'We could not find this product.';
        } else {
          this.infoMessage = '';
        }
      }

      this.quantity = 1;
      this.cdr.markForCheck();
    });
  }
}
