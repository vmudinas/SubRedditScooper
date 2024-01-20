using System.ComponentModel.DataAnnotations;

namespace SubredditApp.Data.Entities
{
    /// <summary>
    /// Class SubRedditEntity.
    /// </summary>
    public class SubRedditEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the before.
        /// </summary>
        /// <value>The before.</value>
        public string? Before { get; set; }
        /// <summary>
        /// Gets or sets the after.
        /// </summary>
        /// <value>The after.</value>
        public string? After { get; set; }

        // Navigation property for related posts
        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>The posts.</value>
        public virtual ICollection<PostEntity> Posts { get; set; } = new HashSet<PostEntity>();
    }
}
