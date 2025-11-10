import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { DataStoreService } from './core/state/data-store.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class App {
  readonly isLoggedIn$: Observable<boolean>;

  constructor(
    private readonly store: DataStoreService,
    private readonly router: Router
  ) {
    this.isLoggedIn$ = this.store.isLoggedIn$;
  }

  logout(): void {
    this.store.resetAuth();
    this.router.navigate(['/login']);
  }
}
