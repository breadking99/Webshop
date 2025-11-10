using Shared.Requests;
using Shared.Responses;

namespace Shared.Interfaces;

//! auth
public interface IAuthService
{
    //! login
    Task<Response<AuthData>> PostLoginAsync(AuthRequest request);
    //! register
    Task<Response<AuthData>> PostRegisterAsync(AuthRequest request);
}