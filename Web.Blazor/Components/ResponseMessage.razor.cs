using Shared.Enums;
using Microsoft.AspNetCore.Components;
using Shared.Responses;

namespace Web.Blazor.Components;

public partial class ResponseMessage
{
    [Parameter] public Response? Response { get; set; }
    private EResponseStatus? Status => Response?.Status;
    private string? Message => Response?.Message;
    private bool IsVisible => Status is not null && !string.IsNullOrWhiteSpace(Message);
    private string Class => (Status ?? EResponseStatus.Default) switch
    {
        EResponseStatus.Ok => "alert alert-success",
        EResponseStatus.TimeOut or EResponseStatus.NotFound => "alert alert-warning",
        EResponseStatus.Unauthorized or EResponseStatus.BadRequest or EResponseStatus.Forbid => "alert alert-danger",
        EResponseStatus.InternalServerError => "alert alert-danger",
        _ => "alert alert-secondary"
    };
}