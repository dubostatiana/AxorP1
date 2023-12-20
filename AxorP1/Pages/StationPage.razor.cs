using System.Linq.Expressions;
using AxorP1.Components;
using AxorP1.Shared.Components.Panels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Layouts;

namespace AxorP1.Pages
{
    public class StationPageBase : MainComponent<StationPage>
    {
        [Parameter] public string id { get; set; } = string.Empty;

        private int num;
        protected Class.Station? Station { get; set; }

        protected SfDashboardLayout? DashboardLayout;
        protected RangeComponent? RangeComponent;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            Dictionary<string, int> stationIdToNumber = new Dictionary<string, int>
            {
                { "Chutes-à-Gorry", 1 },
                { "Jean-Guérin", 2 },
                { "Hydro-Canyon", 3 },
                { "Petites-Bergeronnes", 4 },
                { "Franquelin", 5 },
                { "Sheldrake", 6 },
                { "Long Rapids", 7 }
            };

            // Get the value associated with the key
            num = stationIdToNumber.TryGetValue(id.Trim(), out var result) ? result : 1;

            // Update the DataSource
            await NewDataAsync();
            // Update PastDataSource List with the data of the wanted station 
            await UpdatePastDataSourceAsync(num);

            // Starting live update of the Dashboard data every 5s
            timer.Elapsed += async (sender, e) => await NewDataAsync();
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private async Task NewDataAsync()
        {
            // Update the DataSource
            await UpdateDataSourceAsync();

            // Get Station object
            Station = DataSource.FirstOrDefault(obj => obj.StationName == $"Station {num}");

            await InvokeAsync(async () =>
            {
                // Re-render components
               StateHasChanged();

                if (DashboardLayout != null)
                {
                  await DashboardLayout.RefreshAsync();
                }
            });
            
        }


        // Dashboard event Created
        public void Created(Object args)
        {
            Logger.LogInformation($"StationPage Dashboard created");
            RangeComponent?.Refresh();
        }

        // Dashboard event OnWindowResize
        public async Task OnWindowResize(Syncfusion.Blazor.Layouts.ResizeArgs args)
        {
           
            await DashboardLayout?.RefreshAsync();
        }

    }
}
