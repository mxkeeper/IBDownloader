using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro;
using System.Windows;

namespace IBDownloader
{
    public class Options
    {
        private int _MaxConcurrentDownloads = 1;
        private int _MaxConnectionPerServer = 1;
        private bool _AutoRefresh = false;

        public int MaxConcurrentDownloads
        {
            get { return _MaxConcurrentDownloads; }
            set { _MaxConcurrentDownloads = value; }
        }

        public int MaxConnectionPerServer
        {
            get { return _MaxConnectionPerServer; }
            set { _MaxConnectionPerServer = value; }
        }

        public bool AutoRefresh
        {
            get { return _AutoRefresh; }
            set { _AutoRefresh = value; }
        }

        public void Save()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            Properties.Settings.Default["AppTheme"] = theme.Item1.Name;
            Properties.Settings.Default["AppColor"] = theme.Item2.Name;
            Properties.Settings.Default.Save();
        }

        public void Load()
        {
            string AppTheme = Properties.Settings.Default["AppTheme"].ToString();
            string AppColor = Properties.Settings.Default["AppColor"].ToString();

            // get the theme from the current application
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            // now set the Green accent and dark theme
            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent(AppColor),
                                        ThemeManager.GetAppTheme(AppTheme));
        }
    }
}
