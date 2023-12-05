using Timer = System.Timers.Timer;
using Syncfusion.Blazor.Layouts;
using AxorP1.Class;
using Microsoft.AspNetCore.Http.HttpResults;
using AxorP1.Components.Panels;
using System.Collections.Generic;
using static AxorP1.Components.Panels.ChartComponent;
using Microsoft.AspNetCore.Components;
using AxorP1.Services;
using static AxorP1.Components.Panels.GridComponent;
using System;


namespace AxorP1.Components.Pages
{
    public class IndexBase : MainComponent<Index>
    {
        private static Timer timer = new Timer(5000);

        protected SfDashboardLayout DashboardLayout { get; set; }

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

            InitializedPanelData();

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
            string resizedPanelId = args.Id;

            RefreshPanel(resizedPanelId);
        }

        public async Task OnWindowResize(Syncfusion.Blazor.Layouts.ResizeArgs args)
        {
            await DashboardLayout.RefreshAsync();
        }


        public void RefreshPanel(string id)
        {
            var panelToRefresh = PanelData.FirstOrDefault((panel) => panel.Id == id);

            if (panelToRefresh != null)
            {
                panelToRefresh.refreshCounter++;
            }
        }

        public void RefreshAllPanels()
        {
            foreach (PanelObject Panel in PanelData)
            {
                Panel.refreshCounter++;
            }
        }

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
                     new PanelObject() { Id = "panelGroupTA", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Groupe T/A", ComponentType = typeof(GridComponent),
                         Parameters = new Dictionary<string, object>
                         {
                             { "GridId", "gridGroupTA" },
                             { "TValue", typeof(Station) },
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
                      new PanelObject() { Id = "panelTargetProduction", Column = 0, Row = 0, SizeX = 1, SizeY = 1, Title = "Avancement production", ComponentType = typeof(GridComponent),
                         Parameters = new Dictionary<string, object>
                         {
                             { "GridId", "gridTargetProduction" },
                             { "TValue", typeof(Station) },
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


            };
        }
    }
}
