using System.Linq.Expressions;
using AxorP1.Components;
using AxorP1.Shared.Components.Panels;
using Microsoft.AspNetCore.Components;
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

            await NewDataAsync();

            Dictionary<string, int> stationIdToNumber = new Dictionary<string, int>
            {
                { "Station 1", 1 },
                { "Station 2", 2 },
                { "Station 3", 3 },
                { "Station 4", 4 },
                { "Station 5", 5 },
                { "Station 6", 6 },
                { "Station 7", 7 }
            };

            // Get the value associated with the key
            num = stationIdToNumber.TryGetValue(id.Trim(), out var result) ? result : 1;

            // Update PastDataSource List with the data of the wanted station 
            await UpdatePastDataSourceAsync(num);

            // Starting live update of the Dashboard data every 5s
            timer.Elapsed += async (sender, e) => await NewDataAsync();
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private async Task NewDataAsync()
        {
            // Update the data source
            await UpdateDataSourceAsync();

            // Get Station object
            Station = DataSource.FirstOrDefault(obj => obj.StationName == id.Trim());

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
