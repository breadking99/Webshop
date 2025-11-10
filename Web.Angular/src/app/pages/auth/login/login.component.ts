import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize, Subject, takeUntil } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { DataStoreService } from '../../../core/state/data-store.service';
import { createLoadingResponse, Response } from '../../../shared/responses/response';
import { isSuccessStatus } from '../../../shared/enums/response-status';
import { LoginRequest } from '../../../shared/requests/login-request';
import { AuthData, hasValidToken } from '../../../shared/responses/auth-data';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent implements OnDestroy {
  readonly form: FormGroup<{
    email: FormControl<string>;
    password: FormControl<string>;
  }>;

  response: Response<AuthData> | null = null;
  isSubmitting = false;

  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly store: DataStoreService
  ) {
    this.form = this.fb.nonNullable.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });

    if (this.store.token) {
      this.router.navigate(['/'], { replaceUrl: true });
    }
  }

  submit(): void {
    if (this.form.invalid || this.isSubmitting) {
      this.form.markAllAsTouched();
      return;
    }

    const request: LoginRequest = this.form.getRawValue();

  this.response = createLoadingResponse<AuthData>();
    this.isSubmitting = true;

    this.authService.login(request).pipe(
      takeUntil(this.destroy$),
      finalize(() => {
        this.isSubmitting = false;
      })
    ).subscribe(response => {
      this.response = response;
      if (isSuccessStatus(response.statusCode) && response.value && hasValidToken(response.value)) {
        this.router.navigate(['/']);
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  get email() {
    return this.form.controls.email;
  }

  get password() {
    return this.form.controls.password;
  }
}
