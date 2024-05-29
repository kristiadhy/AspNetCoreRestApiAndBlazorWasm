using Microsoft.AspNetCore.Components;
using Services.IRepositories;

namespace WebAssembly.Components;

public partial class AuthLink
{
    [Inject]
    NavigationManager NavigationHelper { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;

    private void Logout()
    {
        ServiceManager.AuthService.Logout();
        NavigationHelper.NavigateTo($"/login");
    }
}
