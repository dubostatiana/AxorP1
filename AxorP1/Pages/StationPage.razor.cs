using AxorP1.Class;
using AxorP1.Components;
using AxorP1.Shared.Components.Panels;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Layouts;

namespace AxorP1.Pages
{
    public class StationPageBase : MainComponent<StationPage>, IDisposable
    {
        [Parameter] public string id { get; set; } = string.Empty;

        private int num;
        protected Class.Station? Station { get; set; }

        protected SfDashboardLayout? DashboardLayout;
        protected bool IsDisposed = true;

        // List of Component references
        protected List<ComponentBase> componentsReferences = new List<ComponentBase>() { };
        protected ComponentBase? componentReference
        {
            set
            {
                if (value is not null) componentsReferences.Add(value);
            }
        }

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
            num = stationIdToNumber.TryGetValue(id.Trim(), out var result) ? result : 0;

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

               await RefreshDashboard();

            });

        }

        // Implement IDisposable
        public void Dispose()
        {
            Logger.LogInformation($"Station Dashboard disposed");
            RefProvider.StationDashboard = null;
            IsDisposed = true;
            DashboardLayout = null;
            componentsReferences.Clear();
        }
        
        // Dashboard event Created
        public async void Created(Object args)
        {
            Logger.LogInformation($"Station Dashboard created");
            RefProvider.StationDashboard = this;
            IsDisposed = false;

            await Task.Delay(500);
            await RefreshDashboard();
            // Initiate components that need to be notified to render
            PanelsStateChanged();
        }

        // Dashboard event OnWindowResize
        public async Task OnWindowResize(Syncfusion.Blazor.Layouts.ResizeArgs args)
        {
            await RefreshDashboard();
        }

        public async Task RefreshDashboard()
        {
            try
            {
                if (DashboardLayout != null && !IsDisposed)
                {
                    await DashboardLayout?.RefreshAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error trying to Refresh Station Dashboard : {ex.Message}\n{ex.StackTrace}");
            }
        }

        // Refresh the content of Dashboard panels
        public void RefreshPanel(int index)
        {
            if (componentsReferences.Count < 1 || IsDisposed) { return; }

            // Get the component instance corresponding to the panel
            var component = componentsReferences[index];

            // Check the type of the component and perform a refresh
            if (component is RangeComponent rangeComponent)
            {
                 rangeComponent?.Refresh();
            }
        }

        public async Task RefreshAllPanelsAsync()
        {
            await RefreshDashboard();

            await Task.Delay(500);

            // Iterate through all panels and refresh their content
            foreach (var panelNum in Enumerable.Range(0, componentsReferences.Count))
            {
                RefreshPanel(panelNum);
            }
        }

        // Call StateHasChange of Dashboard panels components
        public void PanelsStateChanged()
        {
            if (componentsReferences.Count < 1 || IsDisposed) { return; }

            // Iterate through all panels and call StateHasChanged()
            foreach (var component in componentsReferences)
            {
                if (component is RangeComponent rangeComponent)
                {
                    rangeComponent?.StateChanged();
                }
            }
        }

    }
}
