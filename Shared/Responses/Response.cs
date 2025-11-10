using Shared.Enums;
using Shared.Extensions;
using Shared.Interfaces;

namespace Shared.Responses;

public class Response : IResponse
{
    #region Contructors
    public Response() { }
    public Response(EResponseStatus status, string? message = null) : this(status.ToStatusCode(), message) { }
    public Response(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message;
    }
    #endregion

    #region Properties
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess => StatusCode.IsSuccessStatusCode();
    public bool IsLoading => Status == EResponseStatus.Loading;
    public Func<Task<Response>>? Request { get; set; }
    // Callback to notify UI state changes
    public Action? Update { get; set; }
    public EResponseStatus Status
    {
        get => StatusCode.FromStatusCode();
        set => StatusCode = value.ToStatusCode();
    }
    #endregion

    #region Methods
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
    #endregion
}

public class Response<TValue> : Response, IResponse<TValue>
{
    #region Contructors
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
    #endregion

    #region Properties
    public TValue? Value { get; set; }

    private Func<Task<Response<TValue>>>? request;
    public new Func<Task<Response<TValue>>>? Request
    {
        get => request;
        set => SetRequest(value);
    }
    #endregion

    #region Methods
    public override async Task DoRequestAsync()
    {
        if (request == null) return;
        Status = EResponseStatus.Loading;
        Update?.Invoke();
        Response<TValue> response = await request();
        StatusCode = response.StatusCode;
        Message = response.Message;
        Value = response.Value;
        Update?.Invoke();
    }

    private void SetRequest(Func<Task<Response<TValue>>>? value)
    {
        request = value;
        base.Request = value is null
            ? null
            : async () => await value();
    }
    #endregion
}