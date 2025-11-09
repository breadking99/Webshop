using Shared.Requests;
using Shared.Responses;

namespace Shared.Interfaces;

//! auth
public interface IAuthService
{
    //! login
    Task<Response<string>> PostLoginAsync(LoginRequest request);
    //! register
    Task<Response<string>> PostRegisterAsync(RegisterRequest request);
}