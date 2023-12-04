using System;
using AxorP1.Class;

namespace AxorP1.Services
{
    public class DataProvider 
    {

        // Axor hydroelectric power stations within Canada
        private List<StationMapData> MapDetails = new List<StationMapData>
        {
            new StationMapData { Name = "Station 1", Latitude = 55.4215, Longitude = -75.6993 },
            new StationMapData { Name = "Station 2", Latitude = 60.5461, Longitude = -120.4938 },
            new StationMapData { Name = "Station 3", Latitude = 49.2827, Longitude = -123.1207 },
            new StationMapData { Name = "Station 4", Latitude = 43.6532, Longitude = -79.3832 },
            new StationMapData { Name = "Station 5", Latitude = 50.8139, Longitude = -60.208 },
            new StationMapData { Name = "Station 6", Latitude = 53.9333, Longitude = -116.5765 },
            new StationMapData { Name = "Station 7", Latitude = 45.5017, Longitude = -73.5673 },
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
        public List<Group> GetAllGroups(List<Station> Data)
        {
            List<Group> allGroups = new List<Group>();

            foreach (var station in Data)
            {
                allGroups.AddRange(station.Groups);
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
                for (int year = 1; year <= 3; year++) // Years
                {
                    for (int i = 1; i <= 12; i++) // Months
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
