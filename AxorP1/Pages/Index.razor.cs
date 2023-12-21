using Timer = System.Timers.Timer;
using Syncfusion.Blazor.Layouts;
using AxorP1.Class;
using AxorP1.Shared.Components.Panels;
using static AxorP1.Shared.Components.Panels.ChartComponent;
using static AxorP1.Shared.Components.Panels.GridComponent<AxorP1.Class.Station>;
using static AxorP1.Shared.Components.Panels.PieChartComponent;
using AxorP1.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Maps;
using AxorP1.Components;
using System.Runtime.CompilerServices;


namespace AxorP1.Pages
{
    public class IndexBase : MainComponent<Index>, IDisposable
    {

        protected List<PanelObject> PanelData { get; set; } = new List<PanelObject>(); // List of Dashboard Panels 

        // DashboardLayout attributs
        protected SfDashboardLayout? DashboardLayout;
        
        public int Columns = 4;
        public static double[] Spacing = new double[] { 10, 10 };
        public double Ratio = 160 / 100;

        // List of DynamicComponent references
        protected List<DynamicComponent> componentsReferences = new List<DynamicComponent>();
        protected DynamicComponent? componentReference {
            set
            {
                if (value is not null) componentsReferences.Add(value);
            } 
        }

        protected bool IsStacked { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Initialize data and panel objects
            await NewDataAsync();

            // Starting live update of the Dashboard data every 5s
            timer.Elapsed += async (sender, e) => await NewDataAsync();
            timer.AutoReset = true;
            timer.Enabled = true;
        }


        private async Task NewDataAsync()
        {
            // Update the data source
            await UpdateDataSourceAsync();

            // Update the panel objects
            InitializedPanelData();

            // Re-render components
            if(DashboardLayout != null) 
            {
                await DashboardLayout.RefreshAsync();
            }
        }

        // Implement IDisposable
        public void Dispose()
        {
            
        }

        // Dashboard event Created
        public async void Created(Object args)
        {
            Logger.LogInformation($"Dashboard created");

            await IsLayoutStackedAsync();
           
            RefreshPanel("panelPieChart");
            RefreshPanel("panelMap");
        }


        // Dashboard event OnResizeStop
        public void OnResizeStop(Syncfusion.Blazor.Layouts.ResizeArgs args)
        {
            RefreshPanel(args.Id);
        }

        // Dashboard event OnWindowResize
        public async Task OnWindowResize(Syncfusion.Blazor.Layouts.ResizeArgs args)
        {
            await IsLayoutStackedAsync();

            await Task.Delay(500);
            RefreshPanel("panelMap");
            DashboardLayout?.RefreshAsync();
        }

        // Refresh the content of a panel
        public void RefreshPanel(string id)
        {
            // Avoid IndexOutOfBound exception
            if (PanelData.Count != componentsReferences.Count) { return; }

            // Find the index of the panel 
            int index = PanelData.FindIndex(obj => obj.Id == id);

            if (index != -1)
            {
                // Get the component instance corresponding to the panel
                var component = componentsReferences[index].Instance;

                // Check the type of the component and perform a refresh
                if (component is ChartComponent chartComponent)
                {
                    chartComponent.Refresh();
                }
                else if (component is GridComponent<Station> gridComponent)
                {
                    gridComponent.Refresh();
                }
                else if (component is MapComponent<StationMapData> mapComponent)
                {
                    mapComponent.Refresh();
                }
                else if (component is PieChartComponent pieComponent)
                {
                    pieComponent.Refresh();
                }
            }
        }

        // Verify if the DashboardLayout panels are stacked
        public async Task IsLayoutStackedAsync()
        {
            double width = await JSRuntime.InvokeAsync<double>("getWidth");
            IsStacked = (width <= MaxWidth) ? true : false;

            if (IsStacked)
            {
                StateHasChanged();
            }
        }

