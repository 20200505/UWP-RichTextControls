using System;
using HtmlAgilityPack;

namespace Html2XHtml
{
    public class Html2XHtml
    {
        public static string Convert(string Html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(Html);
            return htmlDocument.ParsedText;
        }
    }
}
