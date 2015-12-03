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
        // Время обновления треда по умолчанию 5 мин
        private int _AutoUpdateInterval = 5;
        private bool _AutoRefresh = false;
        private string _LastFolder;

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

        public int AutoUpdateInterval
        {
            get { return _AutoUpdateInterval; }
            set { _AutoUpdateInterval = value; }
        }

        public string LastFolder
        {
            get { return _LastFolder; }
            set { _LastFolder = value; }
        }

        public void Save()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            Properties.Settings.Default["AppTheme"] = theme.Item1.Name;
            Properties.Settings.Default["AppColor"] = theme.Item2.Name;

            Properties.Settings.Default["AutoRefresh"] = AutoRefresh;
            Properties.Settings.Default["AutoUpdateInterval"] = AutoUpdateInterval;

            Properties.Settings.Default["LastFolder"] = LastFolder;

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


            AutoRefresh = Convert.ToBoolean(Properties.Settings.Default["AutoRefresh"].ToString());
            AutoUpdateInterval = Convert.ToInt32(Properties.Settings.Default["AutoUpdateInterval"].ToString());
            LastFolder = Properties.Settings.Default["LastFolder"].ToString();
        }
    }
}
