using Microsoft.AspNetCore.Mvc;
using SubredditApp.Model;
using SubredditApp.Services;

namespace SubredditApp.Controllers
{
    /// <summary>
    /// Class StatisticsController.
    /// Implements the <see cref="ControllerBase" />
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IDataService _dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dataService">The data service.</param>
        public StatisticsController(ILogger<StatisticsController> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        /// <summary>
        /// Tops the posts users.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <param name="userCount">The user count.</param>
        /// <returns>IEnumerable&lt;UserByPostDto&gt;.</returns>
        [Route("TopPostsUsers")]
        [HttpGet]
        public async Task<IEnumerable<UserByPostDto>> TopPostsUsers(string subreddit, int userCount = 10)
        {
            return await _dataService.GetUsersWithMostPost(subreddit, userCount);
        }

        /// <summary>
        /// Tops the posts.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <param name="postsCount">The posts count.</param>
        /// <returns>IEnumerable&lt;PostsScoreDto&gt;.</returns>
        [Route("TopPosts")]
        [HttpGet]
        public async Task<IEnumerable<PostsScoreDto>> TopPosts(string subreddit, int postsCount = 10)
        {
            return await _dataService.GetPostsWithHighestScore(subreddit, postsCount);
        }

        /// <summary>
        /// Counts the posts.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <returns>System.Int32.</returns>
        [Route("CountPosts")]
        [HttpGet]
        public async Task<int> CountPosts(string subreddit)
        {
            return await _dataService.CountPostsBySubreddit(subreddit);
        }
    }
}
