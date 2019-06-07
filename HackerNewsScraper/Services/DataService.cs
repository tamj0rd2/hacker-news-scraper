using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HackerNewsScraper
{
    public class DataService
    {
        private readonly HackerNewsRepository hackerNewsRepository;

        public DataService(HackerNewsRepository hackerNewsRepository)
        {
            this.hackerNewsRepository = hackerNewsRepository;
        }

        public async Task<string> GetTopPosts()
        {
            var posts = await this.hackerNewsRepository.GetPostsAsync();
            return JsonConvert.SerializeObject(posts, Formatting.Indented);
        }
    }
}
