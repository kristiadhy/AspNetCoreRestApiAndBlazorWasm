using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Services.IRepositories;

namespace WebAssembly.Pages;

public partial class Login
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;
    bool AlertVisible = false;
    string ErrorMessage = string.Empty;

    async Task OnLogin(LoginArgs args)
    {
        UserAuthenticationDTO userDTO = new(args.Username, args.Password);
        try
        {
            TokenDTO tokenResponse = await ServiceManager.AuthService.Login(userDTO);
        }
        catch (Exception ex)
        {
            AlertVisible = true;
            ErrorMessage = "Wrong username/password";
            return;
        }

        AlertVisible = false;
        NavigationManager.NavigateTo($"/");
    }

    void OnRegister()
    {
        //Console.WriteLine($"{name} -> Register");
    }

    void OnResetPassword()
    {
        //Console.WriteLine($"{name} -> ResetPassword for user: {value}");
    }
}
