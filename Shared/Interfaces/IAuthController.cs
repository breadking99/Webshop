using Microsoft.AspNetCore.Mvc;
using Shared.Requests;

namespace Shared.Interfaces;

//! auth
public interface IAuthController
{
    //! login
    Task<ActionResult<string>> PostLoginAsync(LoginRequest request);
    //! register
    Task<ActionResult<string>> PostRegisterAsync(RegisterRequest request);
}