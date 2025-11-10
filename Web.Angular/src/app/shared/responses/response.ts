import { ResponseStatus, isSuccessStatus } from '../enums/response-status';

export interface BaseResponse {
  statusCode: number;
  message?: string | null;
}

export interface Response<T> extends BaseResponse {
  value?: T;
}

export const createLoadingResponse = <T>(): Response<T> => ({
  statusCode: ResponseStatus.Loading
});

export const hasValue = <T>(response: Response<T>): response is Response<T> & { value: T } =>
  response.value !== undefined && isSuccessStatus(response.statusCode);
