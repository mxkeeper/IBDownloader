using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBDownloader.Parser
{
    public static class AnalyzerLinks
    {
        public static Board Do(string link)
        {
            if (link.Contains("arhivach.org")) return Board.Arhivach;
            if (link.Contains("2ch.hk")) return Board.Dvach;

            return Board.Unidentified;
        }
    }
}
