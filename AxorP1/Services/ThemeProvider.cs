using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AxorP1.Services
{
    public class ThemeProvider
    {
        public bool? isDarkMode;
        public string ThemeClass { 
            get {
               return isDarkMode == true ? "dark" : "light";
            } 
        }
        public Syncfusion.Blazor.Theme AppTheme { get; set; } = Syncfusion.Blazor.Theme.Fluent;

        private readonly IJSRuntime JSRuntime;

        public ThemeProvider(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }
    
        public async Task InitialTheme()
        {
            var themePreference = await JSRuntime.InvokeAsync<string>("getThemePreference");
            var darkTheme = themePreference == "dark";

           await ChangeTheme(darkTheme);
            
        }

        public async Task ChangeTheme(bool? darkTheme)
        {

            if (darkTheme == null || isDarkMode == darkTheme) { return; }


            isDarkMode = darkTheme;

            // Save theme preference
            await JSRuntime.InvokeVoidAsync("setThemePreference", ThemeClass);


            String link;

            if (isDarkMode == true)
            {
                link = "_content/Syncfusion.Blazor.Themes/fluent-dark.css"; // Update with your dark theme CSS path
                AppTheme = Syncfusion.Blazor.Theme.FluentDark;
            }
            else
            {
                link = "_content/Syncfusion.Blazor.Themes/fluent.css"; // Update with your light theme CSS path
                AppTheme = Syncfusion.Blazor.Theme.Fluent;
            }

            await JSRuntime.InvokeVoidAsync("changeTheme", link);
        }

       


    }
}
