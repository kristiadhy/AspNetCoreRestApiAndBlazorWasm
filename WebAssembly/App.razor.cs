using Microsoft.AspNetCore.Components;
using Services.IRepositories;

namespace WebAssembly;

public partial class App : IDisposable
{
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        ServiceManager.InterceptorService.RegisterEvent();
    }

    public void Dispose()
    {
        ServiceManager.InterceptorService.DisposeEvent();
    }
}
