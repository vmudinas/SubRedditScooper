using Microsoft.EntityFrameworkCore;
using SubredditApp.Data.Entities;

namespace SubredditApp.Data
{
    /// <summary>
    /// Class ApplicationDbContext.
    /// Implements the <see cref="DbContext" />
    /// </summary>
    /// <seealso cref="DbContext" />
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Gets or sets the sub reddit.
        /// </summary>
        /// <value>The sub reddit.</value>
        public DbSet<SubRedditEntity> SubReddit { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>The posts.</value>
        public DbSet<PostEntity> Posts { get; set; }

        #region Required
        /// <summary>
        /// Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the one-to-many relationship
            modelBuilder.Entity<PostEntity>()
                .HasOne(p => p.SubReddit)   // Each PostEntity has one SubRedditEntity
                .WithMany(s => s.Posts)     // Each SubRedditEntity has many PostEntity items
                .HasForeignKey(p => p.SubRedditId); // The foreign key in PostEntity
        }
        #endregion
    }
}
