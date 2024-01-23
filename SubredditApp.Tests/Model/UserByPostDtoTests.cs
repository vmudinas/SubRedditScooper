using Xunit;
using SubredditApp.Model;

namespace SubredditApp.Tests
{
    public class UserByPostDtoTests
    {
        [Fact]
        public void UserByPostDto_PropertiesShouldWork()
        {
            // Arrange
            var userByPostDto = new UserByPostDto();

            // Act
            userByPostDto.Author = "Test Author";
            userByPostDto.PostsCount = 10;

            // Assert
            Assert.Equal("Test Author", userByPostDto.Author);
            Assert.Equal(10, userByPostDto.PostsCount);
        }
    }
}
