namespace SubredditApp.Model
{
    /// <summary>
    /// Class Subreddit.
    /// </summary>
    public class Subreddit
    {
        /// <summary>
        /// Gets or sets the before.
        /// </summary>
        /// <value>The before.</value>
        public string Before { get; set; }
        /// <summary>
        /// Gets or sets the after.
        /// </summary>
        /// <value>The after.</value>
        public string After { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>The posts.</value>
        public List<PostDto> Posts { get; set; }
    }
}
