using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SubredditApp.Data.Entities;
using SubredditApp.Model;
using SubredditApp.Service;
using SubredditApp.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class SubRedditPoolingServiceTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<IServiceScope> _mockServiceScope;
    private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory;
    private readonly Mock<ILogger<SubRedditPoolingService>> _mockLogger;
    private readonly Mock<IRedditService> _mockRedditService;
    private readonly Mock<IDataService> _mockDataService;

    public SubRedditPoolingServiceTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockServiceScope = new Mock<IServiceScope>();
        _mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
        _mockLogger = new Mock<ILogger<SubRedditPoolingService>>();
        _mockRedditService = new Mock<IRedditService>();
        _mockDataService = new Mock<IDataService>();

        _mockServiceScope.Setup(x => x.ServiceProvider).Returns(_mockServiceProvider.Object);
        _mockServiceScopeFactory.Setup(x => x.CreateScope()).Returns(_mockServiceScope.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(_mockServiceScopeFactory.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(ILogger<SubRedditPoolingService>))).Returns(_mockLogger.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IRedditService))).Returns(_mockRedditService.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IDataService))).Returns(_mockDataService.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoSubredditsAvailable_ShouldNotAttemptDataRetrieval()
    {
        // Arrange
        _mockDataService.Setup(x => x.ListSubReddits()).ReturnsAsync(new List<SubRedditEntity>());

        var cancellationTokenSource = new CancellationTokenSource();
        var subRedditPoolingService = new SubRedditPoolingService(_mockLogger.Object, _mockServiceProvider.Object);

        // Act
        var executeTask = subRedditPoolingService.StartAsync(cancellationTokenSource.Token);

        // Allow some time for the service to run
        await Task.Delay(500);

        // Signal cancellation to stop the service
        cancellationTokenSource.Cancel();

        // Ensure the service stops gracefully
        await executeTask;

        // Assert
        _mockRedditService.Verify(x => x.GetSubredditAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never());
    }
}
