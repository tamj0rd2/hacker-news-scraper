using System;

namespace HackerNewsScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var hackerNewsRepository = new HackerNewsRepository();
            var dataService = new DataService(hackerNewsRepository);
            var posts = dataService.GetTopPosts().GetAwaiter().GetResult();
            Console.WriteLine(posts);

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
