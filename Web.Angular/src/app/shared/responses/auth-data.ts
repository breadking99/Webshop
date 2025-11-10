export interface AuthData {
  success: boolean;
  message?: string | null;
  username?: string | null;
  email?: string | null;
  password?: string | null;
  token?: string | null;
  validTo?: string | Date | null;
}

export const hasValidToken = (data: AuthData | null | undefined): boolean => {
  if (!data?.token || !data.success) {
    return false;
  }

  if (!data.validTo) {
    return true;
  }

  const expiry = typeof data.validTo === 'string'
    ? new Date(data.validTo)
    : data.validTo;

  return Number.isFinite(expiry.getTime()) && expiry.getTime() > Date.now();
};
