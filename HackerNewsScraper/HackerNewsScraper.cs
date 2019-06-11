using HackerNewsScraper.Factories;
using HackerNewsScraper.Wrappers;
using System;

namespace HackerNewsScraper
{
    public class HackerNewsScraper
    {
        private readonly SystemWrapper systemWrapper;
        private readonly DataService dataService;
        private readonly JsonWrapper jsonWrapper;

        public HackerNewsScraper(
            SystemWrapper systemWrapper, DataService dataService, JsonWrapper jsonWrapper)
        {
            this.systemWrapper = systemWrapper;
            this.dataService = dataService;
            this.jsonWrapper = jsonWrapper;
        }

        public void Run(string[] args)
        {
            var postsToDisplay = this.GetNumOfPostsToDisplay(args);
            if (postsToDisplay == -1)
            {
                this.systemWrapper.ExitAppWithErrorCode();
                return;
            }

            // TODO error handling could be taken care of here
            var posts = this.dataService.GetTopPosts(postsToDisplay);
            var output = this.jsonWrapper.SerializeObject(posts);
            this.systemWrapper.WriteLine(output);
        }

        private int GetNumOfPostsToDisplay(string[] args)
        {
            if (args.Length != 2 || !string.Equals(args[0], "--posts"))
            {
                this.systemWrapper.WriteUsageInformation();
                return -1;
            }

            if (!int.TryParse(args[1], out var postsToDisplay) || postsToDisplay <= 0 || postsToDisplay > 100)
            {
                this.systemWrapper.WriteInvalidNumOfPosts();
                return -1;
            }

            return postsToDisplay;
        }
    }
}
