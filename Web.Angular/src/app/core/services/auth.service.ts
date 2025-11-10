import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { BaseApiService } from './base-api.service';
import { AuthRequest } from '../../shared/requests/auth-request';
import { Response } from '../../shared/responses/response';
import { DataStoreService } from '../state/data-store.service';
import { isSuccessStatus, ResponseStatus } from '../../shared/enums/response-status';
import { AuthData, hasValidToken } from '../../shared/responses/auth-data';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseApiService {
  constructor(
    http: HttpClient,
    private readonly dataStore: DataStoreService
  ) {
    super(http);
  }

  login(request: AuthRequest): Observable<Response<AuthData>> {
    console.log('[AuthService] login request', { email: request.email });
    const call = this.http.post<AuthData>(`${this.baseUrl}/auth/login`, request, {
      observe: 'response'
    });

    return this.toResponse(call).pipe(
      tap(response => {
        console.log('[AuthService] login response', response);
        this.handleAuth(response);
      })
    );
  }

  register(request: AuthRequest): Observable<Response<AuthData>> {
    console.log('[AuthService] register request', { email: request.email });
    const call = this.http.post<AuthData>(`${this.baseUrl}/auth/register`, request, {
      observe: 'response'
    });

    return this.toResponse(call).pipe(
      tap(response => {
        console.log('[AuthService] register response', response);
        this.handleAuth(response);
      })
    );
  }

  private handleAuth(response: Response<AuthData>): void {
    if (response.value && isSuccessStatus(response.statusCode) && hasValidToken(response.value)) {
      this.dataStore.authData = response.value;
    } else if (response.statusCode === ResponseStatus.Unauthorized) {
      this.dataStore.resetAuth();
    }
  }
}