        // Map Marker OnClick event
        public void OnMarkerClickEvent(MarkerClickEventArgs args)
        {
            try
            {
                if (args.Data.TryGetValue("Name", out var station))
                {
                    // Navigate to station page

                    NavigationManager.NavigateTo($"station/{station}");
                    Logger.LogInformation($"Navigate to {station} page");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error navigating to station page: {ex.Message} {ex.StackTrace}");
            }
        }


        // Initialize the dashboard panel objects
        public void InitializedPanelData()
        {
            PanelData = new List<PanelObject>()
            {
                // TOTAL PRODUCTION
                 new PanelObject() { Id = "panelTotalProduction", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Production Centrale", ComponentType = typeof(ChartComponent),
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
                                         {"XName", nameof(Station.StationName) },
                                         {"YName", nameof(Station.CentralProduction) },
                                         {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Spline },
                                     },
                                 },
                             }
                         }
                     }
                 }, 
                 // GROUPS PRODUCTION
                 new PanelObject() { Id = "panelGroupProduction", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Production des Groupes", ComponentType = typeof(ChartComponent),
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
                                         {"DataSource", DataProvider.GetAllGroups(DataSource, "Group 1") },
                                         {"XName", nameof(Group.StationName) },
                                         {"YName", nameof(Group.Production) },
                                         {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Spline },
                                         {"Name", "Groupe 1" }
                                     }
                                 },
                                 new ChartSeriesConfig()
                                 {
                                     SeriesAttributes = new Dictionary<string, object>
                                     {
                                         {"DataSource", DataProvider.GetAllGroups(DataSource, "Group 2") },
                                         {"XName", nameof(Group.StationName) },
                                         {"YName", nameof(Group.Production) },
                                         {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Spline },
                                         {"Name", "Groupe 2" }
                                     },
                                 },
                             }
                         }
                     }
                 },
                 // HAUTEUR DE CHUTE
                  new PanelObject() { Id = "panelFallHeight", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Hauteur de Chute", ComponentType = typeof(ChartComponent),
                     Parameters = new Dictionary<string, object>
                     {
                         { "ChartId", "chartFallHeight" },
                         { "ChartTheme", AppTheme },
                         { "XAxisAttributes", new Dictionary<string, object>
                             {
                                 {"ValueType", Syncfusion.Blazor.Charts.ValueType.Category },
                             }
                         },
                         { "YAxisAttributes", new Dictionary<string, object>
                             {
                                 {"ValueType", Syncfusion.Blazor.Charts.ValueType.Double },
                                 {"Title", "m"},
                             }
                         },
                         { "ToolTipAttributes", new Dictionary<string, object>
                             {
                                 {"Enable", true },
                                 {"Format", "${point.x} : <b>${point.y} m</b>" },
                             }
                         },
                         { "ChartSeriesList", new List<ChartSeriesConfig>
                             {
                                 new ChartSeriesConfig()
                                 {
                                     SeriesAttributes = new Dictionary<string, object>
                                     {
                                         {"DataSource", DataSource },
                                         {"XName", nameof(Station.StationName) },
                                         {"YName", nameof(Station.FallHeight) },
                                         {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.StepLine },
                                     },
                                 },
                             }
                         }
                     }
                 }, 
                  // DIFFERENTIEL DE GRILLE
                   new PanelObject() { Id = "panelGridDifferential", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Différentiel de Grille", ComponentType = typeof(ChartComponent),
                         Parameters = new Dictionary<string, object>
                         {
                             { "ChartId", "chartGridDifferential" },
                             { "ChartTheme", AppTheme },
                             { "XAxisAttributes", new Dictionary<string, object>
                                 {
                                     {"ValueType", Syncfusion.Blazor.Charts.ValueType.Category },
                                 }
                             },
                             { "YAxisAttributes", new Dictionary<string, object>
                                 {
                                     {"ValueType", Syncfusion.Blazor.Charts.ValueType.Double },
                                     {"Title", "m"},
                                 }
                             },
                             { "ToolTipAttributes", new Dictionary<string, object>
                                 {
                                     {"Enable", true },
                                     {"Format", "${point.x} : <b>${point.y} m</b>" },
                                 }
                             },
                             { "ChartSeriesList", new List<ChartSeriesConfig>
                                 {
                                     new ChartSeriesConfig()
                                     {
                                         SeriesAttributes = new Dictionary<string, object>
                                         {
                                             {"DataSource", DataProvider.GetAllGroups(DataSource, "Group 1") },
                                             {"XName", nameof(Group.StationName) },
                                             {"YName", nameof(Group.FineGridDifferential) },
                                             {"Name", "Groupe 1 - Fines" },
                                             {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Column },
                                             {"ColumnSpacing", 0.1 }
                                         },
                                     },
                                     new ChartSeriesConfig()
                                     {
                                         SeriesAttributes = new Dictionary<string, object>
                                         {
                                             {"DataSource", DataProvider.GetAllGroups(DataSource, "Group 1") },
                                             {"XName", nameof(Group.StationName) },
                                             {"YName", nameof(Group.CoarseGridDifferential) },
                                             {"Name", "Groupe 1 - Grossières" },
                                             {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Column },
                                             {"ColumnSpacing", 0.1 }
                                         },
                                     },
                                     new ChartSeriesConfig()
                                     {
                                         SeriesAttributes = new Dictionary<string, object>
                                         {
                                             {"DataSource", DataProvider.GetAllGroups(DataSource, "Group 2") },
                                             {"XName", nameof(Group.StationName) },
                                             {"YName", nameof(Group.FineGridDifferential) },
                                             {"Name", "Groupe 2 - Fines" },
                                             {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Column },
                                             {"ColumnSpacing", 0.1 }
                                         },
                                     },
                                     new ChartSeriesConfig()
                                     {
                                        SeriesAttributes = new Dictionary<string, object>
                                        {
                                            {"DataSource", DataProvider.GetAllGroups(DataSource, "Group 2") },
                                            {"XName", nameof(Group.StationName) },
                                            {"YName", nameof(Group.CoarseGridDifferential) },
                                            {"Name", "Groupe 2 - Grossières" },
                                            {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Column },
                                            {"ColumnSpacing", 0.1 }
                                        },
                                     },

                                 }
                             }
                         }
                   },  
                   // DÉBIT TOTAL
                    new PanelObject() { Id = "panelTotalFlowRate", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Débit Total", ComponentType = typeof(ChartComponent),
                         Parameters = new Dictionary<string, object>
                         {
                             { "ChartId", "chartTotalFlowRate" },
                             { "ChartTheme", AppTheme },
                             { "XAxisAttributes", new Dictionary<string, object>
                                 {
                                     {"ValueType", Syncfusion.Blazor.Charts.ValueType.Category },
                                 }
                             },
                             { "YAxisAttributes", new Dictionary<string, object>
                                 {
                                     {"ValueType", Syncfusion.Blazor.Charts.ValueType.Double },
                                     {"Title", "m²/sec"},
                                 }
                             },
                             { "ToolTipAttributes", new Dictionary<string, object>
                                 {
                                     {"Enable", true },
                                     {"Format", "${point.x} : <b>${point.y} m²/s</b>" },
                                 }
                             },
                             { "ChartSeriesList", new List<ChartSeriesConfig>
                                 {
                                     new ChartSeriesConfig()
                                     {
                                         SeriesAttributes = new Dictionary<string, object>
                                         {
                                             {"DataSource", DataSource },
                                             {"XName", nameof(Station.StationName) },
                                             {"YName", nameof(Station.TotalFlowRate) },
                                             {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Column },
                                         },
                                     },
                                 }
                             }
                         }
                   }, 
                    // DÉBIT DES GROUPES
                    new PanelObject() { Id = "panelGroupFlowRate", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Débit des Groupes", ComponentType = typeof(ChartComponent),
                         Parameters = new Dictionary<string, object>
                         {
                             { "ChartId", "chartGroupFlowRate" },
                             { "ChartTheme", AppTheme },
                             { "XAxisAttributes", new Dictionary<string, object>
                                 {
                                     {"ValueType", Syncfusion.Blazor.Charts.ValueType.Category },
                                 }
                             },
                             { "YAxisAttributes", new Dictionary<string, object>
                                 {
                                     {"ValueType", Syncfusion.Blazor.Charts.ValueType.Double },
                                     {"Title", "m²/sec"},
                                 }
                             },
                             { "ToolTipAttributes", new Dictionary<string, object>
                                 {
                                     {"Enable", true },
                                     {"Format", "${point.x} : <b>${point.y} m²/s</b>" },
                                 }
                             },
                             { "ChartSeriesList", new List<ChartSeriesConfig>
                                 {
                                     new ChartSeriesConfig()
                                     {
                                         SeriesAttributes = new Dictionary<string, object>
                                         {
                                             {"DataSource", DataProvider.GetAllGroups(DataSource, "Group 1") },
                                             {"XName", nameof(Group.StationName) },
                                             {"YName", nameof(Group.FlowRate) },
                                             {"Name", "Groupe 1" },
                                             {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.StackingColumn },
                                         },
                                     },
                                     new ChartSeriesConfig()
                                     {
                                         SeriesAttributes = new Dictionary<string, object>
                                         {
                                             {"DataSource", DataProvider.GetAllGroups(DataSource, "Group 2") },
                                             {"XName", nameof(Group.StationName) },
                                             {"YName", nameof(Group.FlowRate) },
                                             {"Name", "Groupe 2" },
                                             {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.StackingColumn },
                                         },
                                     },
                                 }
                             }
                         }
                    }, 
                    // NIVEAU AMONT ET AVAL
                     new PanelObject() { Id = "panelStreamLevel", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Niveau Amont et Aval", ComponentType = typeof(ChartComponent),
                          Parameters = new Dictionary<string, object>
                          {
                              { "ChartId", "chartStreamLevel" },
                              { "ChartTheme", AppTheme },
                              { "XAxisAttributes", new Dictionary<string, object>
                                  {
                                      {"ValueType", Syncfusion.Blazor.Charts.ValueType.Category },
                                  }
                              },
                              { "YAxisAttributes", new Dictionary<string, object>
                                  {
                                      {"ValueType", Syncfusion.Blazor.Charts.ValueType.Double },
                                      {"Title", "m"},
                                  }
                              },
                              { "ToolTipAttributes", new Dictionary<string, object>
                                  {
                                      {"Enable", true },
                                      {"Format", "${point.x} : <b>${point.y} m</b>" },
                                  }
                              },
                              { "ChartSeriesList", new List<ChartSeriesConfig>
                                  {
                                      new ChartSeriesConfig()
                                      {
                                          SeriesAttributes = new Dictionary<string, object>
                                          {
                                              {"DataSource", DataSource },
                                              {"XName", nameof(Station.StationName) },
                                              {"YName", nameof(Station.UpstreamLevel) },
                                              {"Name", "Amont" },
                                              {"ColumnSpacing", 0.1 },
                                              {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Column },
                                          },
                                      },
                                      new ChartSeriesConfig()
                                      {
                                          SeriesAttributes = new Dictionary<string, object>
                                          {
                                              {"DataSource", DataSource },
                                              {"XName", nameof(Station.StationName) },
                                              {"YName", nameof(Station.DownstreamLevel) },
                                              {"Name", "Aval" },
                                              {"ColumnSpacing", 0.1 },
                                              {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.Column },
                                          },
                                      },
                                  }
                              }
                          }
                     },
                     // AVANCEMENT PRODUCTION MENSUELLE 
                     new PanelObject() { Id = "panelTargetProductionMonth", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Avancement Production Mensuelle", ComponentType = typeof(ChartComponent),
                          Parameters = new Dictionary<string, object>
                          {
                              { "ChartId", "chartTargetProductionMonth" },
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
                                      {"Format", "${series.name} : <b>${point.y} mW</b>" },
                                      {"Shared", true },
                                  }
                              },
                              { "ChartSeriesList", new List<ChartSeriesConfig>
                                  {
                                      new ChartSeriesConfig()
                                      {
                                          SeriesAttributes = new Dictionary<string, object>
                                          {
                                              {"DataSource", DataSource },
                                              {"XName", nameof(Station.StationName) },
                                              {"YName", nameof(Station.MonthlyProductionTarget) },
                                              {"Name", "Objectif Mensuel" },
                                              {"Opacity", 0.5 },
                                              {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.SplineArea },
                                          },
                                          MarkerAttributes = new Dictionary<string, object>
                                          {
                                              { "Visible", false }
                                          },
                                          LabelAttributes = new Dictionary<string, object>
                                          {
                                              { "Visible", false },
                                          },
                                      },
                                      new ChartSeriesConfig()
                                      {
                                          SeriesAttributes = new Dictionary<string, object>
                                          {
                                              {"DataSource", DataSource },
                                              {"XName", nameof(Station.StationName) },
                                              {"YName", nameof(Station.MonthlyProductionActual) },
                                              {"Name", "Réalisation Mensuelle" },
                                              {"Opacity", 0.5 },
                                              {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.SplineArea },
                                          },
                                          MarkerAttributes = new Dictionary<string, object>
                                          {
                                              { "Visible", false }
                                          },
                                          LabelAttributes = new Dictionary<string, object>
                                          {
                                              { "Visible", false },
                                          },
                                      },
                                   }
                              }
                          }
                     },
                     // AVANCEMENT PRODUCTION ANUELLE 
                     new PanelObject() { Id = "panelTargetProductionYear", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Avancement Production Annuelle", ComponentType = typeof(ChartComponent),
                          Parameters = new Dictionary<string, object>
                          {
                              { "ChartId", "chartTargetProductionYear" },
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
                                      {"Format", "${series.name} : <b>${point.y} mW</b>" },
                                      {"Shared", true },
                                  }
                              },
                              { "ChartSeriesList", new List<ChartSeriesConfig>
                                  {
                                      new ChartSeriesConfig()
                                      {
                                          SeriesAttributes = new Dictionary<string, object>
                                          {
                                              {"DataSource", DataSource },
                                              {"XName", nameof(Station.StationName) },
                                              {"YName", nameof(Station.AnnualProductionTarget) },
                                              {"Name", "Objectif Annuel" },
                                              {"Opacity", 0.5 },
                                              {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.SplineArea },
                                          },
                                          MarkerAttributes = new Dictionary<string, object>
                                          {
                                              { "Visible", false }
                                          },
                                          LabelAttributes = new Dictionary<string, object>
                                          {
                                              { "Visible", false },
                                          },
                                      },
                                      new ChartSeriesConfig()
                                      {
                                          SeriesAttributes = new Dictionary<string, object>
                                          {
                                              {"DataSource", DataSource },
                                              {"XName", nameof(Station.StationName) },
                                              {"YName", nameof(Station.AnnualProductionActual) },
                                              {"Name", "Réalisation Annuelle" },
                                              {"Opacity", 0.5 },
                                              {"Type", Syncfusion.Blazor.Charts.ChartSeriesType.SplineArea },
                                          },
                                          MarkerAttributes = new Dictionary<string, object>
                                          {
                                              { "Visible", false }
                                          },
                                          LabelAttributes = new Dictionary<string, object>
                                          {
                                              { "Visible", false },
                                          },
                                      },
                                   }
                              }
                          }
                     },
                     // GROUP T/A
                     new PanelObject() { Id = "panelGroupTA", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Groupe T/A", ComponentType = typeof(GridComponent<Station>),
                         Parameters = new Dictionary<string, object>
                         {
                             { "GridId", "gridGroupTA" },
                             { "DataSource", DataSource },
                             { "ColumnsList", new List<GridColumnsConfig>
                                {
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"Field", nameof(Station.StationName) },
                                            {"HeaderText", "Centrale" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center }
                                        },
                                    },
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"HeaderText", "Groupe 1" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center }
                                        },
                                        Template = (context) =>
                                        {
                                                var station = context as Station;
                                                if (station != null)
                                                {
                                                    Boolean? group1 = (station.Groups.Count > 0) ? station.Groups[0].GroupTA : null;

                                                    if (group1.HasValue)
                                                    {
                                                        String id = (group1 == true) ? "actif" : "inactif";

                                                        return (builder) =>
                                                        {

                                                            builder.OpenElement(0, "div");
                                                            builder.OpenElement(1, "span");
                                                            builder.AddAttribute(2, "id", id);
                                                            builder.AddAttribute(3, "class", "groupta");
                                                            builder.AddContent(4, id);
                                                            builder.CloseElement(); // </span>
                                                            builder.CloseElement(); // </div>
                                                        };
                                                    }
                                                }

                                                return (builder) =>
                                                {
                                                    builder.AddContent(0, "");
                                                };
                                        }
                                    },
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"HeaderText", "Groupe 2" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center }
                                        },
                                        Template = (context) =>
                                        {
                                                var station = context as Station;
                                                if (station != null)
                                                {
                                                    Boolean? group2 = (station.Groups.Count > 1) ? station.Groups[1].GroupTA : null;

                                                    if (group2.HasValue)
                                                    {
                                                        String id = (group2 == true) ? "actif" : "inactif";

                                                        return (builder) =>
                                                        {

                                                            builder.OpenElement(0, "div");
                                                            builder.OpenElement(1, "span");
                                                            builder.AddAttribute(2, "id", id);
                                                            builder.AddAttribute(3, "class", "groupta");
                                                            builder.AddContent(4, id);
                                                            builder.CloseElement(); // </span>
                                                            builder.CloseElement(); // </div>
                                                        };
                                                    }
                                                }

                                                return (builder) =>
                                                {
                                                    builder.AddContent(0, "");
                                                };
                                        }
                                    },
                                }
                             }
                         }
                     },
                     // AVANCEMENT PRODUCTION
                      new PanelObject() { Id = "panelTargetProduction", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Avancement production", ComponentType = typeof(GridComponent<Station>),
                         Parameters = new Dictionary<string, object>
                         {
                             { "GridId", "gridTargetProduction" },
                             { "DataSource", DataSource },
                             { "ColumnsList", new List<GridColumnsConfig>
                                {
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"Field", nameof(Station.StationName) },
                                            {"HeaderText", "Centrale" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center }
                                        },
                                    },
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"Field", nameof(Station.MonthlyProductionActual) },
                                            {"HeaderText", "Réalisation Mensuelle" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center },
                                            {"Format", "0 mW" }
                                        },
                                    },
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"Field", nameof(Station.MonthlyProductionTarget) },
                                            {"HeaderText", "Objectif Mensuel" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center },
                                            {"Format", "0 mW" }
                                        },
                                    },
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"Field", nameof(Station.MonthlyPercentage) },
                                            {"HeaderText", "%" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center },
                                            {"Format", "0%" }
                                        },
                                    },
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"Field", nameof(Station.AnnualProductionActual) },
                                            {"HeaderText", "Réalisation Annuelle" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center },
                                            {"Format", "0 mW" }
                                        },
                                    },
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"Field", nameof(Station.AnnualProductionTarget) },
                                            {"HeaderText", "Objectif Annuel" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center },
                                            {"Format", "0 mW" }
                                        },
                                    },
                                    new GridColumnsConfig()
                                    {
                                        ColumnAttributes = new Dictionary<string, object>
                                        {
                                            {"Field", nameof(Station.AnnualPercentage) },
                                            {"HeaderText", "%" },
                                            {"TextAlign", Syncfusion.Blazor.Grids.TextAlign.Center },
                                            {"Format", "0%" }
                                        },
                                    },
                                }
                             }
                         }
                     },
                      // CANADA MAP
                      new PanelObject() { Id = "panelMap", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Localisation", ComponentType = typeof(MapComponent<StationMapData>),
                         Parameters = new Dictionary<string, object>
                         {
                             { "MapId", "Map" },
                             { "Title", "Canada" },
                             { "MapTheme", AppTheme },
                             { "OnMarkerClickEvent",  new EventCallback<MarkerClickEventArgs>(this, OnMarkerClickEvent) },
                             {"MarkerAttributes", new Dictionary<string, object>()
                                {
                                    {"Visible", true },
                                    {"DataSource", DataProvider.GetMapDetails() },
                                    {"Height", 15.0 },
                                    {"Width", 15.0 },
                                    {"LatitudeValuePath", nameof(StationMapData.Latitude) },
                                    {"LongitudeValuePath", nameof(StationMapData.Longitude) },
                                    {"AnimationDelay", 0.0 },
                                    {"AnimationDuration", 0.0 },

                                } 
                             },
                             {"MarkerToolTipAttributes", new Dictionary<string, object>()
                                {
                                    {"Visible", true },
                                    {"ValuePath", nameof(StationMapData.Name) },
                                } 
                             },
                             {"ZoomToolBarAttributes", new Dictionary<string, object>()
                                {
                                    {"HorizontalAlignment", Syncfusion.Blazor.Maps.Alignment.Near },
                                    {"Orientation", Syncfusion.Blazor.Maps.Orientation.Vertical },
                                } 
                             },
                         }
                     },
                      // STATISTICS
                      new PanelObject() { Id = "panelPieChart", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Statistiques", ComponentType = typeof(PieChartComponent),
                         Parameters = new Dictionary<string, object>
                         {
                             { "PieChartId", "pieChart" },
                             { "ChartTheme", AppTheme },
                            
                             {"ToolTipAttributes", new Dictionary<string, object>()
                                {
                                    {"Enable", true },
                                    {"Format", "${point.x} : <b>${point.y}%</b>" },
                                }
                             },
                             {"LegendAttributes", new Dictionary<string, object>()
                                {
                                    {"Position", Syncfusion.Blazor.Charts.LegendPosition.Bottom },
                                }
                             },
                             {"SeriesAttributes", new Dictionary<string, object>()
                                {
                                    { "DataSource", new List<PieData>()
                                        {
                                            new PieData { Name = "Station 1", Percentage = 57.28 },
                                            new PieData { Name = "Station 2", Percentage = 4.73 },
                                            new PieData { Name = "Station 3", Percentage = 5.96 },
                                            new PieData { Name = "Station 4", Percentage = 4.37 },
                                            new PieData { Name = "Station 5", Percentage = 7.48 },
                                            new PieData { Name = "Station 6", Percentage = 14.06 },
                                            new PieData { Name = "Station 7", Percentage = 6.12 }
                                        }
                                    },
                                    { "XName", nameof(PieData.Name) },
                                    { "YName", nameof(PieData.Percentage) },
                                    { "Name", "Production" }
                                }
                             },
                              {"LabelAttributes", new Dictionary<string, object>()
                                {
                                   { "Visible", false }
                                }
                             },
                         }
                     },


            };
        }

       
    }
}
