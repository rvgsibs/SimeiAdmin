﻿using SimeiAdmin.Core.Request;
using SimeiAdmin.Core.Response;
using SimeiAdmin.Core.Services.Interface;

namespace SimeiAdmin.Core.Services;

public class LoginService : ILoginService
{
    public LoginService()
    {

    }
    public async Task<LoginResponse>? Login(LoginRequest loginRequest)
    {
        if (loginRequest.Usuario == "admin" && loginRequest.Senha == "123")
        {
            var retorno = new LoginResponse
            {
                Error = "",
                Expiration = DateTime.Now.AddMinutes(15).ToString(),
                NomeUsuario = loginRequest.Usuario,
                Token = "1234567890asdfghjklpoiuytrewqmnbvcxz09876543210"
            };

            return retorno;
        }
        return null;
    }
}