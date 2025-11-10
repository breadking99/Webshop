using Shared.Enums;
using Shared.Extensions;
using Shared.Interfaces;

namespace Shared.Responses;

public class Response : IResponse
{
    public Response() { }
    public Response(EResponseStatus status, string? message = null) : this(status.ToStatusCode(), message) { }
    public Response(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess => StatusCode.IsSuccessStatusCode();
    public bool IsLoading => Status == EResponseStatus.Loading;
    public EResponseStatus Status
    {
        get => StatusCode.FromStatusCode();
        set => StatusCode = value.ToStatusCode();
    }
    public Action? Update { get; set; }
    public Func<Task<Response>>? Request { get; set; }

    public virtual async Task DoRequestAsync()
    {
        if (Request == null) return;
        Status = EResponseStatus.Loading;
        Update?.Invoke();
        Response response = await Request();
        StatusCode = response.StatusCode;
        Message = response.Message;
        Update?.Invoke();
    }
}

public class Response<TValue> : Response, IResponse<TValue>
{
    public Response() { }
    public Response(int statusCode, string? message = null) : base(statusCode, message) { }
    public Response(EResponseStatus status, string? message = null) : base(status, message) { }
    public Response(Response response) : base(response.StatusCode, response.Message) { }
    public Response(Response<TValue> response) : base(response.StatusCode, response.Message)
    {
        Value = response.Value;
    }
    public Response(TValue value)
    {
        Value = value;
        Status = EResponseStatus.Ok;
    }

    public TValue? Value { get; set; }
    public new Func<Task<Response<TValue>>>? Request { get; set; }

    public override async Task DoRequestAsync()
    {
        if (Request == null) return;
        Status = EResponseStatus.Loading;
        Update?.Invoke();
        Response<TValue> response = await Request();
        StatusCode = response.StatusCode;
        Message = response.Message;
        Value = response.Value;
        Update?.Invoke();
    }
}