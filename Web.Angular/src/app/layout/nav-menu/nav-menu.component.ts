import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Observable, map } from 'rxjs';
import { DataStoreService } from '../../core/state/data-store.service';

@Component({
  selector: 'app-nav-menu',
  standalone: false,
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavMenuComponent {
  readonly isLoggedIn$: Observable<boolean>;
  readonly basketCount$: Observable<number>;

  constructor(private readonly store: DataStoreService) {
    this.isLoggedIn$ = this.store.isLoggedIn$;
    this.basketCount$ = this.store.order$.pipe(
      map(order => (order.orderProducts ?? []).reduce((acc, item) => acc + item.count, 0))
    );
  }
}
