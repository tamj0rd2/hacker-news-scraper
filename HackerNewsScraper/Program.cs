using HackerNewsScraper.Factories;
using HackerNewsScraper.Wrappers;
using System;

namespace HackerNewsScraper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var systemWrapper = new SystemWrapper();
            var dataService = new DataService(new HtmlWebWrapper(), new PostFactory());
            var jsonWrapper = new JsonWrapper();
            new HackerNewsScraper(systemWrapper, dataService, jsonWrapper).Run(args);

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
