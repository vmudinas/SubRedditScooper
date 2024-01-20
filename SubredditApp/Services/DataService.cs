using Microsoft.EntityFrameworkCore;
using SubredditApp.Data;
using SubredditApp.Data.Entities;
using SubredditApp.Model;

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
        /// <summary>
        /// Initializes a new instance of the <see cref="DataService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public DataService(ApplicationDbContext dbContext)
        {
            this._context = dbContext;
        }

        /// <summary>
        /// Adds the post.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        public async Task AddPost(Subreddit subreddit)
        {
            var dbSubreddit = await this._context.SubReddit.FirstOrDefaultAsync(x => x.Name == subreddit.Name);

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
                        await this._context.Posts.AddAsync(new Data.Entities.PostEntity
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
                    }                    
                }

                await this._context.SaveChangesAsync();
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
            if (!this._context.SubReddit.Any(x => x.Name == name))
            {
                await this._context.SubReddit.AddAsync(new Data.Entities.SubRedditEntity { Name = name });
                await this._context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes the sub reddit.
        /// </summary>
        /// <param name="name">The name.</param>
        public async Task RemoveSubReddit(string name)
        {
            var removeSubReddit = await this._context.SubReddit.FirstOrDefaultAsync(x => x.Name == name);

            if (removeSubReddit != null)
            {                
                this._context.SubReddit.Remove(removeSubReddit);
                await this._context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Lists the sub reddits.
        /// </summary>
        /// <returns>IEnumerable&lt;SubRedditEntity&gt;.</returns>
        public async Task<IEnumerable<SubRedditEntity>> ListSubReddits()
        {
           return await this._context.SubReddit.ToArrayAsync();
        }

        /// <summary>
        /// Counts the posts by subreddit.
        /// </summary>
        /// <param name="subreddit">The subreddit.</param>
        /// <returns>System.Int32.</returns>
        public async Task<int> CountPostsBySubreddit(string subreddit)
        {
            return await this._context.Posts.CountAsync(x => x.SubReddit.Name == subreddit);
        }
    }
}
