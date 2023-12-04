using AxorP1.Class;
using Microsoft.AspNetCore.Components;

namespace AxorP1.Components.Pages
{
	public class StationPageBase : MainComponent<StationPage>
	{
		[Parameter]
		public string id { get; set; } = string.Empty;


		protected override void OnParametersSet()
		{
			base.OnParametersSet();
		}

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
