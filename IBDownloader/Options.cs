using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBDownloader
{
    public class Options
    {
        private int _MaxConcurrentDownloads = 1;
        private int _MaxConnectionPerServer = 1;
        private bool _AutoRefresh = false;
        private bool _DownloadEntirePage = false;

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

        public bool DownloadEntirePage
        {
            get { return _DownloadEntirePage; }
            set { _DownloadEntirePage = value; }
        }

        public bool AutoRefresh
        {
            get { return _AutoRefresh; }
            set { _AutoRefresh = value; }
        }
    }
}
