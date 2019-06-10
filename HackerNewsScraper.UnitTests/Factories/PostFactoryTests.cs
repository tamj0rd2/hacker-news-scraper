using FluentAssertions;
using HackerNewsScraper.Factories;
using HackerNewsScraper.Wrappers;
using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace HackerNewsScraper.UnitTests.Factories
{
    [TestFixture]
    public class PostFactoryTests
    {
        private PostFactory postFactory;

        [SetUp]
        public void SetUp()
        {
            this.postFactory = new PostFactory();
        }

        [Test]
        public void CreatePost_SetsPropertiesCorrectly()
        {
            // Arrange
            const string expectedTitle = "My first post!";
            var primaryNode = this.CreatePrimaryNodeMock(title: expectedTitle);
            var secondaryNode = this.CreateSecondaryNodeMock();

            // Act
            var post = this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Assert
            post.Title.Should().Be(expectedTitle);
        }

        [Test]
        public void CreatePost_SetsCorrectTitle()
        {
            // Arrange
            const string expectedTitle = "My first post!";
            var primaryNode = this.CreatePrimaryNodeMock(title: expectedTitle);
            var secondaryNode = this.CreateSecondaryNodeMock();

            // Act
            var post = this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Assert
            post.Title.Should().Be(expectedTitle);
        }

        [TestCase("")]
        [TestCase(null)]
        public void CreatePost_ThrowsValidationExceptionWhenTitleNotProvided(string title)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock(title: title);
            var secondaryNode = this.CreateSecondaryNodeMock();

            Action createPost = () => this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Act/Assert
            createPost.Should().ThrowExactly<ValidationException>().WithMessage(PostFactory.TitleBlank);
        }

        [Test]
        public void CreatePost_ThrowsValidationExceptionWhenTitleIsTooLong()
        {
            // Arrange
            var title = new string('a', 257);
            var primaryNode = this.CreatePrimaryNodeMock(title: title);
            var secondaryNode = this.CreateSecondaryNodeMock();

            Action createPost = () => this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Act/Assert
            createPost.Should().ThrowExactly<ValidationException>().WithMessage(PostFactory.TitleTooLong);
        }

        [Test]
        public void CreatePost_SetsCorrectAuthor()
        {
            // Arrange
            const string expectedAuthor = "Tam";
            var primaryNode = this.CreatePrimaryNodeMock();
            var secondaryNode = this.CreateSecondaryNodeMock(author: expectedAuthor);

            // Act
            var post = this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Assert
            post.Author.Should().Be(expectedAuthor);
        }

        [TestCase("")]
        [TestCase(null)]
        public void CreatePost_ThrowsValidationExceptionWhenAuthorNotProvided(string author)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock();
            var secondaryNode = this.CreateSecondaryNodeMock(author: author);

            Action createPost = () => this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Act/Assert
            createPost.Should().ThrowExactly<ValidationException>().WithMessage(PostFactory.AuthorBlank);
        }

        [Test]
        public void CreatePost_ThrowsValidationExceptionWhenAuthorIsTooLong()
        {
            // Arrange
            var author = new string('a', 257);
            var primaryNode = this.CreatePrimaryNodeMock();
            var secondaryNode = this.CreateSecondaryNodeMock(author: author);

            Action createPost = () => this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Act/Assert
            createPost.Should().ThrowExactly<ValidationException>().WithMessage(PostFactory.AuthorTooLong);
        }

        [TestCase("http://some-url.com")]
        [TestCase("https://some-url.io")]
        public void CreatePost_SetsCorrectUri(string expectedUri)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock(uri: expectedUri);
            var secondaryNode = this.CreateSecondaryNodeMock();

            // Act
            var post = this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Assert
            post.Uri.Should().Be(expectedUri);
        }

        [TestCase("notauri")]
        [TestCase("what!the!heck!")]
        [TestCase(null)]
        [TestCase("")]
        public void CreatePost_ThrowsValidationException_WhenUriNotValid(string uri)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock(uri: uri);
            var secondaryNode = this.CreateSecondaryNodeMock();

            Action createPost = () => this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Act/Assert
            createPost.Should().ThrowExactly<ValidationException>().WithMessage(PostFactory.UriInvalid);
        }

        [TestCase("0 points", 0)]
        [TestCase("1 point", 1)]
        [TestCase("123 points", 123)]
        public void CreatePost_SetsCorrectPoints(string points, int expectedPoints)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock();
            var secondaryNode = this.CreateSecondaryNodeMock(points: points);

            // Act
            var post = this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Assert
            post.Points.Should().Be(expectedPoints);
        }

        [TestCase("-1 point")]
        [TestCase("-1 points")]
        [TestCase("-1337 points")]
        public void CreatePost_ThrowsValidationException_WhenPointsLessThanZero(string points)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock();
            var secondaryNode = this.CreateSecondaryNodeMock(points: points);

            Action createPost = () => this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Act/Assert
            createPost.Should().ThrowExactly<ValidationException>().WithMessage(PostFactory.PointsLessThanZero);
        }

        [TestCase("0 comments", 0)]
        [TestCase("1 comment", 1)]
        [TestCase("123 comments", 123)]
        public void CreatePost_SetsCorrectCommentsCount(string comments, int expectedCommentsCount)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock();
            var secondaryNode = this.CreateSecondaryNodeMock(comments: comments);

            // Act
            var post = this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Assert
            post.CommentsCount.Should().Be(expectedCommentsCount);
        }

        [TestCase("-1 comment")]
        [TestCase("-1 comments")]
        [TestCase("-1337 comments")]
        public void CreatePost_ThrowsValidationException_WhenCommentsLessThanZero(string comments)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock();
            var secondaryNode = this.CreateSecondaryNodeMock(comments: comments);

            Action createPost = () => this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Act/Assert
            createPost.Should().ThrowExactly<ValidationException>().WithMessage(PostFactory.CommentsLessThanZero);
        }

        [TestCase("0.", 0)]
        [TestCase("1.", 1)]
        [TestCase("99.", 99)]
        public void CreatePost_SetsCorrectRank(string rank, int expectedRank)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock(rank: rank);
            var secondaryNode = this.CreateSecondaryNodeMock();

            // Act
            var post = this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Assert
            post.Rank.Should().Be(expectedRank);
        }

        [TestCase("-1.")]
        [TestCase("-1337.")]
        public void CreatePost_ThrowsValidationException_WhenRankLessThanZero(string rank)
        {
            // Arrange
            var primaryNode = this.CreatePrimaryNodeMock(rank: rank);
            var secondaryNode = this.CreateSecondaryNodeMock();

            Action createPost = () => this.postFactory.CreatePost(primaryNode.Object, secondaryNode.Object);

            // Act/Assert
            createPost.Should().ThrowExactly<ValidationException>().WithMessage(PostFactory.RankLessThanZero);
        }

        private Mock<HtmlNodeWrapper> CreatePrimaryNodeMock(
            string rank = "1.", string title = "First post", string uri = "http://some-url.com")
        {
            var primaryNode = new Mock<HtmlNodeWrapper>(MockBehavior.Strict, null);
            primaryNode.Setup(x => x.GetInnerHtml(@"//span[@class='rank']")).Returns(rank);
            primaryNode.Setup(x => x.GetInnerHtml(@"//a[@class='storylink']")).Returns(title);
            primaryNode.Setup(x => x.GetAttributeValue("href", @"//a[@class='storylink']", null)).Returns(uri);
            return primaryNode;
        }

        private Mock<HtmlNodeWrapper> CreateSecondaryNodeMock(
            string points = "123 points", string author = "tamara", string comments = "15 comments")
        {
            var secondaryNode = new Mock<HtmlNodeWrapper>(MockBehavior.Strict, null);
            secondaryNode.Setup(x => x.GetInnerHtml(@"//span[@class='score']")).Returns(points);
            secondaryNode.Setup(x => x.GetInnerHtml(@"//a[@class='hnuser']")).Returns(author);
            secondaryNode.Setup(x => x.GetInnerHtml(@"//td[@class='subtext']/a[last()]")).Returns(comments);
            return secondaryNode;
        }
    }
}
