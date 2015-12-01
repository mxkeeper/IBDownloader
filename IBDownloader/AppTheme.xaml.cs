using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace IBDownloader
{
    /// <summary>
    /// Interaction logic for AppTheme.xaml
    /// </summary>
    public partial class AppTheme : MetroWindow
    {
        private string[] themes = {
                            "BaseLight",
                            "BaseDark"
        };
        private string[] colors = {
                            "Red",
                            "Green",
                            "Blue",
                            "Purple",
                            "Orange",
                            "Lime",
                            "Emerald",
                            "Teal",
                            "Cyan",
                            "Cobalt",
                            "Indigo",
                            "Violet",
                            "Pink",
                            "Magenta",
                            "Crimson",
                            "Amber",
                            "Yellow",
                            "Brown",
                            "Olive",
                            "Steel",
                            "Mauve",
                            "Taupe",
                            "Sienna"};
        public AppTheme()
        {
            InitializeComponent();

            var CurrentTheme = ThemeManager.DetectAppStyle(Application.Current);
            lstThemeStyle.SelectedIndex = Array.IndexOf(themes, CurrentTheme.Item1.Name);
            cmbAppColor.SelectedIndex = Array.IndexOf(colors, CurrentTheme.Item2.Name);
            
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeAppStyle(string color, string theme)
        {
            // get the theme from the window
            var CurrentTheme = ThemeManager.DetectAppStyle(Application.Current);
            // now set the Red accent and dark theme
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(color), ThemeManager.GetAppTheme(theme));
        }

        private void lstThemeStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the theme from the window
            var CurrentTheme = ThemeManager.DetectAppStyle(Application.Current);

            switch (lstThemeStyle.SelectedIndex)
            {
                case 0:
                    ChangeAppStyle(CurrentTheme.Item2.Name, themes[0]);
                        break;
                case 1:
                    ChangeAppStyle(CurrentTheme.Item2.Name, themes[1]);
                    break;
            }
        }

        private void cmbAppColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the theme from the window
            var CurrentTheme = ThemeManager.DetectAppStyle(Application.Current);

            ChangeAppStyle(colors[cmbAppColor.SelectedIndex], CurrentTheme.Item1.Name);
        }
    }
}
