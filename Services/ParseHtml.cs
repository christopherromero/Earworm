using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Earworm.Services
{
    public class ParseHtml
    {
        public string ParseFromWeb(string url)
        {

            var hw = new HtmlWeb();
            var doc = hw.Load(url);

            IEnumerable<HtmlNode> nodes =
                doc.DocumentNode.Descendants(0)
                    .Where(n => n.HasClass("lyrics"));

            string lyrics = nodes.FirstOrDefault().InnerHtml.ToString();

            lyrics = StripTagsRegex(lyrics);
            lyrics = lyrics.Substring(30);
            lyrics = StripNewLines(lyrics);
            return lyrics;
        }
        public static string StripTagsRegex(string source)
        {

            return Regex.Replace(source, "<[^>]*>", string.Empty);
        }

        public static string StripNewLines(string source)
        {
            return Regex.Replace(source, @"\t|\n|\r", "</br>");
        }

        public string Urlify(string source)
        {
            return Regex.Replace(source, " ", "%20");
        }

    }
}
