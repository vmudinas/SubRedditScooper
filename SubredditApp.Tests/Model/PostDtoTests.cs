using System;
using Xunit;
using SubredditApp.Model;

namespace SubredditApp.Tests
{
    public class PostDtoTests
    {
        [Fact]
        public void PostDto_PropertiesShouldWork()
        {
            // Arrange
            var postDto = new PostDto();

            // Act
            postDto.Id = "123";
            postDto.Title = "Test Post";
            postDto.Author = "Test Author";
            postDto.Score = 42;
            postDto.Thumbnail = "thumbnail.jpg";
            postDto.Url = "http://example.com";
            postDto.NumberOfComments = 10;
            postDto.CreatedUTC = DateTime.UtcNow;
            postDto.Created = DateTime.Now;

            // Assert
            Assert.Equal("123", postDto.Id);
            Assert.Equal("Test Post", postDto.Title);
            Assert.Equal("Test Author", postDto.Author);
            Assert.Equal(42, postDto.Score);
            Assert.Equal("thumbnail.jpg", postDto.Thumbnail);
            Assert.Equal("http://example.com", postDto.Url);
            Assert.Equal(10, postDto.NumberOfComments);
            Assert.IsType<DateTime>(postDto.CreatedUTC);
            Assert.IsType<DateTime>(postDto.Created);
        }
    }
}
