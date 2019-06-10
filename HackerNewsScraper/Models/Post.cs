using Newtonsoft.Json;
using System;

namespace HackerNewsScraper.Models
{
    [Serializable]
    public class Post
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("comments")]
        public int Comments { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }
    }
}
