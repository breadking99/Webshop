using Shared.Enums;

namespace Shared.Interfaces;

public interface IResponse
{
    int StatusCode { get; set; }
    string? Message { get; set; }
    bool IsSuccess { get; }
    EResponseStatus Status { get; set; }
}

public interface IResponse<TValue> : IResponse
{
    TValue? Value { get; set; }
}