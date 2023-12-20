using AxorP1.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Navigations;

namespace AxorP1.Shared
{
    public class NavMenuBase : MainComponent<NavMenu>
    {
        protected SfSidebar SidebarRef;
        protected SidebarType Type = SidebarType.Push;

        // Specify the value of Sidebar component state (open/close).
        protected bool SidebarToggle = false;
        protected string ToggleClass = "close";

        // Lock the Sidebar in open state
        protected bool SidebarLocked = false;

        // Event handler for Clicked event on Toggle Button 
        protected async Task ToggleSidebarAsync(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            // If Sidebar is open
            if (SidebarToggle == true)
            {
                // Lock or unlock the Sidebar
                SidebarLocked = !SidebarLocked;
            }
            else
            {
                // If Sidebar is closed
                // Change the SidebarType depending on screen size

                double width = await JSRuntime.InvokeAsync<double>("getWidth");
                bool IsSmallScreen = (width <= 600) ? true : false;

                if (IsSmallScreen)
                {
                    Type = SidebarType.Over;
                }
                else
                {
                    Type = SidebarType.Push;
                }
            }

            // If Sidebar is not lock
            if (SidebarLocked == false) 
            {
                // open/close the Sidebar 
                SidebarToggle = !SidebarToggle;
                ToggleClass = SidebarToggle ? "open" : "close";
            }
            else
            {
                // Adding the locked class
                ToggleClass += " locked";
            }
        }

        // Method to handle the close event
        public void OnClose(Syncfusion.Blazor.Navigations.EventArgs args)
        {
            // If Sidebar is not lock
            if (SidebarLocked == false)
            {
                ToggleClass = "close";
            }
            else
            {
                // If Sidebar is lock
                // Prevent the Sidebar from closing
                args.Cancel = true;

            }
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
                if (SidebarLocked == false)
                { 
                    // If Sidebar is not lock
                    SidebarToggle = false;
                    StateHasChanged();
                }
            }
        }
    }
}

