import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable, map, take } from 'rxjs';
import { DataStoreService } from '../state/data-store.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private readonly store: DataStoreService,
    private readonly router: Router
  ) { }

  canActivate(): Observable<boolean | UrlTree> {
    return this.store.isLoggedIn$.pipe(
      take(1),
      map(isLoggedIn =>
        isLoggedIn ? true : this.router.createUrlTree(['/login'])
      )
    );
  }
}
