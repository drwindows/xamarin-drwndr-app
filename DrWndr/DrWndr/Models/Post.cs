using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace DrWndr.Models
{
    /// <summary>
    /// Model of a simplified post.
    /// </summary>
    public class Post
    {
        #region Members

        /// <summary>
        /// Id of the post.
        /// Renders as `id` to JSON.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Title of the post.
        /// Renders as `tile` to JSON.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Article image url of the post.
        /// Renders as `article_image_url` to JSON.
        /// </summary>
        [JsonProperty("article_image_url")]
        public string ArticleImageUrl { get; set; }

        /// <summary>
        /// Plain text summary of the post.
        /// Renders as `summary` to JSON.
        /// </summary>
        [JsonProperty("summary")]
        public string Summary { get; set; }

        /// <summary>
        /// Published date of the post.
        /// Renders as `pub_date` to JSON.
        /// </summary>
        [JsonProperty("pub_date")]
        public DateTimeOffset PupDate { get; set; }

        #endregion

        #region Public helper

        /// <summary>
        /// Gets all posts from data json file.
        /// </summary>
        /// <returns>Found posts.</returns>
        static internal List<Post> GetAll()
        {
            var assembly = typeof(Post).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("DrWndr.Data.result.json");

            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var foo = JsonConvert.DeserializeObject<List<Post>>(json);
            reader.Close();
            return foo;
        }

        #endregion

        #region Private constants

        /// <summary>
        /// Fallback for the image source of a post.
        /// </summary>
        private const string ImagesourceFallback = "https://www.drwindows.de/news/wp-content/themes/drwindows_theme/img/DrWindows-Windows-News.png";

        #endregion

        #region Constructor

        public Post()
        {

        }

        /// <summary>
        /// Post constructor to create a new instance from
        /// a given SyndicationItem.
        /// </summary>
        /// <param name="item">SyndicationItem input.</param>
        public Post(SyndicationItem item)
        {
            Id = item.Id;
            Title = item.Title.Text;
            Summary = GetTextFromHtml(item.Summary.Text);
            ArticleImageUrl = GetImageSourceOutOfContent(item.Summary.Text);
            PupDate = item.PublishDate;
        }

        #endregion

        #region Private helper

        /// <summary>
        /// Gets plain text from html string.
        /// </summary>
        /// <param name="html">Html input string.</param>
        /// <returns>Pure text from html.</returns>
        private static string GetTextFromHtml(string html)
        {
            return Regex.Replace(html, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Gets the image source out of a content string.
        /// </summary>
        /// <param name="content">Content string of a post.</param>
        /// <returns>Found or fallback image source.</returns>
        private static string GetImageSourceOutOfContent(string content)
        {
            // Try to extract the first `<img src=".." /> out of the content string 
            // to use as image source.
            var source = Regex.Match(content, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase)
                .Groups[1]
                .Value;

            // If no image found in content, use fallback.
            return source.Length != 0 ? source : ImagesourceFallback;
        }

        #endregion
    }
}
