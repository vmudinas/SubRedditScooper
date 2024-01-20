using Microsoft.Extensions.Logging;
using Moq;
using SubredditApp.Controllers;
using SubredditApp.Model;
using SubredditApp.Services;

public class StatisticsControllerTests
{
    private readonly Mock<ILogger<StatisticsController>> _mockLogger;
    private readonly Mock<IDataService> _mockDataService;
    private readonly StatisticsController _controller;

    public StatisticsControllerTests()
    {
        _mockLogger = new Mock<ILogger<StatisticsController>>();
        _mockDataService = new Mock<IDataService>();
        _controller = new StatisticsController(_mockLogger.Object, _mockDataService.Object);
    }

    [Fact]
    public async Task TopPostsUsers_ShouldReturnUsersWithMostPosts()
    {
        // Arrange
        var mockUsers = new List<UserByPostDto> { /* populate with test data */ };
        _mockDataService.Setup(s => s.GetUsersWithMostPost(It.IsAny<string>(), It.IsAny<int>()))
                        .ReturnsAsync(mockUsers);

        // Act
        var result = await _controller.TopPostsUsers("testSubreddit", 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mockUsers, result);
    }

    [Fact]
    public async Task TopPosts_ShouldReturnPostsWithHighestScores()
    {
        // Arrange
        var mockPosts = new List<PostsScoreDto> { /* populate with test data */ };
        _mockDataService.Setup(s => s.GetPostsWithHighestScore(It.IsAny<string>(), It.IsAny<int>()))
                        .ReturnsAsync(mockPosts);

        // Act
        var result = await _controller.TopPosts("testSubreddit", 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mockPosts, result);
    }

    [Fact]
    public async Task CountPosts_ShouldReturnCorrectPostsCount()
    {
        // Arrange
        _mockDataService.Setup(s => s.CountPostsBySubreddit(It.IsAny<string>()))
                        .ReturnsAsync(5); // Example count

        // Act
        var result = await _controller.CountPosts("testSubreddit");

        // Assert
        Assert.Equal(5, result);
    }



}
