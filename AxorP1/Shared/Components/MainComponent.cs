using AxorP1.Class;
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

        // DataSource List
        protected List<Station> DataSource = new List<Station>();
        protected List<Station> PastDataSource = new List<Station>();

        protected static Syncfusion.Blazor.Theme AppTheme { get; set; } = Syncfusion.Blazor.Theme.Fluent;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Initialize DataSource List
            if (DataSource.Count == 0)
            {
                await UpdateDataSourceAsync();
            }
        }


        // Update data source asynchronously
        protected async Task UpdateDataSourceAsync()
        {
            try
            {
                DataSource = await DataProvider.GetDataAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error Data source assignment: {ex.Message} {ex.StackTrace}");
            }
        }

        // Update past data source asynchronously
        protected async Task UpdatePastDataSourceAsync(int id)
        {
            try
            {
                PastDataSource = await DataProvider.GetPastDataAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error Past Data source assignment: {ex.Message} {ex.StackTrace}");
            }
        }
    }
}
