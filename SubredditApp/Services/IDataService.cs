using SubredditApp.Data.Entities;
using SubredditApp.Model;

namespace SubredditApp.Services
{
    /// <summary>
    /// Interface IDataService
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Adds the sub reddit.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Task.</returns>
        Task AddSubReddit(string name);
        /// <summary>
        /// Removes the sub reddit.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Task.</returns>
        Task RemoveSubReddit(string name);
        /// <summary>
        /// Lists the sub reddits.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;SubRedditEntity&gt;&gt;.</returns>
        Task<IEnumerable<SubRedditEntity>> ListSubReddits();
        /// <summary>
        /// Gets the users with most post.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <param name="authorCount">The author count.</param>
        /// <returns>Task&lt;List&lt;UserByPostDto&gt;&gt;.</returns>
        Task<List<UserByPostDto>> GetUsersWithMostPost(string subreddit, int authorCount = 10);
        /// <summary>
        /// Adds the post.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <returns>Task.</returns>
        Task AddPost(Subreddit subreddit);
        /// <summary>
        /// Gets the posts with highest score.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <param name="postsCount">The posts count.</param>
        /// <returns>Task&lt;List&lt;PostsScoreDto&gt;&gt;.</returns>
        Task<List<PostsScoreDto>> GetPostsWithHighestScore(string subreddit, int postsCount = 10);
        /// <summary>
        /// Counts the posts by subreddit.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> CountPostsBySubreddit(string subreddit);
    }
}
