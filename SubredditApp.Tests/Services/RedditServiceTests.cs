using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SubredditApp.Service;
using System;

public class RedditServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly RedditService _redditService;

    public RedditServiceTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        var client = new HttpClient(_mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        _redditService = new RedditService(_mockHttpClientFactory.Object);
    }

    [Fact]
    public async Task GetSubredditAsync_WhenSuccessfulResponse_ShouldReturnSubreddit()
    {
        // Arrange
        SetupMockHttpMessageHandler(HttpStatusCode.OK, "your json response here");

        // Act
        var result = await _redditService.GetSubredditAsync("testSubreddit");

        // Assert
        Assert.NotNull(result);
        // More assertions based on the expected outcome
    }

    private void SetupMockHttpMessageHandler(HttpStatusCode statusCode, string content)
    {
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content)
            });
    }

    // Non-Successful HTTP Response (e.g., 404 Not Found)
    [Fact]
    public async Task GetSubredditAsync_WhenResponseIsNotFound_ShouldThrowException()
    {
        // Arrange
        SetupMockHttpMessageHandler(HttpStatusCode.NotFound, "");

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _redditService.GetSubredditAsync("nonexistentSubreddit"));
    }

    // HTTP Response with TooManyRequests Status Code
    [Fact]
    public async Task GetSubredditAsync_WhenRateLimitExceeded_ShouldRetryAfterDelay()
    {
        // Arrange
        SetupMockHttpMessageHandler(HttpStatusCode.TooManyRequests, "", rateLimitReset: 1); // RateLimitReset set to 1 second for testing

        // Act
        var result = await _redditService.GetSubredditAsync("popularSubreddit");

        // Assert
        Assert.NotNull(result); // Verify that the service eventually returns a result after retry
    }   

    [Fact]
    public async Task GetSubredditAsync_WhenAfterIsNull_ShouldReturnFirstPageOfResults()
    {
        // Arrange
        string jsonResponse = "{/* mock JSON response for the first page */}";
        SetupMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _redditService.GetSubredditAsync("subreddit", after: null);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Posts); // Assuming the first page has posts
    }  

    private void SetupMockHttpMessageHandler(HttpStatusCode statusCode, string content, bool includeRateLimitHeaders = true)
    {
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = new StringContent(content)
        };

        if (includeRateLimitHeaders)
        {
            httpResponseMessage.Headers.Add("X-RateLimit-Used", "10");
            httpResponseMessage.Headers.Add("X-RateLimit-Remaining", "5");
            httpResponseMessage.Headers.Add("X-RateLimit-Reset", "60");
        }

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponseMessage);
    }


    [Fact]
    public async Task GetSubredditAsync_WhenRateLimitHeadersAreMissing_ShouldNotThrowException()
    {
        // Arrange
        string jsonResponse = "{/* valid JSON response */}";
        SetupMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse, includeRateLimitHeaders: false);

        // Act
        var result = await _redditService.GetSubredditAsync("subreddit");

        // Assert
        Assert.NotNull(result);
    }

    // Helper Method for Setting up Mock HttpMessageHandler with Rate Limit Headers
    private void SetupMockHttpMessageHandler(HttpStatusCode statusCode, string content, int rateLimitReset = 60)
    {
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = new StringContent(content)
        };

        httpResponseMessage.Headers.Add("X-RateLimit-Used", "10");
        httpResponseMessage.Headers.Add("X-RateLimit-Remaining", "5");
        httpResponseMessage.Headers.Add("X-RateLimit-Reset", rateLimitReset.ToString());

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponseMessage);
    }

}
