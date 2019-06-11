using HackerNewsScraper.Models;
using HackerNewsScraper.Wrappers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace HackerNewsScraper.UnitTests
{
    [TestFixture]
    public class HackerNewsScraperTests
    {
        private Mock<SystemWrapper> mockSystemWrapper;
        private Mock<JsonWrapper> mockJsonWrapper;
        private Mock<DataService> mockDataService;
        private HackerNewsScraper hackerNewsScraper;

        private readonly string jsonSerialiserOutput = "The posts in json";

        [SetUp]
        public void SetUp()
        {
            this.mockSystemWrapper = new Mock<SystemWrapper>(MockBehavior.Strict);
            this.mockSystemWrapper.Setup(x => x.WriteLine(It.IsNotNull<object>()));
            this.mockSystemWrapper.Setup(x => x.WriteInvalidNumOfPosts());
            this.mockSystemWrapper.Setup(x => x.ExitAppWithErrorCode());
            this.mockSystemWrapper.Setup(x => x.WriteUsageInformation());

            this.mockJsonWrapper = new Mock<JsonWrapper>(MockBehavior.Strict);
            this.mockJsonWrapper.Setup(x => x.SerializeObject(It.IsNotNull<object>())).Returns(string.Empty);

            this.mockDataService = new Mock<DataService>(MockBehavior.Strict, null, null);
            this.mockDataService.Setup(x => x.GetTopPosts(It.IsAny<int>())).Returns(new List<Post>());

            this.hackerNewsScraper = new HackerNewsScraper(
                this.mockSystemWrapper.Object, this.mockDataService.Object, this.mockJsonWrapper.Object);
        }

        [Test]
        public void Run_GivesUsageInformationAndExists_WhenNoArgsProvided()
        {
            // Arrange
            var args = new string[] { };

            // Act
            this.hackerNewsScraper.Run(args);

            // Assert
            this.mockSystemWrapper.Verify(x => x.WriteUsageInformation(), Times.Once);
            this.mockSystemWrapper.Verify(x => x.ExitAppWithErrorCode(), Times.Once);
        }

        [Test]
        public void Run_GivesUsageInformationAndExists_WhenMissingArgs()
        {
            // Arrange
            var args = new[] { "--posts" };

            // Act
            this.hackerNewsScraper.Run(args);

            // Assert
            this.mockSystemWrapper.Verify(x => x.WriteUsageInformation(), Times.Once);
            this.mockSystemWrapper.Verify(x => x.ExitAppWithErrorCode(), Times.Once);
        }

        [Test]
        public void Run_GivesUsageInformationAndExists_WhenTooManyArgs()
        {
            // Arrange
            var args = new[] { "--posts", "10", "what" };

            // Act
            this.hackerNewsScraper.Run(args);

            // Assert
            this.mockSystemWrapper.Verify(x => x.WriteUsageInformation(), Times.Once);
            this.mockSystemWrapper.Verify(x => x.ExitAppWithErrorCode(), Times.Once);
        }

        [Test]
        public void Run_GivesUsageInformationAndExists_WhenFirstArgNotPosts()
        {
            // Arrange
            var args = new[] { "whatwhat", "12" };

            // Act
            this.hackerNewsScraper.Run(args);

            // Assert
            this.mockSystemWrapper.Verify(x => x.WriteUsageInformation(), Times.Once);
            this.mockSystemWrapper.Verify(x => x.ExitAppWithErrorCode(), Times.Once);
        }

        [Test]
        public void Run_GivesUsageInformationAndExists_WhenSecondArgNotANumber()
        {
            // Arrange
            var args = new[] { "--posts", "whatwhat" };

            // Act
            this.hackerNewsScraper.Run(args);

            // Assert
            this.mockSystemWrapper.Verify(x => x.WriteInvalidNumOfPosts(), Times.Once);
            this.mockSystemWrapper.Verify(x => x.ExitAppWithErrorCode(), Times.Once);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(101)]
        [TestCase(150)]
        public void Run_GivesUsageInformationAndExists_WhenNumOfPostsOutOfValidRange(int numOfPosts)
        {
            // Arrange
            var args = new[] { "--posts", numOfPosts.ToString() };

            // Act
            this.hackerNewsScraper.Run(args);

            // Assert
            this.mockSystemWrapper.Verify(x => x.WriteInvalidNumOfPosts(), Times.Once);
            this.mockSystemWrapper.Verify(x => x.ExitAppWithErrorCode(), Times.Once);
        }

        [Test]
        public void Run_CallsDataService_WithCorrectNumberOfPosts()
        {
            // Arrange
            var args = new[] { "--posts", "15" };

            // Act
            this.hackerNewsScraper.Run(args);

            // Assert
            this.mockDataService.Verify(x => x.GetTopPosts(15), Times.Once);
        }

        [Test]
        public void Run_CallsJsonService_WithPostsFromDataService()
        {
            // Arrange
            var posts = new List<Post> { new Post(), new Post(), new Post() };
            this.mockDataService.Setup(x => x.GetTopPosts(15)).Returns(posts);

            // Act
            this.hackerNewsScraper.Run(new[] { "--posts", "15" });

            // Assert
            this.mockJsonWrapper.Verify(x => x.SerializeObject(posts), Times.Once);
        }

        [Test]
        public void Run_WritesOututFromJsonService()
        {
            // Arrange
            this.mockJsonWrapper.Setup(x => x.SerializeObject(It.IsNotNull<object>())).Returns(this.jsonSerialiserOutput);

            // Act
            this.hackerNewsScraper.Run(new[] { "--posts", "15" });

            // Assert
            this.mockSystemWrapper.Verify(x => x.WriteLine(this.jsonSerialiserOutput), Times.Once);
        }
    }
}
