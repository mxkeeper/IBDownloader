using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBDownloader
{
    public class Thread
    {
        public Thread()
        {
            // default constructor
        }

        public string Link { get; set; }
        public string OutputDir { get; set; }
        public string DownloadEntirePage { get; set; }
        public string Progress { get; set; }
        public int ProgressBarVal { get; set; }
        public string Status { get; set; }
    }
}
