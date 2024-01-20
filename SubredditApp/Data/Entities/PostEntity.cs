namespace SubredditApp.Data.Entities
{
    /// <summary>
    /// Class PostEntity.
    /// </summary>
    public class PostEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the number of comments.
        /// </summary>
        /// <value>The number of comments.</value>
        public int NumberOfComments { get; set; }

        /// <summary>
        /// Gets or sets the created UTC.
        /// </summary>
        /// <value>The created UTC.</value>
        public DateTime CreatedUTC { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        // Foreign key property
        /// <summary>
        /// Gets or sets the sub reddit identifier.
        /// </summary>
        /// <value>The sub reddit identifier.</value>
        public int SubRedditId { get; set; }

        // Navigation property for related subreddit
        /// <summary>
        /// Gets or sets the sub reddit.
        /// </summary>
        /// <value>The sub reddit.</value>
        public virtual SubRedditEntity SubReddit { get; set; }
    }
}
