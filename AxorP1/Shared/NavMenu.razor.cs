using AxorP1.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Navigations;

namespace AxorP1.Shared
{
    public class NavMenuBase : MainComponent<NavMenu>
    {
        protected SfSidebar SidebarRef;

        // Specify the value of Sidebar component state (open/close).
        protected bool SidebarToggle = false;
        protected string ToggleClass = "close";

        // Lock the Sidebar in open state
        protected bool SidebarLocked = false;

        // Event handler for Clicked event. It's used to open/close the Sidebar component. 
        protected void ToggleSidebar(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        { 
            if (SidebarToggle == true)
            {
                // Lock or unlock the Sidebar
                SidebarLocked = !SidebarLocked;
            }

            
            if(SidebarLocked == false) 
            {
                // If Sidebar is not lock
                SidebarToggle = !SidebarToggle;
                ToggleClass = SidebarToggle ? "open" : "close";
            }
            else
            {
                ToggleClass += " locked";
            }
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
                if (SidebarLocked == false)
                { 
                    // If Sidebar is not lock
                    SidebarToggle = false;
                    StateHasChanged();
                }
            }
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);


        }
    }
}

