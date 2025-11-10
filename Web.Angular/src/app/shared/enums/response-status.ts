export enum ResponseStatus {
  Default = 0,
  Loading = 1,
  Offline = 2,
  DeserializeError = 3,
  UnknownError = 4,
  Ok = 200,
  BadRequest = 400,
  Unauthorized = 401,
  Forbid = 403,
  NotFound = 404,
  TimeOut = 408,
  InternalServerError = 500
}

export const isSuccessStatus = (status: ResponseStatus | number): boolean => {
  const value = Number(status);
  return value >= 200 && value <= 299;
};
