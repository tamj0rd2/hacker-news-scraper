using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace HackerNewsScraper.Wrappers
{
    public class HtmlWebWrapper
    {
        public virtual HtmlNodeWrapper Load(string url)
        {
            var htmlDocument = new HtmlWeb().Load(url);
            return new HtmlNodeWrapper(htmlDocument.DocumentNode);
        }
    }

    public class HtmlNodeWrapper
    {
        private readonly HtmlNode node;

        public HtmlNodeWrapper(HtmlNode node)
        {
            this.node = node;
        }

        public virtual IEnumerable<HtmlNodeWrapper> SelectNodes(string xpath)
        {
            return this.node.SelectNodes(xpath).Select(node => new HtmlNodeWrapper(node));
        }

        public virtual string GetInnerHtml(string xpath)
        {
            return this.node.SelectSingleNode(xpath).InnerHtml;
        }

        public virtual string GetAttributeValue(string name, string xpath, string defaultValue = null)
        {
            return this.node.SelectSingleNode(xpath).GetAttributeValue(name, defaultValue);
        }
    }
}
