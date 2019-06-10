using HackerNewsScraper.Models;
using HackerNewsScraper.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace HackerNewsScraper.Factories
{
    public class PostFactory
    {
        public const string MissingNodes = "A required node could not be found in the HTML";
        public const string TitleBlank = "Title could not be parsed from the HTML. It was blank";
        public const string TitleTooLong = "Title parsed from the HTML was longer than 256 characters";
        public const string AuthorBlank = "Author could not be parsed from the HTML. It was blank";
        public const string AuthorTooLong = "Author parsed from the HTML was longer than 256 characters";
        public const string UriInvalid = "Uri parsed from the HTML is invalid";
        public const string PointsLessThanZero = "Points were less than 0";
        public const string CommentsLessThanZero = "Comments were less than 0";
        public const string RankLessThanZero = "Rank was less than 0";

        private readonly string intOnlyRegex = @"[^0-9]";
        private readonly IReadOnlyList<string> whitelistedValues = new[] { "discuss" };

        public virtual Post CreatePost(HtmlNodeWrapper primaryNode, HtmlNodeWrapper secondaryNode)
        {
            string titleHtml;
            string authorHtml;
            string postHref;
            string pointsHtml;
            string commentsHtml;
            string rankHtml;

            try
            {
                titleHtml = primaryNode.GetInnerHtml(@".//a[@class='storylink']");
                authorHtml = secondaryNode.GetInnerHtml(@".//a[@class='hnuser']");
                postHref = primaryNode.GetAttributeValue("href", @".//a[@class='storylink']", null);
                pointsHtml = secondaryNode.GetInnerHtml(@".//span[@class='score']");
                commentsHtml = secondaryNode.GetInnerHtml(@".//td[@class='subtext']/a[last()]");
                rankHtml = primaryNode.GetInnerHtml(@".//span[@class='rank']");
            }
            catch (NullReferenceException)
            {
                // a node could not be found on the page
                throw new ValidationException(MissingNodes);
            }

            return new Post
            {
                Title = this.ParseTitle(titleHtml),
                Author = this.ParseAuthor(authorHtml),
                Uri = this.ParseUri(postHref),
                Points = this.ParsePositiveInt(pointsHtml, PointsLessThanZero),
                CommentsCount = this.ParsePositiveInt(commentsHtml, CommentsLessThanZero),
                Rank = this.ParsePositiveInt(rankHtml, RankLessThanZero),
            };
        }

        private string ParseTitle(string title)
        {
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

        private string ParseAuthor(string author)
        {
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

        private string ParseUri(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                throw new ValidationException(UriInvalid);
            }

            return uri;
        }

        private int ParsePositiveInt(string innerHtml, string numNegativeMessage)
        {
            if (this.whitelistedValues.Contains(innerHtml.ToLower()))
            {
                return 0;
            }

            if (innerHtml.StartsWith('-'))
            {
                throw new ValidationException(numNegativeMessage);
            }

            var strippedValue = Regex.Replace(innerHtml, intOnlyRegex, string.Empty);
            return Convert.ToInt32(strippedValue);
        }
    }
}
