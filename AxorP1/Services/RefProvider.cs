using AxorP1.Pages;

namespace AxorP1.Services
{
    public class RefProvider
    {
        public IndexBase? MainDashboard;
        public StationPageBase? StationDashboard;

        public async Task RefreshComponents()
        {
            if (MainDashboard != null)
            {
                await MainDashboard?.RefreshAllPanelsAsync();
            }
            
            if(StationDashboard != null)
            {
                await StationDashboard?.RefreshAllPanelsAsync();
            }
        }

        public void ComponentsStateChanged()
        {
            if (MainDashboard != null)
            {
                MainDashboard?.PanelsStateChanged();
            }

            if (StationDashboard != null)
            {
                StationDashboard?.PanelsStateChanged();
            }
        }
    }
}
