namespace Shared.Enums;

public enum EResponseStatus
{
    // Non-HTTP sentinel values
    Default = 0,
    Loading = 1,
    Offline = 2,
    DeserializeError = 3,
    UnknownError = 4,

    // HTTP-aligned values
    Ok = 200,
    BadRequest = 400,
    Unauthorized = 401,
    Forbid = 403,
    NotFound = 404,
    TimeOut = 408,
    InternalServerError = 500,
}