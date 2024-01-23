using System.Collections.Generic;
using Xunit;
using SubredditApp.Model;

namespace SubredditApp.Tests
{
    public class SubredditTests
    {
        [Fact]
        public void Subreddit_PropertiesAndPostsCollectionShouldWork()
        {
            // Arrange
            var subreddit = new Subreddit();

            // Act
            subreddit.Before = "BeforeToken";
            subreddit.After = "AfterToken";
            subreddit.Name = "TestSubreddit";

            // Create a list of PostDto objects
            var posts = new List<PostDto>
            {
                new PostDto { Id = "1", Title = "Post 1" },
                new PostDto { Id = "2", Title = "Post 2" }
            };
            subreddit.Posts = posts;

            // Assert
            Assert.Equal("BeforeToken", subreddit.Before);
            Assert.Equal("AfterToken", subreddit.After);
            Assert.Equal("TestSubreddit", subreddit.Name);
            Assert.Equal(2, subreddit.Posts.Count);
            Assert.Contains(subreddit.Posts, post => post.Title == "Post 1");
            Assert.Contains(subreddit.Posts, post => post.Title == "Post 2");
        }
    }
}
