using FluentAssertions;
using HackerNewsScraper.Schema;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsScraper.UnitTests.Services
{
    [TestFixture]
    public class DataServiceTests
    {
        private Mock<HackerNewsRepository> mockRepository;
        private DataService dataService;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new Mock<HackerNewsRepository>(MockBehavior.Strict);
            this.dataService = new DataService(this.mockRepository.Object);
        }

        [Test]
        public async Task GetTopPostsAsync_ReturnsSerializedPosts_FromHackerNewsRepository()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post(),
                new Post(),
                new Post()
            };

            this.mockRepository.Setup(x => x.GetPostsAsync()).ReturnsAsync(posts);

            // Act
            var result = await this.dataService.GetTopPosts();

            // Assert
            this.mockRepository.Verify(x => x.GetPostsAsync(), Times.Once);
            result.Should().NotBeNull();
            var serialisedResult = JsonConvert.DeserializeObject<List<Post>>(result);
            serialisedResult.Should().NotBeNullOrEmpty();
            serialisedResult.Count.Should().Be(3);
        }
    }
}
