using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SubredditApp.Controllers;
using SubredditApp.Data.Entities;
using SubredditApp.Services;

public class SubRedditControllerTests
{
    private readonly Mock<ILogger<SubRedditController>> _mockLogger;
    private readonly Mock<IDataService> _mockDataService;
    private readonly SubRedditController _controller;

    public SubRedditControllerTests()
    {
        _mockLogger = new Mock<ILogger<SubRedditController>>();
        _mockDataService = new Mock<IDataService>();
        _controller = new SubRedditController(_mockLogger.Object, _mockDataService.Object);
    }

    [Fact]
    public async Task SaveSubReddit_ShouldCallAddSubReddit_AndReturnOk()
    {
        // Act
        var result = await _controller.SaveSubReddit("testSubreddit");

        // Assert
        _mockDataService.Verify(s => s.AddSubReddit("testSubreddit"), Times.Once());
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task ListSubreddits_ShouldReturnSubredditsList()
    {
        // Arrange
        var mockSubreddits = new List<SubRedditEntity> { /* Populate with test data */ };
        _mockDataService.Setup(s => s.ListSubReddits()).ReturnsAsync(mockSubreddits);

        // Act
        var result = await _controller.ListSubreddits();

        // Assert
        Assert.Equal(mockSubreddits, result);
    }

    [Fact]
    public async Task RemoveSubReddit_ShouldCallRemoveSubReddit()
    {
        // Act
        await _controller.RemoveSubReddit("testSubreddit");

        // Assert
        _mockDataService.Verify(s => s.RemoveSubReddit("testSubreddit"), Times.Once());
    }   
}
