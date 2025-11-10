using Shared.Enums;
using System.Net;

namespace Shared.Extensions;

public static class StatusCodeExtension
{
    public static EResponseStatus FromStatusCode(this int statusCode) => (EResponseStatus)statusCode;
    public static int ToStatusCode(this EResponseStatus status) => (int)status;
    public static int ToStatusCode(this HttpStatusCode status) => (int)status;
    public static bool IsSuccessStatusCode(this int statusCode)
    {
        return statusCode >= 200 && statusCode <= 299;
    }
}