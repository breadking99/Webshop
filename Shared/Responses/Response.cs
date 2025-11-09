using Shared.Enums;
using Shared.Extensions;
using Shared.Interfaces;

namespace Shared.Responses;

public class Response : IResponse
{
    public Response() { }
    public Response(EResponseStatus status, string? message = null)
    {
        Status = status;
        Message = message;
    }

    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess => StatusCode.IsSuccessStatusCode();
    public EResponseStatus Status
    {
        get => StatusCode.FromStatusCode();
        set => StatusCode = value.ToStatusCode();
    }
}

public class Response<TValue> : Response, IResponse<TValue>
{
    public Response() { }
    public Response(EResponseStatus status, string? message = null) : base(status, message) { }
    public Response(TValue value)
    {
        Value = value;
        Status = EResponseStatus.Success;
    }

    public TValue? Value { get; set; }
}