using Timer = System.Timers.Timer;
using Syncfusion.Blazor.Layouts;
using AxorP1.Class;
using Microsoft.AspNetCore.Http.HttpResults;
using AxorP1.Components.Charts;

namespace AxorP1.Components.Pages
{
	public class IndexBase : MainComponent<Index>
	{
		private static Timer timer = new Timer(5000);

		protected SfDashboardLayout DashboardLayout { get; set; }

		public string MediaQuery { get { return "max-width:" + MaxWidth + "px"; } }
		public int MaxWidth = 800;
		public int Columns = 4;

		
        protected List<PanelObject> PanelData { get; set; } = new List<PanelObject>()
        {
            new PanelObject() { Id = "panelTotalProduction",        Column = 0, Row = 0, SizeX = 2, SizeY = 2, Title = "Production centrale", ComponentType = typeof(ChartComponent)},
         
        };
       

        protected override void OnInitialized()
        {

        }

        public void Created(Object args)
        {
            Logger.LogInformation($"Dashboard created");

            // Starting live update of the Dashboard data every 5s
            timer.Elapsed += async (sender, e) => await NewDataAsync();
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private async Task NewDataAsync()
        {
            await DashboardLayout.RefreshAsync();
        }

        public void Destroyed(Object args)
        {

        }

        public void OnResizeStop(Syncfusion.Blazor.Layouts.ResizeArgs args){ 

        }

        public async Task OnWindowResize (Syncfusion.Blazor.Layouts.ResizeArgs args)
        {

        }
    }
}
