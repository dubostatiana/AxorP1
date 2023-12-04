using Timer = System.Timers.Timer;
using Syncfusion.Blazor.Layouts;
using AxorP1.Class;
using Microsoft.AspNetCore.Http.HttpResults;
using AxorP1.Components.Charts;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts;
using static AxorP1.Components.Charts.ChartComponent;
using Microsoft.AspNetCore.Components;
using AxorP1.Services;
using System.Runtime.CompilerServices;

namespace AxorP1.Components.Pages
{
	public class IndexBase : MainComponent<Index>
	{
		private static Timer timer = new Timer(5000);

		protected SfDashboardLayout DashboardLayout { get; set; }
       
        protected int refreshCounter = 0;

        public string MediaQuery { get { return "max-width:" + MaxWidth + "px"; } }
		public int MaxWidth = 800;
		public int Columns = 4;


        protected List<PanelObject> PanelData { get; set; } = new List<PanelObject>();
       

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            await NewDataAsync();

            // Starting live update of the Dashboard data every 5s
            timer.Elapsed += async (sender, e) => await NewDataAsync();
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void Created(Object args)
        {
            Logger.LogInformation($"Dashboard created");
        }

        private async Task NewDataAsync()
        {
            await UpdateDataSourceAsync();

            PanelData = new List<PanelObject>()
            {
                new PanelObject() { Id = "panelTotalProduction", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Production centrale", ComponentType = typeof(ChartComponent),
                    Parameters = new Dictionary<string, object>
                    {
                        { "ChartId", "chartTotalProduction" },
                        { "ChartTheme", AppTheme },
                        { "XAxisAttributes", new Dictionary<string, object>
                            {
                                {"ValueType", Syncfusion.Blazor.Charts.ValueType.Category },
                            }
                        },
                        { "YAxisAttributes", new Dictionary<string, object>
                            {
                                {"ValueType", Syncfusion.Blazor.Charts.ValueType.Double },
                                {"Title", "mW"},
                            }
                        },
                        { "ToolTipAttributes", new Dictionary<string, object>
                            {
                                {"Enable", true },
                                {"Format", "${point.x} : <b>${point.y} mW</b>" },
                            }
                        },
                        { "ChartSeriesList", new List<ChartSeriesConfig>
                            {
                                new ChartSeriesConfig()
                                {
                                    SeriesAttributes = new Dictionary<string, object>
                                    {
                                        {"DataSource", DataSource },
                                        {"XName", "StationName" },
                                        {"YName", "CentralProduction" },
                                        {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Spline },
                                    },
                                    LabelAttributes  = new Dictionary<string, object>
                                    {
                                        { "Visible", true },
                                        { "Position", Syncfusion.Blazor.Charts.LabelPosition.Bottom },
                                    }
                                },
                            }
                        }
                    }
                },
                new PanelObject() { Id = "panelGroupProduction", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Production centrale", ComponentType = typeof(ChartComponent),
                    Parameters = new Dictionary<string, object>
                    {
                        { "ChartId", "chartGroupProduction" },
                        { "ChartTheme", AppTheme },
                        { "XAxisAttributes", new Dictionary<string, object>
                            {
                                {"ValueType", Syncfusion.Blazor.Charts.ValueType.Category },
                            }
                        },
                        { "YAxisAttributes", new Dictionary<string, object>
                            {
                                {"ValueType", Syncfusion.Blazor.Charts.ValueType.Double },
                                {"Title", "mW"},
                            }
                        },
                        { "ToolTipAttributes", new Dictionary<string, object>
                            {
                                {"Enable", true },
                                {"Format", "${point.x} : <b>${point.y} mW</b>" },
                            }
                        },
                        { "ChartSeriesList", new List<ChartSeriesConfig>
                            {
                                new ChartSeriesConfig()
                                {
                                    SeriesAttributes = new Dictionary<string, object>
                                    {
                                        {"DataSource", DataProvider.GetAllGroups(DataSource).Where(d => d.GroupName == "Group 1") },
                                        {"XName", "StationName" },
                                        {"YName", "Production" },
                                        {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Spline },
                                        {"Name", "Groupe 1" }
                                    },
                                    LabelAttributes  = new Dictionary<string, object>
                                    {
                                        { "Visible", true },
                                        { "Position", Syncfusion.Blazor.Charts.LabelPosition.Bottom },
                                    }
                                },
                                new ChartSeriesConfig()
                                {
                                    SeriesAttributes = new Dictionary<string, object>
                                    {
                                        {"DataSource", DataProvider.GetAllGroups(DataSource).Where(d => d.GroupName == "Group 2") },
                                        {"XName", "StationName" },
                                        {"YName", "Production" },
                                        {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Spline },
                                        {"Name", "Groupe 2" }
                                    },
                                    LabelAttributes  = new Dictionary<string, object>
                                    {
                                        { "Visible", true },
                                        { "Position", Syncfusion.Blazor.Charts.LabelPosition.Bottom },
                                    }
                                },
                            }
                        }
                    }
                },

            };

            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
            
        }

        public void Destroyed(Object args)
        {

        }

        public void OnResizeStop(Syncfusion.Blazor.Layouts.ResizeArgs args)
        {
            refreshCounter++;
        }

        public async Task OnWindowResize (Syncfusion.Blazor.Layouts.ResizeArgs args)
        {
            refreshCounter++;
        }
    }
}
