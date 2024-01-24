using SubredditApp.Model;
using Newtonsoft.Json.Linq;
using RedditNet;
using System.Net.Http;
using System.Diagnostics.Metrics;

namespace SubredditApp.Service
{
    /// <summary>
    /// Class RedditService.
    /// Implements the <see cref="SubredditApp.Service.IRedditService" />
    /// </summary>
    /// <seealso cref="SubredditApp.Service.IRedditService" />
    /// <font color="red">Badly formed XML comment.</font>
    // Reddit client class
    /// </summary>
    public class RedditService : IRedditService
    {
        /// <summary>
        /// The client
        /// </summary>
        private readonly HttpClient _client;
        /// <summary>
        /// The subreddit
        /// </summary>
        private Subreddit subreddit;
        /// <summary>
        /// The rate limit used
        /// </summary>
        private int _rateLimitUsed;
        /// <summary>
        /// The rate limit remaining
        /// </summary>
        private int _rateLimitRemaining;
        /// <summary>
        /// The rate limit reset
        /// </summary>
        private int _rateLimitReset;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedditService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        public RedditService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Get the subreddit and related posts for the given subreddit name
        /// </summary>
        /// <param name="name">The name of the subreddit to retrieve</param>
        /// <param name="before">The ID of the post to start at</param>
        /// <param name="after">The after.</param>
        /// <param name="limit">The number of posts to retrieve per request</param>
        /// <returns>A Task&lt;SubredditApp.Model.Subreddit&gt; representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Subreddit name cannot be null or empty., nameof(name)</exception>
        /// <exception cref="Exception">$"An unexpected error occurred: {ex.Message}</exception>
        /// <exception cref="Exception">$"An unexpected error occurred:</exception>
        public async Task<Subreddit> GetSubredditAsync(string name, string? before = null, string? after = null, int limit = 1000)
        {
            try
            {
                var redditService = new RedditApi();

                // Check if subredditName is null or empty
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Subreddit name cannot be null or empty.", nameof(name));
                }

                HttpResponseMessage? response = null;

                try
                {
                    subreddit = new Subreddit
                    {
                        Name = name,
                        Posts = []
                    };

                    string subRedditUrl = $"https://www.reddit.com/r/{name}/.json?limit={limit}";
                    if (!string.IsNullOrEmpty(after))
                        subRedditUrl += $"&after={after}";

                    response = await redditService.Client.GetAsync(subRedditUrl);

                    try
                    {
                        response.EnsureSuccessStatusCode();
                        // Read rate limit headers
                        _rateLimitUsed = int.Parse(response.Headers.GetValues("X-RateLimit-Used").FirstOrDefault());
                        _rateLimitRemaining = int.Parse(response.Headers.GetValues("X-RateLimit-Remaining").FirstOrDefault());
                        _rateLimitReset = int.Parse(response.Headers.GetValues("X-RateLimit-Reset").FirstOrDefault());
                    }
                    catch (Exception)
                    {   // currently these headers are not returned, I believe Reddit is working on it
                        _rateLimitRemaining = 0;
                        _rateLimitReset = 0;
                        _rateLimitUsed = 0;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        // read the response content and populate the subreddit object
                        var content = await response.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(content);

                        var jArray = jObject["data"]["children"] as JArray;

                        foreach (var obj in jArray)
                        {
                            PostDto post = new()
                            {
                                Id = obj["data"]["id"].ToString(),
                                Title = obj["data"]["title"].ToString(),
                                Author = obj["data"]["author"].ToString(),
                                Score = obj["data"]["score"].Value<int>(),
                                Thumbnail = obj["data"]["thumbnail"].ToString(),
                                Url = obj["data"]["url"].ToString(),
                                NumberOfComments = obj["data"]["num_comments"].Value<int>(),
                                CreatedUTC = DateTimeOffset.FromUnixTimeSeconds(obj["data"]["created"].Value<long>()).DateTime,
                                Created = DateTimeOffset.FromUnixTimeSeconds(obj["data"]["created_utc"].Value<long>()).DateTime
                            };

                            subreddit.Posts.Add(post);
                        }

                        if (subreddit.Posts.Count > 0) // if there are no posts, update the before and after
                        {
                            subreddit.Before = $"t3_{subreddit.Posts.FirstOrDefault().Id}" ?? string.Empty;
                            subreddit.After = jObject["data"]["after"]?.ToString();
                        }
                        else if (string.IsNullOrWhiteSpace(subreddit.Before) && !string.IsNullOrWhiteSpace(before))
                        {
                            subreddit.Before = before;
                        }
                        else if (string.IsNullOrWhiteSpace(subreddit.After) && !string.IsNullOrWhiteSpace(after))
                        {
                            subreddit.After = after;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        // Wait until the reset period passes before retrying
                        await Task.Delay(_rateLimitReset * 1000);
                        return await GetSubredditAsync(name);
                    }

                    // Wait a little 
                    await DelayPool();

                    return subreddit;
                }
                catch (Exception ex)
                {
                    throw new Exception($"An unexpected error occurred: {ex.Message}");
                }
                finally
                {
                    response?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting random meme from /r/dankmemes! ({ex.Message})");
            }
            throw new Exception($"An unexpected error occurred:");

        }

        /// <summary>
        /// Delays the pool.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task DelayPool()
        {
            if (_rateLimitRemaining > 0)
            {
                // Spread out the remaining requests evenly over the reset period
                await Task.Delay(_rateLimitReset / _rateLimitRemaining * 1000);
            }
            else
            {
                // wait for the reset period
                await Task.Delay(_rateLimitReset * 1000);
            }
        }
    }
}