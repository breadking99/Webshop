import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize, Subject, takeUntil } from 'rxjs';
import { ProductService } from '../../core/services/product.service';
import { DataStoreService } from '../../core/state/data-store.service';
import { Product } from '../../shared/models/product';
import { ProductFilter } from '../../shared/queries/product-filter';
import { createLoadingResponse, Response } from '../../shared/responses/response';
import { isSuccessStatus } from '../../shared/enums/response-status';

@Component({
  selector: 'app-products',
  standalone: false,
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductsComponent implements OnDestroy {
  readonly filterForm: FormGroup<{
    nameContains: FormControl<string>;
    onlyAvailable: FormControl<boolean>;
  }>;

  products: Product[] = [];
  response: Response<Product[]> | null = null;
  isLoading = false;
  readonly pageSizeOptions = [6, 12, 24];

  private pager = {
    number: 1,
    size: 12
  };

  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly fb: FormBuilder,
    private readonly productService: ProductService,
    private readonly router: Router,
    private readonly store: DataStoreService
  ) {
    this.filterForm = this.fb.nonNullable.group({
      nameContains: [''],
      onlyAvailable: [false]
    });

    if (!this.store.token) {
      this.router.navigate(['/login'], { replaceUrl: true });
      return;
    }

    this.loadProducts();
  }

  search(): void {
    this.pager.number = 1;
    this.loadProducts();
  }

  nextPage(): void {
    this.pager.number += 1;
    this.loadProducts();
  }

  prevPage(): void {
    if (this.pager.number === 1) {
      return;
    }

    this.pager.number -= 1;
    this.loadProducts();
  }

  changePageSize(size: number): void {
    if (this.pager.size === size) {
      return;
    }

    this.pager.size = size;
    this.pager.number = 1;
    this.loadProducts();
  }

  onPageSizeChange(event: Event): void {
    const value = Number((event.target as HTMLSelectElement).value);
    if (!Number.isNaN(value)) {
      this.changePageSize(value);
    }
  }

  get pagerNumber(): number {
    return this.pager.number;
  }

  get pageSize(): number {
    return this.pager.size;
  }

  get canPrev(): boolean {
    return this.pager.number > 1 && !this.isLoading;
  }

  get canNext(): boolean {
    return !this.isLoading && this.products.length === this.pager.size && this.hasSuccessResponse;
  }

  get hasSuccessResponse(): boolean {
    return !!this.response && isSuccessStatus(this.response.statusCode);
  }

  trackByProductId(index: number, product: Product): number {
    return product.id;
  }

  viewDetails(product: Product): void {
    this.router.navigate(['/products', product.id]);
  }

  private loadProducts(): void {
    if (!this.store.token) {
      return;
    }

    const filter = this.createFilter();

    this.isLoading = true;
    this.response = createLoadingResponse<Product[]>();

    this.productService.getProducts(filter).pipe(
      takeUntil(this.destroy$),
      finalize(() => {
        this.isLoading = false;
      })
    ).subscribe(response => {
      this.response = response;
      this.products = isSuccessStatus(response.statusCode) && response.value
        ? response.value
        : [];
    });
  }

  private createFilter(): ProductFilter {
    const { nameContains, onlyAvailable } = this.filterForm.getRawValue();
    const trimmedName = nameContains.trim();

    return {
      number: this.pager.number,
      size: this.pager.size,
      nameContains: trimmedName.length > 0 ? trimmedName : undefined,
      onlyAvailable: onlyAvailable ?? false
    };
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
