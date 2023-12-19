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

        // Theme Switcher
        protected bool isDarkMode = false;
        protected EventCallback<ChangeEventArgs<bool?>> switchStateChanged;

        // Specify the value of Sidebar component state (open/close).
        protected bool SidebarToggle = false;
        protected string ToggleClass = "close";

        // Event handler for Clicked event. It's used to open/close the Sidebar component. 
        protected void ToggleSidebar(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
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

            switchStateChanged = EventCallback.Factory.Create<ChangeEventArgs<bool?>>(this, HandleSwitchStateChanged);

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


        protected void HandleSwitchStateChanged(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
        {
            isDarkMode = (bool)args.Checked;

            // (true for dark mode, false for light mode)
            if (isDarkMode)
            {
                // Dark mode is enabled
               // AppTheme = Syncfusion.Blazor.Theme.FluentDark;
                
                
            }
            else
            {
                // Light mode is enabled
                
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            await JSRuntime.InvokeVoidAsync("checkOverflow");

        }


    }
}

