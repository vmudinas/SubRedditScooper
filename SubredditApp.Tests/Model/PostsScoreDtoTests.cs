using Xunit;
using SubredditApp.Model;

namespace SubredditApp.Tests
{
    public class PostsScoreDtoTests
    {
        [Fact]
        public void PostsScoreDto_PropertiesShouldWork()
        {
            // Arrange
            var postsScoreDto = new PostsScoreDto();

            // Act
            postsScoreDto.Author = "Test Author";
            postsScoreDto.PostsTitle = "Test Post Title";
            postsScoreDto.Score = 42;

            // Assert
            Assert.Equal("Test Author", postsScoreDto.Author);
            Assert.Equal("Test Post Title", postsScoreDto.PostsTitle);
            Assert.Equal(42, postsScoreDto.Score);
        }
    }
}
