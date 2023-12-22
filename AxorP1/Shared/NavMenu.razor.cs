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

        // Lock the Sidebar in open state
        protected bool SidebarLocked = false;

        // Specify the value of Sidebar component state (open/close).
        protected string ToggleClass = "close";
        private bool _sidebarToggle;
        protected bool SidebarToggle
        {
            get => _sidebarToggle;
            set
            {
                if (_sidebarToggle != value)
                {
                    _sidebarToggle = value;
                    StateHasChanged();

                    // Calling IsOpenChanged Method
                    IsOpenChanged();
                }
            }
        }



        protected override void OnInitialized()
        {
            // Handle LocationChanged Event
            NavigationManager.LocationChanged += HandleLocationChanged;

            base.OnInitialized();
        }

        // Close Sidebar when location changed
        private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
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
                try
                { 
                    // If Sidebar is closed
                    // Change the SidebarType depending on screen size
                    double width = await JSRuntime.InvokeAsync<double>("getWidth");
                    bool IsSmallScreen = (width <= 600) ? true : false;

                    if (IsSmallScreen)
                    {
                        // Type = SidebarType.Over;
                    }
                    else
                    {
                        Type = SidebarType.Push;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Error Invoking JSRuntime at ToggleSidebarAsync() : {ex.Message}\n{ex.StackTrace}");
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

        // Method triggered when SidebarToggle changes
        private async void IsOpenChanged()
        {
            await Task.Delay(500);

            // Refresh Dashboard Panels
            RefProvider.MainDashboard?.RefreshAllPanelsAsync();
            RefProvider.StationDashboard?.RefreshAllPanelsAsync();
        }



    }
}

