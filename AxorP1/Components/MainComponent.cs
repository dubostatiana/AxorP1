using AxorP1.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AxorP1.Components
{
    public class MainComponent<T> : ComponentBase
    {
        [Inject] protected DataProvider DataProvider { get; set; }
        [Inject] protected ILogger<T> Logger { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

    }
}
