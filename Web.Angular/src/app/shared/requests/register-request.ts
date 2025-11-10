import { LoginRequest } from './login-request';

export interface RegisterRequest extends LoginRequest {
  confirmPassword: string;
}
