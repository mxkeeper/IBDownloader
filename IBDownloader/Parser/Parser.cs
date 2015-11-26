using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.Threading.Tasks;

namespace IBDownloader.Parser
{
    public abstract class Parser
    {
        private bool _downloadEntirePage;

        public bool DownloadEntirePage
        {
            get { return _downloadEntirePage; }
            set { _downloadEntirePage = value; }
        }

        /*
        public Parser(bool DownloadEntirePage)
        {
            _downloadEntirePage = DownloadEntirePage;
        }
        */

        public virtual async Task<List<string>> GetLinksToDownload(string url)
        {
            return new List<string>();
        }

        internal virtual string ExtractImageURL(string input)
        {
            return String.Empty;
        }

        internal static int IndexOfNth(string str, char c, int n)
        {
            int s = -1;

            for (int i = 0; i < n; i++)
            {
                s = str.IndexOf(c, s + 1);

                if (s == -1) break;
            }

            return s;
        }

        internal Task<string> HttpGetAsync(string URI)
        {
            try
            {
                HttpClient hc = new HttpClient();
                return hc.GetStringAsync(URI);
            }
            catch (WebException)
            {
                return Task.Run(() =>
                {
                    return string.Empty;
                });
            }
        }
    }
}
