using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DrWndr.Utils;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DrWndr.Models
{
    /// <summary>
    /// Model of a simplified post.
    /// </summary>
    public class Post
    {
        #region JSON member

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
        /// Article url of the post.
        /// Renders as `article_url` to JSON.
        /// </summary>
        [JsonProperty("article_url")]
        public string ArticleUrl { get; set; }

        /// <summary>
        /// Article image url of the post.
        /// Renders as `article_image_url` to JSON.
        /// </summary>
        [JsonProperty("article_image_url")]
        public string ArticleImageUrl { get; set; }

        /// <summary>
        /// Article author.
        /// Renders as `author` to JSON.
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

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

        #region App members

        /// <summary>
        /// Determines the swipe status.
        /// Default value: Neutral.
        /// Ignored from JSON.
        /// </summary>
        [JsonIgnore]
        public SwipeStatus Status { get; set; }

        /// <summary>
        /// Gets the badge text.
        /// Ignored from JSON.
        /// </summary>
        [JsonIgnore]
        public string BadgeText
        {
            get
            {
                switch(Status)
                {
                    case SwipeStatus.Liked:
                        return "Liked";

                    case SwipeStatus.Disliked:
                        return "Disliked";

                    default:
                        return "Hot or Not?";
                }
            }
        }

        /// <summary>
        /// Gets the badge color.
        /// Ignored from JSON.
        /// </summary>
        [JsonIgnore]
        public Color BadgeColor
        {
            get
            {
                switch (Status)
                {
                    case SwipeStatus.Liked:
                        return Color.DarkOliveGreen;

                    case SwipeStatus.Disliked:
                        return Color.DarkRed;

                    default:
                        // OnPrimary
                        return Color.FromHex("FAFAFA");
                }
            }
        }

        /// <summary>
        /// Gets the computed subtitle of a post.
        /// Ignored from JSON.
        /// </summary>
        [JsonIgnore]
        public string Subtitle
        {
            get
            {
                return $"Geschrieben von {Author} am {PupDate:dd.mm.yyyy}";
            }
        }

        /// <summary>
        /// Gets the badge background color.
        /// Ignored from JSON.
        /// </summary>
        [JsonIgnore]
        public Color BadgeBackgroundColor
        {
            get
            {
                switch (Status)
                {
                    case SwipeStatus.Liked:
                        return Color.DarkSeaGreen;

                    case SwipeStatus.Disliked:
                        return Color.IndianRed;

                    default:
                        // Primary.
                        return Color.FromHex("213868").MultiplyAlpha(0.6);
                }
            }
        }

        #endregion

        #region Public helper

        /// <summary>
        /// Gets all posts from data json file.
        /// </summary>
        /// <returns>Found posts.</returns>
        static internal List<Post> GetAll()
        {
            // Read json data from disk.
            var assembly = typeof(Post).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("DrWndr.Data.result.json");
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            // Parse json into objects.
            var list = JsonConvert.DeserializeObject<List<Post>>(json);
            reader.Close();

            // Shuffle list randomly.
            list.Shuffle();

            // Return found, parsed and radomized list.
            return list;
        }

        #endregion

        #region Private constants

        /// <summary>
        /// Fallback for the image source of a post.
        /// </summary>
        private const string ImagesourceFallback = "https://www.drwindows.de/news/wp-content/themes/drwindows_theme/img/DrWindows-Windows-News.png";

        #endregion

        #region Constructor

        /// <summary>
        /// Empty constructor used by the deserializer.
        /// </summary>
        public Post()
        {
            Status = SwipeStatus.Neutral;
        }

        /// <summary>
        /// Post constructor to create a new instance from
        /// a given SyndicationItem.
        /// </summary>
        /// <param name="item">SyndicationItem input.</param>
        public Post(SyndicationItem item)
        {
            // Read basic attributes.
            Id = item.Id;
            Title = item.Title.Text;
            Summary = GetTextFromHtml(item.Summary.Text);
            ArticleUrl = item.Links[0].Uri.ToString();
            ArticleImageUrl = GetImageSourceOutOfContent(item.Summary.Text);
            PupDate = item.PublishDate;

            // Read custom attributes
            Author = item.ElementExtensions
                .ReadElementExtensions<XElement>("creator", "http://purl.org/dc/elements/1.1/")
                .Select(e => e.Value)
                .First();

            // Post processing
            //  1.  If author is "Dr. Windows" which is used for "guest articles"
            //      Change the author name to "einem Gast".
            if (Author == "Dr. Windows") Author = "einem Gast";
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
