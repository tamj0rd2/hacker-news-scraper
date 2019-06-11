using System;

namespace HackerNewsScraper.Wrappers
{
    public class SystemWrapper
    {
        public virtual void WriteLine(object value)
        {
            Console.WriteLine(value);
        }

        public virtual void WriteUsageInformation()
        {
            Console.WriteLine("Command usage: hackernews --posts n");
            Console.WriteLine("> --post is the flag to set the number of posts to display");
            Console.WriteLine("> n is the number of posts and must be from 1-100 (inclusive)");
        }

        public virtual void WriteInvalidNumOfPosts()
        {
            Console.WriteLine("The number of posts to display should be from 1-100, inclusive");
        }

        public virtual void ExitAppWithErrorCode()
        {
#if DEBUG
            Console.ReadLine();
#endif

            Environment.Exit(1);
        }
    }
}
