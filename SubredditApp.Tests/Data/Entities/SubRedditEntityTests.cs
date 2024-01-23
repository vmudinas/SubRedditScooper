using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using SubredditApp.Data.Entities;

namespace SubredditApp.Tests
{
    public class SubRedditEntityTests
    {
        [Fact]
        public void SubRedditEntity_PropertiesAndNavigationPropertyShouldWork()
        {
            // Arrange
            var subreddit = new SubRedditEntity();

            // Act
            subreddit.Id = 1;
            subreddit.Name = "TestSubreddit";
            subreddit.Before = "BeforeToken";
            subreddit.After = "AfterToken";

            // Create a list of related posts
            var posts = new List<PostEntity>
            {
                new PostEntity { Id = "1", Title = "Post 1" },
                new PostEntity { Id = "2", Title = "Post 2" }
            };
            subreddit.Posts = posts;

            // Assert
            Assert.Equal(1, subreddit.Id);
            Assert.Equal("TestSubreddit", subreddit.Name);
            Assert.Equal("BeforeToken", subreddit.Before);
            Assert.Equal("AfterToken", subreddit.After);
            Assert.Equal(2, subreddit.Posts.Count);
            Assert.Contains(subreddit.Posts, post => post.Title == "Post 1");
            Assert.Contains(subreddit.Posts, post => post.Title == "Post 2");
        }
    }
}
