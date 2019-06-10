using HackerNewsScraper.Models;
using HackerNewsScraper.Wrappers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HackerNewsScraper.Factories
{
    public class PostFactory
    {
        public const string TitleBlank = "Title could not be parsed from the HTML. It was blank";
        public const string TitleTooLong = "Title parsed from the HTML was longer than 256 characters";
        public const string AuthorBlank = "Author could not be parsed from the HTML. It was blank";
        public const string AuthorTooLong = "Author parsed from the HTML was longer than 256 characters";
        public const string UriInvalid = "Uri parsed from the HTML is invalid";
        public const string PointsLessThanZero = "Points were less than 0";
        public const string CommentsLessThanZero = "Comments were less than 0";
        public const string RankLessThanZero = "Rank was less than 0";

        private readonly string intOnlyRegex = @"[^0-9]";

        private readonly string pointsXpath = @"//span[@class='score']";
        private readonly string commentsXpath = @"//td[@class='subtext']/a[last()]";
        private readonly string rankXpath = @"//span[@class='rank']";

        public virtual Post CreatePost(HtmlNodeWrapper primaryNode, HtmlNodeWrapper secondaryNode)
        {
            return new Post
            {
                Title = this.ParseTitle(primaryNode),
                Author = this.ParseAuthor(secondaryNode),
                Uri = this.ParseUri(primaryNode),
                Points = this.ParsePositiveInt(secondaryNode, pointsXpath, PointsLessThanZero),
                CommentsCount = this.ParsePositiveInt(secondaryNode, commentsXpath, CommentsLessThanZero),
                Rank = this.ParsePositiveInt(primaryNode, rankXpath, RankLessThanZero),
            };
        }

        private string ParseTitle(HtmlNodeWrapper primaryNode)
        {
            var title = primaryNode.GetInnerHtml(@"//a[@class='storylink']");
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ValidationException(TitleBlank);
            }
            
            if (title.Length > 256)
            {
                throw new ValidationException(TitleTooLong);
            }

            return title;
        }

        private string ParseAuthor(HtmlNodeWrapper secondaryNode)
        {
            var author = secondaryNode.GetInnerHtml(@"//a[@class='hnuser']");
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ValidationException(AuthorBlank);
            }

            if (author.Length > 256)
            {
                throw new ValidationException(AuthorTooLong);
            }

            return author;
        }

        private string ParseUri(HtmlNodeWrapper primaryNode)
        {
            var uri = primaryNode.GetAttributeValue("href", @"//a[@class='storylink']", null);
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                throw new ValidationException(UriInvalid);
            }

            return uri;
        }

        private int ParsePositiveInt(HtmlNodeWrapper node, string xpath, string numNegativeMessage)
        {
            var innerHtml = node.GetInnerHtml(xpath);

            if (innerHtml.StartsWith('-'))
            {
                throw new ValidationException(numNegativeMessage);
            }

            var strippedValue = Regex.Replace(innerHtml, intOnlyRegex, string.Empty);
            return Convert.ToInt32(strippedValue);
        }
    }
}
