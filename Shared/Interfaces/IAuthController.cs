using Microsoft.AspNetCore.Mvc;
using Shared.Requests;
using Shared.Responses;

namespace Shared.Interfaces;

//! auth
public interface IAuthController
{
    //! login
    Task<ActionResult<AuthData>> PostLoginAsync(LoginRequest request);
    //! register
    Task<ActionResult<AuthData>> PostRegisterAsync(RegisterRequest request);
}