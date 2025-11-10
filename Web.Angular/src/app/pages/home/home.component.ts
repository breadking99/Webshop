import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Observable } from 'rxjs';
import { DataStoreService } from '../../core/state/data-store.service';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent {
  readonly isLoggedIn$: Observable<boolean>;

  constructor(private readonly store: DataStoreService) {
    this.isLoggedIn$ = this.store.isLoggedIn$;
  }
}
