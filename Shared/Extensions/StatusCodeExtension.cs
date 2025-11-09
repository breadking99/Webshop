using Shared.Enums;

namespace Shared.Extensions;

public static class StatusCodeExtension
{
    public static EResponseStatus FromStatusCode(this int statusCode) => statusCode switch
    {
        200 => EResponseStatus.Success,
        400 => EResponseStatus.BadRequest,
        401 => EResponseStatus.Unauthorized,
        403 => EResponseStatus.Forbid,
        404 => EResponseStatus.NotFound,
        408 => EResponseStatus.TimeOut,
        500 => EResponseStatus.InternalServerError,
        1   => EResponseStatus.Offline,
        _ => EResponseStatus.Unknown,
    };

    public static int ToStatusCode(this EResponseStatus status) => status switch
    {
        EResponseStatus.Success => 200,
        EResponseStatus.BadRequest => 400,
        EResponseStatus.Unauthorized => 401,
        EResponseStatus.Forbid => 403,
        EResponseStatus.NotFound => 404,
        EResponseStatus.TimeOut => 408,
        EResponseStatus.InternalServerError => 500,
        EResponseStatus.Offline => 1,
        _ => 0,
    };

    public static bool IsSuccessStatusCode(this int statusCode)
    {
        return statusCode >= 200 && statusCode <= 299;
    }
}