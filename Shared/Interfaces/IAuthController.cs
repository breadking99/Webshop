using Microsoft.AspNetCore.Mvc;
using Shared.Requests;
using Shared.Responses;

namespace Shared.Interfaces;

//! auth
public interface IAuthController
{
    //! login
    Task<ActionResult<AuthData>> PostLoginAsync(AuthRequest request);
    //! register
    Task<ActionResult<AuthData>> PostRegisterAsync(AuthRequest request);
}