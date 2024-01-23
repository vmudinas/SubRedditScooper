using System;
using Xunit;
using SubredditApp.Data.Entities;

namespace SubredditApp.Tests
{
    public class PostEntityTests
    {
        [Fact]
        public void PostEntity_PropertiesShouldWork()
        {
            // Arrange
            var post = new PostEntity();

            // Act
            post.Id = "123";
            post.Title = "Test Post";
            post.Author = "Test Author";
            post.Score = 42;
            post.Thumbnail = "thumbnail.jpg";
            post.Url = "http://example.com";
            post.NumberOfComments = 10;
            post.CreatedUTC = DateTime.UtcNow;
            post.Created = DateTime.Now;
            post.SubRedditId = 1;

            // Assert
            Assert.Equal("123", post.Id);
            Assert.Equal("Test Post", post.Title);
            Assert.Equal("Test Author", post.Author);
            Assert.Equal(42, post.Score);
            Assert.Equal("thumbnail.jpg", post.Thumbnail);
            Assert.Equal("http://example.com", post.Url);
            Assert.Equal(10, post.NumberOfComments);
            Assert.IsType<DateTime>(post.CreatedUTC);
            Assert.IsType<DateTime>(post.Created);
            Assert.Equal(1, post.SubRedditId);
        }
    }
}
