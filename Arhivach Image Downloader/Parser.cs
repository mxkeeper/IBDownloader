using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.Threading.Tasks;

namespace Arhivach_Image_Downloader
{
    public class Parser
    {
        private bool _downloadEntirePage;
        public Parser(bool DownloadEntirePage)
        {
            _downloadEntirePage = DownloadEntirePage;
        }

        public async Task<List<string>> GetLinksToDownload(string url)
        {
            List<string> resultList = new List<string>();

            string data = await HttpGetAsync(url);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(data);
            if (htmlDoc.ParseErrors.Count() == 0)
            {
                if (htmlDoc.DocumentNode != null)
                {
                    // Выделяем тег, содержащий ссылку на картинку
                    HtmlNodeCollection Nodes = htmlDoc.DocumentNode.SelectNodes("//a[@class=\"expand_image\"]");

                    foreach (HtmlNode Node in Nodes)
                        resultList.Add(ExtractImageURL(Node.OuterHtml));
                }
            }
            else
            {
                return new List<string>();
            }

            // Удаляем повторяющиеся ссылки из списка
            resultList = resultList.Distinct().ToList();

            return resultList;
        }

        private static string ExtractImageURL(string input)
        {
            int IndexBeginLink = input.IndexOf("http");
            int IndexEndLink = IndexOfNth(input, '\'', 4);

            return input.Substring(IndexBeginLink, IndexEndLink - IndexBeginLink);
        }

        private static int IndexOfNth(string str, char c, int n)
        {
            int s = -1;

            for (int i = 0; i < n; i++)
            {
                s = str.IndexOf(c, s + 1);

                if (s == -1) break;
            }

            return s;
        }

        private Task<string> HttpGetAsync(string URI)
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
