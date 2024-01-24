using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SubredditApp.Data;
using SubredditApp.Data.Entities;
using SubredditApp.Model;
using SubredditApp.Service;

namespace SubredditApp.Services
{
    /// <summary>
    /// Class DataService.
    /// Implements the <see cref="SubredditApp.Services.IDataService" />
    /// </summary>
    /// <seealso cref="SubredditApp.Services.IDataService" />
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _context;
                private readonly ILogger<DataService> _logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="DataService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public DataService(ApplicationDbContext dbContext, ILogger<DataService> logger)
        {
            _context = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Adds the post.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        public async Task AddPost(Subreddit subreddit)
        {
            var dbSubreddit = await _context.SubReddit.FirstOrDefaultAsync(x => x.Name == subreddit.Name);

            if (subreddit.Posts != null && dbSubreddit?.Id != null)
            {
                dbSubreddit.After = subreddit.After;
                dbSubreddit.Before = subreddit.Before;
                _context.Entry(dbSubreddit).CurrentValues.SetValues(subreddit);

                foreach (var posts in subreddit.Posts)
                {
                    var existingProduct = _context.Posts.FirstOrDefault(p => p.Id == posts.Id);

                    if (existingProduct == null)
                    {
                        // Add new posts
                        await _context.Posts.AddAsync(new Data.Entities.PostEntity
                        {
                            Author = posts.Author,
                            Created = posts.Created,
                            CreatedUTC = posts.CreatedUTC,
                            Id = posts.Id,
                            NumberOfComments = posts.NumberOfComments,
                            Score = posts.Score,
                            Thumbnail = posts.Thumbnail,
                            Title = posts.Title,
                            Url = posts.Url,
                            SubRedditId = dbSubreddit.Id
                        });

                        this._logger.LogInformation($"AddPost: {posts.Title}");
                    }
                    else
                    {
                        // Update existing posts
                        _context.Entry(existingProduct).CurrentValues.SetValues(new Data.Entities.PostEntity
                        {
                            Author = posts.Author,
                            Created = posts.Created,
                            CreatedUTC = posts.CreatedUTC,
                            Id = posts.Id,
                            NumberOfComments = posts.NumberOfComments,
                            Score = posts.Score,
                            Thumbnail = posts.Thumbnail,
                            Title = posts.Title,
                            Url = posts.Url,
                            SubRedditId = dbSubreddit.Id
                        });

                        this._logger.LogInformation($"Update posts: {posts.Title}");
                    }                    
                }

               var result = await _context.SaveChangesAsync();
               this._logger.LogInformation($"Add/Update result: {result}"); 
            }
        }

        /// <summary>
        /// Gets the users with most post.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <param name="authorCount">The author count.</param>
        /// <returns>List&lt;UserByPostDto&gt;.</returns>
        public async Task<List<UserByPostDto>> GetUsersWithMostPost(string subreddit, int authorCount = 10)
        {
            return await _context.Posts
                .Include(x => x.SubReddit)
                .Where(x => x.SubReddit.Name == subreddit)
                 .GroupBy(post => post.Author)
                 .Select(group => new UserByPostDto
                 {
                     Author = group.Key,
                     PostsCount = group.Count()
                 })
                 .OrderByDescending(user => user.PostsCount)
                 .Take(authorCount)
                 .ToListAsync();
        }

        /// <summary>
        /// Gets the posts with highest score.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <param name="postsCount">The posts count.</param>
        /// <returns>List&lt;PostsScoreDto&gt;.</returns>
        public async Task<List<PostsScoreDto>> GetPostsWithHighestScore(string subreddit, int postsCount = 10)
        {
            return await _context.Posts
                .Include(x => x.SubReddit)
                .Where(x => x.SubReddit.Name == subreddit)
                 .GroupBy(posts =>  new {  posts.Score, posts.Title, posts.Author })
                 .Select(group => new PostsScoreDto
                 {
                      Score = group.Key.Score,
                      Author = group.Key.Author,
                      PostsTitle = group.Key.Title
                 })
                 .OrderByDescending(user => user.Score)
                 .Take(postsCount)
                 .ToListAsync();
        }

        /// <summary>
        /// Adds the sub reddit.
        /// </summary>
        /// <param name="name">The name.</param>
        public async Task AddSubReddit(string name)
        {
            if (!_context.SubReddit.Any(x => x.Name == name))
            {
                await _context.SubReddit.AddAsync(new Data.Entities.SubRedditEntity { Name = name });
                var result = await _context.SaveChangesAsync();

                this._logger.LogInformation($"Add subreddit: {name} result: {result}");
            }
        }

        /// <summary>
        /// Removes the sub reddit.
        /// </summary>
        /// <param name="name">The name.</param>
        public async Task RemoveSubReddit(string name)
        {
            var removeSubReddit = await _context.SubReddit.FirstOrDefaultAsync(x => x.Name == name);

            if (removeSubReddit != null)
            {                
                _context.SubReddit.Remove(removeSubReddit);
                var result = await _context.SaveChangesAsync();
                this._logger.LogInformation($"Remove subreddit: {name} result: {result}");
            }
        }

        /// <summary>
        /// Lists the sub reddits.
        /// </summary>
        /// <returns>IEnumerable&lt;SubRedditEntity&gt;.</returns>
        public async Task<IEnumerable<SubRedditEntity>> ListSubReddits()
        {
           return await _context.SubReddit.ToArrayAsync();
        }

        /// <summary>
        /// Counts the posts by subreddit.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <returns>System.Int32.</returns>
        public async Task<int> CountPostsBySubreddit(string subreddit)
        {
            return await _context.Posts.CountAsync(x => x.SubReddit.Name == subreddit);
        }

        /// <summary>
        /// Gets the posts by subreddit.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <returns>System.Int32.</returns>
        public async Task<IEnumerable<PostDto>> GetPosts(string subreddit, int skip = 0, int take = 100)
        {
            return await _context.Posts.Where(x => x.SubReddit.Name == subreddit)
                .OrderByDescending(x => x.CreatedUTC)
                .Skip(skip)
                .Take(take)
                .Select(x => new PostDto { 
                                            Id = x.Id,
                                            Title = x.Title,
                                            Author = x.Author,
                                            Created = x.Created,
                                            CreatedUTC = x.CreatedUTC,
                                            NumberOfComments = x.NumberOfComments,
                                            Score = x.Score,
                                            Thumbnail = x.Thumbnail,
                                            Url = x.Url,
                }).ToListAsync();
        }
    }
}
