using HackerNewsScraper.Factories;
using HackerNewsScraper.Models;
using HackerNewsScraper.Wrappers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HackerNewsScraper
{
    public class DataService
    {
        private readonly HtmlWebWrapper htmlWebWrapper;
        private readonly PostFactory postFactory;

        public DataService(HtmlWebWrapper htmlWebWrapper, PostFactory postFactory)
        {
            this.htmlWebWrapper = htmlWebWrapper;
            this.postFactory = postFactory;
        }

        public IEnumerable<Post> GetTopPosts()
        {
            var validPosts = new List<Post>();
            var documentNode = this.htmlWebWrapper.Load("https://news.ycombinator.com/news");
            var nodes = documentNode.SelectNodes(@"//table[@class='itemlist']//tr").ToList();

            for (int i = 0; i < nodes.Count; i += 3)
            {
                var primaryNode = nodes[i];
                var secondaryNode = nodes[i + 1];

                try
                {
                    var post = this.postFactory.CreatePost(primaryNode, secondaryNode);
                    validPosts.Add(post);
                }
                catch (ValidationException e)
                {
                    // TODO should there be logging for malformed posts?
                }
            }

            return validPosts;
        }
    }
}
