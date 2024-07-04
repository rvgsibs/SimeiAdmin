﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using SimeiAdmin.Core.Request;
using SimeiAdmin.Core.Services.Interface;

namespace SimeiAdmin.Pages.Autenticacao;

public partial class LoginPage : ComponentBase
{
    #region Properties
    public LoginRequest InputRequest { get; set; } = new();
    public bool ShowErrors { get; set; } = false;
    public bool Autenticando { get; set; } = false;
    public string Error { get; set; } = "";


    #endregion

    #region Services
    [Inject]
    public IAuthService AuthService { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar SnackbarService { get; set; } = null!;

    #endregion

    #region Methods
    public async Task OnValidSubmitAsync()
    {
        try
        {
            if (ValidarDados())
            {
                var result = await AuthService.Login(InputRequest);

                if (result is not null && result.Token is not null)
                    LoginEfetuado();
                else
                    ExibirMensagem("Não foi possível fazer o Login : Usuário e/ou senha inválidos.");
            }
        }
        catch (Exception ex)
        {
            ExibirMensagem($"Não foi possível fazer o Login : {ex.Message}");
        }
    }

    private bool ValidarDados()
    {
        if (string.IsNullOrWhiteSpace(InputRequest.Usuario) || string.IsNullOrWhiteSpace(InputRequest.Senha))
        {
            ExibirMensagem("Não foi possível fazer o Login : Favor preencher usuário e senha.", Severity.Warning);
            return false;
        }
        return true;
    }

    private void ExibirMensagem(string mensagem, Severity serverity = Severity.Error)
    {
        SnackbarService.RemoveByKey("Login");
        var config = (SnackbarOptions options) =>
        {
            options.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
        };

        SnackbarService.Add(mensagem, serverity, configure: config, key: "Login");
    }

    private void LoginEfetuado()
    {
        SnackbarService.RemoveByKey("Login");
        NavigationManager.NavigateTo("/");
    }

    #endregion
}
