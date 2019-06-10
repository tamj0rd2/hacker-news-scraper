using HackerNewsScraper.Factories;
using HackerNewsScraper.Wrappers;
using System;

namespace HackerNewsScraper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var htmlWebWrapper = new HtmlWebWrapper();
            var postFactory = new PostFactory();
            var dataService = new DataService(htmlWebWrapper, postFactory);
            var posts = dataService.GetTopPosts();
            Console.WriteLine(posts);

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
