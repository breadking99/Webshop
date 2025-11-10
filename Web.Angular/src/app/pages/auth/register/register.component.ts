import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize, Subject, takeUntil } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { DataStoreService } from '../../../core/state/data-store.service';
import { createLoadingResponse, Response } from '../../../shared/responses/response';
import { isSuccessStatus } from '../../../shared/enums/response-status';
import { RegisterRequest } from '../../../shared/requests/register-request';
import { AuthData, hasValidToken } from '../../../shared/responses/auth-data';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent implements OnDestroy {
  readonly form: FormGroup<{
    email: FormControl<string>;
    password: FormControl<string>;
    confirmPassword: FormControl<string>;
  }>;

  response: Response<AuthData> | null = null;
  isSubmitting = false;

  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly store: DataStoreService,
    private readonly cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.nonNullable.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
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

    if (!this.passwordsMatch) {
      const existing = this.form.controls.confirmPassword.errors ?? {};
      this.form.controls.confirmPassword.setErrors({ ...existing, mismatch: true });
      this.form.controls.confirmPassword.markAsTouched();
      return;
    }

    if (this.form.controls.confirmPassword.hasError('mismatch')) {
      const { mismatch, ...rest } = this.form.controls.confirmPassword.errors ?? {};
      this.form.controls.confirmPassword.setErrors(Object.keys(rest).length ? rest : null);
    }

    const request: RegisterRequest = this.form.getRawValue();

  this.response = createLoadingResponse<AuthData>();
    this.isSubmitting = true;
  this.cdr.markForCheck();

    this.authService.register(request).pipe(
      takeUntil(this.destroy$),
      finalize(() => {
        this.isSubmitting = false;
        this.cdr.markForCheck();
      })
    ).subscribe(response => {
      this.response = response;
      if (isSuccessStatus(response.statusCode) && response.value && hasValidToken(response.value)) {
        this.router.navigate(['/']);
      }
      this.cdr.markForCheck();
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

  get confirmPassword() {
    return this.form.controls.confirmPassword;
  }

  get passwordsMatch(): boolean {
    return this.password.value === this.confirmPassword.value;
  }
}
