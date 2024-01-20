using SubredditApp.Data.Entities;
using SubredditApp.Model;
using SubredditApp.Services;

namespace SubredditApp.Service
{
    /// <summary>
    /// Class ProductShelfCloneBackgroundService.
    /// Implements the <see cref="BackgroundService" />
    /// </summary>
    /// <seealso cref="BackgroundService" />
    public class SubRedditPoolingService : BackgroundService
    {
        private readonly IServiceProvider _serviceScopeFactory;
        private readonly ILogger<SubRedditPoolingService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductShelfCloneBackgroundService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="serviceScopeFactory">The serviceScopeFactory.</param>
        public SubRedditPoolingService(
            ILogger<SubRedditPoolingService> logger,
            IServiceProvider serviceScopeFactory)
        {
            this._logger = logger;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Execute as an asynchronous operation.
        /// </summary>
        /// <param name="stoppingToken">Triggered when background service is called.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var subReddits = await GetSubReddits();
                foreach (var subreddit in subReddits)
                {
                    try
                    {
                        // Depending what comes back as rate limit 
                        // We can wait here
                        // Also we can store data from result in db here
                        var subredditData = await this.GetSubRedditData(subreddit);

                        if (subredditData != null && !string.IsNullOrWhiteSpace(subreddit.Name))
                        {
                            await this.AddPost(subredditData);
                        }
                    }
                    catch (ArgumentNullException ex)
                    {
                        this._logger.LogError($"Argument null exception: {ex.Message} {ex.InnerException?.Message}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        this._logger.LogError($"Invalid operation exception: {ex.Message} {ex.InnerException?.Message}");
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError($"Error while cloning product shelf {ex.Message} {ex.InnerException?.Message}");
                    }
                }

                await Task.Delay(300, stoppingToken).ConfigureAwait(false);
            }
        }

        private async Task<Subreddit?> GetSubRedditData(SubRedditEntity subreddit)
        {
            using var scope = this._serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var redditService = serviceProvider.GetRequiredService<IRedditService>();

            if (redditService != null && !string.IsNullOrEmpty(subreddit.Name))
            {
               return await redditService.GetSubredditAsync(subreddit.Name, subreddit?.Before, subreddit?.After).ConfigureAwait(false);
            }

            return null;
        }

        private async Task<IEnumerable<SubRedditEntity>> GetSubReddits()
        {
            using var scope = this._serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var dataService = serviceProvider.GetRequiredService<IDataService>();

            if (dataService != null)
            {
                return await dataService.ListSubReddits().ConfigureAwait(false);
            }

            return new List<SubRedditEntity>();
        }

        private async Task AddPost(Subreddit subreddit)
        {
            using var scope = this._serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var dataService = serviceProvider.GetRequiredService<IDataService>();

            if (dataService != null)
            {
                await dataService.AddPost(subreddit).ConfigureAwait(false);
            }
        }
    }
}