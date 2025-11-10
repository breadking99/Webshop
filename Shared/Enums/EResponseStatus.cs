namespace Shared.Enums;

public enum EResponseStatus
{
    // Non-HTTP sentinel values
    Unknown = 0,
    Offline = 1,
    DeserializeError = 2,

    // HTTP-aligned values
    Ok = 200,
    BadRequest = 400,
    Unauthorized = 401,
    Forbid = 403,
    NotFound = 404,
    TimeOut = 408,
    InternalServerError = 500,
}