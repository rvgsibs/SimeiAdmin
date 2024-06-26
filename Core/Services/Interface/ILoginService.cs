using SimeiAdmin.Core.Request;
using SimeiAdmin.Core.Response;

namespace SimeiAdmin.Core.Services.Interface;

public interface ILoginService
{
    Task<LoginResponse>? Login(LoginRequest loginRequest);
}
