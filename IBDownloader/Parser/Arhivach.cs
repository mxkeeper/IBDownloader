using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBDownloader.Parser
{
    public class Arhivach : Parser
    {
        public override async Task<List<string>> GetLinksToDownload(string url)
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

        /// <summary>
        /// Извлекаем ссылку на изображение(видео) из строки
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal override string ExtractImageURL(string input)
        {
            int IndexBeginLink = input.IndexOf("http");
            int IndexEndLink = IndexOfNth(input, '\'', 4);

            string result = input.Substring(IndexBeginLink, IndexEndLink - IndexBeginLink);

            return Utils.HTTPtoHTTPS(result);
        }
    }
}
