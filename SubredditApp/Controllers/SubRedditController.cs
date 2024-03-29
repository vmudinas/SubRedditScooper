using Microsoft.AspNetCore.Mvc;
using SubredditApp.Data.Entities;
using SubredditApp.Model;
using SubredditApp.Services;

namespace SubredditApp.Controllers
{
    /// <summary>
    /// Class SubRedditController.
    /// Implements the <see cref="ControllerBase" />
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class SubRedditController : ControllerBase
    {
        private readonly ILogger<SubRedditController> _logger;
        private readonly IDataService _dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubRedditController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dataService">The data service.</param>
        public SubRedditController(ILogger<SubRedditController> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }


        /// <summary>
        /// Saves the sub reddit.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IActionResult.</returns>
        [Route("SaveSubReddit")]
        [HttpPut]
        public async Task<IActionResult> SaveSubReddit(string name)
        {
            _logger.LogInformation($"SaveSubReddit: {name}");
            await _dataService.AddSubReddit(name);
            return Ok();
        }

        /// <summary>
        /// Lists the subreddits.
        /// </summary>
        /// <returns>IEnumerable&lt;SubRedditEntity&gt;.</returns>
        [Route("ListSubreddits")]
        [HttpGet]
        public async Task<IEnumerable<SubRedditEntity>> ListSubreddits()
        {
            _logger.LogInformation($"ListSubreddits");
            return await _dataService.ListSubReddits();
        }

        /// <summary>
        /// Removes the sub reddit.
        /// </summary>
        /// <param name="name">The name.</param>
        [Route("RemoveSubReddit")]
        [HttpDelete]
        public async Task RemoveSubReddit(string name)
        {
            _logger.LogInformation($"RemoveSubReddit: {name}");
            await _dataService.RemoveSubReddit(name);
        }

        /// <summary>
        /// Removes the sub reddit.
        /// </summary>
        /// <param name="name">The name.</param>
        [Route("GetPosts")]
        [HttpPost]
        public async Task<IEnumerable<PostDto>> GetPosts(string subredditName, int skip, int take)
        {
            _logger.LogInformation($"GetPosts: {subredditName} skip:{skip} take:{take}");
            return await _dataService.GetPosts(subredditName, skip, take);
        }
    }
}
