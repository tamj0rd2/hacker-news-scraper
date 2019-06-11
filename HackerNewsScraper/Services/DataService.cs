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

        public virtual IEnumerable<Post> GetTopPosts(int numOfPostsToGet)
        {
            var pageNumber = 1;
            var validPosts = new List<Post>();

            while (validPosts.Count < numOfPostsToGet)
            {
                var documentNode = this.htmlWebWrapper.Load($"https://news.ycombinator.com/news?p={pageNumber}");
                var nodes = documentNode.SelectNodes(@"//table[@class='itemlist']//tr").ToList();

                if (nodes.Count == 0)
                {
                    return validPosts;
                }

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
                        // TODO should there be logging for invalid posts
                    }

                    if (validPosts.Count == numOfPostsToGet)
                    {
                        return validPosts;
                    }
                }
            }

            return validPosts;
        }

        private IEnumerable<Post> GetSinglePageOfPosts(int pageNum)
        {
            var validPosts = new List<Post>();
            var documentNode = this.htmlWebWrapper.Load($"https://news.ycombinator.com/news?p={pageNum}");
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
                    // TODO should there be logging for invalid posts
                }
            }

            return validPosts;
        }
    }
}
