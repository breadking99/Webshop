export interface AuthRequest {
  email: string;
  password: string;
  confirmPassword?: string | null;
}
