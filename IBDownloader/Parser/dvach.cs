﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBDownloader.Parser
{
    public class Dvach : Parser
    {
        private string Board = string.Empty;
        public override async Task<List<string>> GetLinksToDownload(string url)
        {
            List<string> resultList = new List<string>();

            GetBoardLetter(url);

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

        private void GetBoardLetter(string url)
        {
            int IndexBeginLink = IndexOfNth(url,'/',3);
            int IndexEndLink = IndexOfNth(url, '/', 4);

            Board = url.Substring(IndexBeginLink, IndexEndLink - IndexBeginLink);
        }

        /// <summary>
        /// Собираем окончательную ссылку, добавляем имя домена к началу пути 
        /// "/s/342425/23565465462.jpg" ---> "https://2ch.hk/s/342425/23565465462.jpg"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal override string ExtractImageURL(string input)
        {
            return input.Replace("..", "https://2ch.hk" + Board);
        }
    }
}
