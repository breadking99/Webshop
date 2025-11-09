using Api.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Interfaces;

public interface IAuthController
{
    //! login
    Task<ActionResult<string>> PostLoginAsync(LoginRequest request);
    //! register
    Task<ActionResult<string>> PostRegisterAsync(RegisterRequest request);

}