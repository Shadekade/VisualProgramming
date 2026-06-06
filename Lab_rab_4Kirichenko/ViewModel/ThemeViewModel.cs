using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab_rab_4Kirichenko.ViewModel
{
    public class ThemeViewModel : NotifyPropertyChanged
    {

        public string[] AvailableThemes { get; } = { "Светлая", "Темная" };

        private string selectedTheme;
        public string SelectedTheme
        {
            get { return selectedTheme; }
            set
            {
                if (selectedTheme != value)
                {
                    selectedTheme = value;
                    OnPropertyChanged(nameof(SelectedTheme));
                    ApplyTheme(value);
                }
            }
        }

        public ThemeViewModel()
        {

            selectedTheme = AvailableThemes.First();
            ApplyTheme(selectedTheme);
        }


        private void ApplyTheme(string themeName)
        {
            string themeFileName = themeName == "Темная" ? "DarkTheme.xaml" : "LightTheme.xaml";

            var newThemeDict = new ResourceDictionary()
            {

                Source = new Uri($"/Lab_rab_4Kirichenko;component/Themes/{themeFileName}", UriKind.Relative)
            };

            var applicationResources = Application.Current.Resources.MergedDictionaries;
            var oldTheme = applicationResources
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme"));

            if (oldTheme != null)
            {
                applicationResources.Remove(oldTheme);
            }

            applicationResources.Add(newThemeDict);
        }
    }
}