using HackerNewsScraper.Schema;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsScraper
{
    public class HackerNewsRepository
    {
        public virtual Task<IEnumerable<Post>> GetPostsAsync()
        {
            return Task.Run(() => this.CreateMockPosts());
        }

        private IEnumerable<Post> CreateMockPosts()
        {
            var posts = new List<Post>();

            for (var i = 1; i < 10; i++)
            {
                posts.Add(new Post
                {
                    Title = i.ToString(),
                    Author = "Tamara",
                    Comments = 3,
                    Points = 5,
                    Rank = i,
                    Uri = "http://some-url.com"
                });
            }

            return posts;
        }
    }
}
