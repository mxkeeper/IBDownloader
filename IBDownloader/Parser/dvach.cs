using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBDownloader.Parser
{
    public class Dvach : Parser
    {
        public override async Task<List<string>> GetLinksToDownload(string url)
        {
            List<string> resultList = new List<string>();

            string data = await HttpGetAsync(url);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(data);
            if (htmlDoc.DocumentNode != null)
            {
                // Выделяем тег, содержащий ссылку на картинку
                HtmlNodeCollection Nodes = htmlDoc.DocumentNode.SelectNodes("//a[@class=\"desktop\"]");

                foreach (HtmlNode Node in Nodes)
                    resultList.Add(ExtractImageURL(Node.Attributes["href"].Value));
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
        /// Собираем окончательную ссылку, добавляем имя домена к началу пути 
        /// "/s/342425/23565465462.jpg" ---> "https://2ch.hk/s/342425/23565465462.jpg"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal override string ExtractImageURL(string input)
        {
            var sb = new StringBuilder(input);
            sb.Insert(0, "https://2ch.hk");
            return sb.ToString();
        }
    }
}
