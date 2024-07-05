using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SimeiAdmin.Core.Extensions;
using SimeiAdmin.Core.Request;
using SimeiAdmin.Core.Response;
using SimeiAdmin.Core.Services.Interface;
using System.Net.Http.Headers;

namespace SimeiAdmin.Core.Services;

public sealed class AuthService : IAuthService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

    public AuthService(IHttpClientFactory httpClientFactory,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _httpClient = httpClientFactory.CreateClient("Api");
    }

    public async Task<LoginResponse> Login(LoginRequest loginModel)
    {
        var response = await _httpClient.PostAsync("api/Login", loginModel.ToStringContent());

        if (!response.IsSuccessStatusCode)
            throw new Exception("Usuário/senha inválidos.");

        var loginResult = await response.ToDeserializeAsync<LoginResponse>();

        await _localStorage.SetItemAsync("authToken", loginResult.Token);
        await _localStorage.SetItemAsync("tokenExpiration", loginResult.Expiration);
        await _localStorage.SetItemAsync("nomeUsuario", loginResult.NomeUsuario);

        ((ApiAuthenticationStateProvider)_authenticationStateProvider)
                            .MarkUserAsAuthenticated(loginModel.Usuario);

        _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("bearer",
                                                     loginResult.Token);

        return loginResult;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");

        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string> NomeUsuario()
    {
        var nome = await _localStorage.GetItemAsync<string>("nomeUsuario");

        return nome ?? "Administrador";


    }

}
