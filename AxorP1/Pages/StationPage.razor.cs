using System.Linq.Expressions;
using AxorP1.Components;
using AxorP1.Pages;
using Microsoft.AspNetCore.Components;

namespace AxorP1.Pages
{
	public class StationPageBase : MainComponent<StationPage>
	{
		[Parameter] public string id { get; set; } = string.Empty;

        private int num;
        protected string RangeHeaderText { get; set; } = string.Empty;

        protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

            RangeHeaderText = $"Production Centrale dans le temps";

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

            num = stationIdToNumber.TryGetValue(id.Trim(), out var result) ? result : 1;

            // Update PastDataSource List with the data of the wanted station 
            await UpdatePastDataSourceAsync(num);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
