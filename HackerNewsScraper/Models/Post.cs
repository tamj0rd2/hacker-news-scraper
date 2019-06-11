using Newtonsoft.Json;
using System;

namespace HackerNewsScraper.Models
{
    [Serializable]
    public class Post
    {
        [JsonProperty("title", Order = 1)]
        public string Title { get; set; }

        [JsonProperty("uri", Order = 2)]
        public string Uri { get; set; }

        [JsonProperty("author", Order = 3)]
        public string Author { get; set; }

        [JsonProperty("points", Order = 4)]
        public int Points { get; set; }

        [JsonProperty("comments", Order = 5)]
        public int CommentsCount { get; set; }

        [JsonProperty("rank", Order = 6)]
        public int Rank { get; set; }
    }
}
