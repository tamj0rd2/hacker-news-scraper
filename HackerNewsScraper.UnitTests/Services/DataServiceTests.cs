using FluentAssertions;
using HackerNewsScraper.Factories;
using HackerNewsScraper.Models;
using HackerNewsScraper.Wrappers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HackerNewsScraper.UnitTests.Services
{
    [TestFixture]
    public class DataServiceTests
    {
        private Mock<HtmlNodeWrapper> mockDocumentNode;
        private Mock<HtmlWebWrapper> mockHtmlWebWrapper;
        private Mock<PostFactory> mockPostFactory;
        private DataService dataService;

        [SetUp]
        public void SetUp()
        {
            this.mockDocumentNode = new Mock<HtmlNodeWrapper>(MockBehavior.Strict, null);
            this.mockHtmlWebWrapper = new Mock<HtmlWebWrapper>(MockBehavior.Strict);
            this.mockHtmlWebWrapper.Setup(x => x.Load(It.IsNotNull<string>())).Returns(this.mockDocumentNode.Object);
            this.mockPostFactory = new Mock<PostFactory>(MockBehavior.Strict);
            this.mockPostFactory
                .Setup(x => x.CreatePost(It.IsNotNull<HtmlNodeWrapper>(), It.IsNotNull<HtmlNodeWrapper>()))
                .Returns(new Post());
            this.dataService = new DataService(this.mockHtmlWebWrapper.Object, this.mockPostFactory.Object);
        }

        [Test]
        public void GetTopPosts_CallsPostFactory_WithCorrectArgs()
        {
            // Arrange
            var primaryNode = new Mock<HtmlNodeWrapper>(MockBehavior.Strict, null).Object;
            var secondaryNode = new Mock<HtmlNodeWrapper>(MockBehavior.Strict, null).Object;
            var spacerNode = new Mock<HtmlNodeWrapper>(MockBehavior.Strict, null).Object;
            var nodes = new List<HtmlNodeWrapper> { primaryNode, secondaryNode, spacerNode };
            this.mockDocumentNode.Setup(x => x.SelectNodes(It.IsNotNull<string>())).Returns(nodes);

            // Act
            var result = this.dataService.GetTopPosts();

            // Assert
            this.mockPostFactory.Verify(x => x.CreatePost(primaryNode, secondaryNode), Times.Once);
        }

        [Test]
        public void GetTopPosts_CallsPostFactory_CorrectNumberOfTimes()
        {
            // Arrange
            var nodes = this.CreateNodesForSinglePost();
            nodes.AddRange(this.CreateNodesForSinglePost());
            nodes.AddRange(this.CreateNodesForSinglePost());
            nodes.AddRange(this.CreateNodesForSinglePost());
            this.mockDocumentNode.Setup(x => x.SelectNodes(It.IsNotNull<string>())).Returns(nodes);

            // Act
            var result = this.dataService.GetTopPosts();

            // Assert
            this.mockPostFactory.Verify(
                x => x.CreatePost(It.IsNotNull<HtmlNodeWrapper>(), It.IsNotNull<HtmlNodeWrapper>()),
                Times.Exactly(4));
        }

        [Test]
        public void GetTopPosts_OnlyReturnsPostsThatDontThrowValidationExceptions()
        {
            // Arrange
            var nodes = this.CreateNodesForSinglePost();
            nodes.AddRange(this.CreateNodesForSinglePost());
            nodes.AddRange(this.CreateNodesForSinglePost());
            nodes.AddRange(this.CreateNodesForSinglePost());
            this.mockDocumentNode.Setup(x => x.SelectNodes(It.IsNotNull<string>())).Returns(nodes);

            this.mockPostFactory
                .SetupSequence(x => x.CreatePost(It.IsNotNull<HtmlNodeWrapper>(), It.IsNotNull<HtmlNodeWrapper>()))
                .Returns(new Post { Title = "Post1" })
                .Throws<ValidationException>()
                .Throws<ValidationException>()
                .Returns(new Post { Title = "Post4" });

            // Act
            var result = this.dataService.GetTopPosts().ToList();

            // Assert
            result.Count.Should().Be(2);
            result[0].Title.Should().Be("Post1");
            result[1].Title.Should().Be("Post4");
        }

        private List<HtmlNodeWrapper> CreateNodesForSinglePost()
        {
            var primaryNode = new Mock<HtmlNodeWrapper>(MockBehavior.Strict, null).Object;
            var secondaryNode = new Mock<HtmlNodeWrapper>(MockBehavior.Strict, null).Object;
            var spacerNode = new Mock<HtmlNodeWrapper>(MockBehavior.Strict, null).Object;
            return new List<HtmlNodeWrapper> { primaryNode, secondaryNode, spacerNode };
        }
    }
}
