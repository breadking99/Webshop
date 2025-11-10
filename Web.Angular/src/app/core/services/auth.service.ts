import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { BaseApiService } from './base-api.service';
import { LoginRequest } from '../../shared/requests/login-request';
import { RegisterRequest } from '../../shared/requests/register-request';
import { Response } from '../../shared/responses/response';
import { DataStoreService } from '../state/data-store.service';
import { isSuccessStatus } from '../../shared/enums/response-status';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseApiService {
  constructor(
    http: HttpClient,
    private readonly dataStore: DataStoreService
  ) {
    super(http);
  }

  login(request: LoginRequest): Observable<Response<string>> {
    const call = this.http.post(`${this.baseUrl}/auth/login`, request, {
      observe: 'response',
      responseType: 'text'
    }) as unknown as Observable<HttpResponse<string>>;

    return this.toResponse(call).pipe(
      tap(response => this.handleToken(response))
    );
  }

  register(request: RegisterRequest): Observable<Response<string>> {
    const call = this.http.post(`${this.baseUrl}/auth/register`, request, {
      observe: 'response',
      responseType: 'text'
    }) as unknown as Observable<HttpResponse<string>>;

    return this.toResponse(call).pipe(
      tap(response => this.handleToken(response))
    );
  }

  private handleToken(response: Response<string>): void {
    if (response.value && isSuccessStatus(response.statusCode)) {
      this.dataStore.token = response.value;
    } else if (response.statusCode === 401) {
      this.dataStore.resetAuth();
    }
  }
}
