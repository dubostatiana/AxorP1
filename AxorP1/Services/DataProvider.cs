using System;
using AxorP1.Class;

namespace AxorP1.Services
{
    public class DataProvider 
    {

        // Axor hydroelectric power stations within Canada
        private List<StationMapData> MapDetails = new List<StationMapData>
        {
            new StationMapData { Name = "Chutes-à-Gorry", Latitude = 46.78983433499708, Longitude = -72.00353219004116 }, // Sainte-Anne / Chutes-à-Gorry
            new StationMapData { Name = "Jean-Guérin", Latitude = 46.63504334946685, Longitude = -71.044330699399 }, // Jean-Guérin 
            new StationMapData { Name = "Hydro-Canyon", Latitude = 47.055080489178124, Longitude = -70.84331694178222 }, // Hydro-Canyon Saint-Joachim  
            new StationMapData { Name = "Petites-Bergeronnes", Latitude = 48.262531954209, Longitude = -69.63143815475888 }, // Petites-Bergeronnes 
            new StationMapData { Name = "Franquelin", Latitude = 49.298872387059106, Longitude = -67.84395606024589 }, // Franquelin 
            new StationMapData { Name = "Sheldrake", Latitude = 50.28440380362198, Longitude = -64.9309060547453 }, // Sheldrake 
            new StationMapData { Name = "Long Rapids", Latitude = 48.60275472499946, Longitude = -88.78180622752375 }, // Long Rapids et Twin Falls 
        };

        // Method to get the data
        public async Task<List<Station>> GetDataAsync()
        {
            List<Station> Data = new List<Station>();
            Random random = new Random();

            return await Task.Run(() =>
            {
                for (int i = 1; i <= 7; i++)
                {
                    double num = random.Next(1, 8);

                    var station = new Station
                    {
                        DateTime = DateTime.Now,
                        StationName = $"Station {i}",
                        UpstreamLevel = 60 - num,
                        DownstreamLevel = 20 - num,
                        CentralProduction = 100 - 10 * num,
                        FallHeight = 15 - num,
                        TotalFlowRate = 20 - num,
                        MonthlyProductionTarget = 150 - 10 * num,
                        AnnualProductionTarget = 1800 - 100 * num,
                        MonthlyProductionActual = 120 - 10 * num,
                        AnnualProductionActual = 1500 - 100 * num,

                        Groups = new List<Group>()
                    };

                    // Add the first group
                    station.Groups.Add(new Group
                    {
                        StationName = $"Station {i}",
                        GroupName = "Group 1",
                        FlowRate = 18 - num,
                        GroupTA = true,
                        Production = 70 + num,
                        FineGridDifferential = 10 - num,
                        CoarseGridDifferential = 13 - num
                    });

                    // Add the second group only if the condition is met
                    if (i % 2 == 0)
                    {
                        station.Groups.Add(new Group
                        {
                            StationName = $"Station {i}",
                            GroupName = "Group 2",
                            FlowRate = 20 - i,
                            GroupTA = false,
                            Production = 50 + num,
                            FineGridDifferential = 11 - num,
                            CoarseGridDifferential = 16 - num
                        });
                    }

                    Data.Add(station);
                }

                return Data;
            });


        }

        // Method to get all groups from the existing Data property
        public List<Group> GetAllGroups(List<Station> Data, string? filterGroupName = null)
        {
            List<Group> allGroups = new List<Group>();

            foreach (var station in Data)
            {
                allGroups.AddRange(station.Groups);
            }

            if (filterGroupName != null)
            {
                return allGroups.Where(d => d.GroupName == filterGroupName).ToList();
            }

            return allGroups;
        }


        // Method to get the data of the past for one station 
        public async Task<List<Station>> GetPastDataAsync(int id)
        {
            List<Station> PastData = new List<Station>();
            Random random = new Random();

            return await Task.Run(() =>
            {
                for (int year = 0; year <= 3; year++) // Years
                {
                    for (int i = 0; i < 12; i++) // Months
                    {
                        double num = random.Next(1, 8);

                        var station = new Station
                        {
                            DateTime = DateTime.Now.AddMonths(-i).AddYears(-year),
                            StationName = $"Station {id}",
                            UpstreamLevel = 60 - num,
                            DownstreamLevel = 20 - num,
                            CentralProduction = 100 - 10 * num,
                            FallHeight = 15 - num,
                            TotalFlowRate = 20 - num,
                            MonthlyProductionTarget = 150 - 10 * num,
                            AnnualProductionTarget = 1800 - 100 * num,
                            MonthlyProductionActual = 120 - 10 * num,
                            AnnualProductionActual = 1500 - 100 * num,

                            Groups = new List<Group>()
                        };

                        // Ajouter le premier groupe
                        station.Groups.Add(new Group
                        {
                            StationName = $"Station {id}",
                            GroupName = "Group 1",
                            FlowRate = 18 - num,
                            GroupTA = true,
                            Production = 70 + num,
                            FineGridDifferential = 10 - num,
                            CoarseGridDifferential = 13 - num
                        });

                        // Ajouter le deuxième groupe uniquement si la condition est remplie
                        if (id % 2 == 0)
                        {
                            station.Groups.Add(new Group
                            {
                                StationName = $"Station {id}",
                                GroupName = "Group 2",
                                FlowRate = 20 - i,
                                GroupTA = false,
                                Production = 50 + num,
                                FineGridDifferential = 11 - num,
                                CoarseGridDifferential = 16 - num
                            });
                        }

                        PastData.Add(station);
                    }
                }

                return PastData;
            });
        }

        public List<StationMapData> GetMapDetails()
        {
            return MapDetails;
        }

    }
}
