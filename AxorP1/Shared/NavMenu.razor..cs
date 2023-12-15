using AxorP1.Components;
using Microsoft.AspNetCore.Components.Routing;
using Syncfusion.Blazor.Navigations;

namespace AxorP1.Shared
{
    public class NavMenuBase : MainComponent<NavMenu>
    {
        protected SfSidebar SidebarRef;

        // Specify the value of Sidebar component state (open/close).
        protected bool SidebarToggle = false;
        protected string ToggleClass = "close";

        // Event handler for Clicked event. It's used to open/close the Sidebar component. 
        public void ToggleSidebar(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            SidebarToggle = !SidebarToggle;
            ToggleClass = SidebarToggle ? "open" : "close";
        }

        // Method to handle the close event
        public void OnClose()
        {
            ToggleClass = "close";
        }

        protected override void OnInitialized()
        {
            NavigationManager.LocationChanged += HandleLocationChanged;
            base.OnInitialized();
        }

        // Close Sidebar when location changed
        private async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            if (SidebarRef.IsOpen)
            {
                SidebarToggle = false;
                StateHasChanged();
            }
        }

    }
}

