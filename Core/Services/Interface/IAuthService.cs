using SimeiAdmin.Core.Request;
using SimeiAdmin.Core.Response;

namespace SimeiAdmin.Core.Services.Interface;

public interface IAuthService
{
    Task<LoginResponse>? Login(LoginRequest loginRequest);

    Task Logout();
}
