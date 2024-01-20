using SubredditApp.Model;

namespace SubredditApp.Service
{
    /// <summary>
    /// Interface IRedditService
    /// </summary>
    public interface IRedditService
    {
        /// <summary>
        /// Gets the subreddit asynchronous.
        /// </summary>
        /// <param name="subredditName">Name of the subreddit.</param>
        /// <param name="before">The before.</param>
        /// <param name="after">The after.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>Task&lt;Subreddit&gt;.</returns>
        Task<Subreddit> GetSubredditAsync(string subredditName, string before = null, string after = null, int limit = 100);
    }
}