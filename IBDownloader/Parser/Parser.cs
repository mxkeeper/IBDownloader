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
        HttpClientHandler handler = new HttpClientHandler();
        CookieContainer cookies = new CookieContainer();
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
                CookieCollection collection = handler.CookieContainer.GetCookies(new Uri(URI));

                handler.CookieContainer = cookies;
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                foreach (Cookie cookie in collection)
                    cookies.Add(cookie);

                HttpClient hc = new HttpClient(handler);

                hc.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("DNT", "1");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36");          

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
