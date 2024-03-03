using Microsoft.AspNetCore.Mvc;
using Moq;
using StatisticsForUsers.Controllers;
using StatisticsForUsers.Models.Request;
using StatisticsForUsers.Models.Response;
using StatisticsForUsers.Service;

public class StatisticsControllerTests
{
    [Fact]
    public void CalculateStatistics_InvalidUserData_ReturnsBadRequest()
    {
        // Arrange
        var statisticsServiceMock = new Mock<IStatisticsService>();
        var controller = new StatisticsController(statisticsServiceMock.Object);

        // Act
        var result = controller.CalculateStatistics(null, "application/json");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void CalculateStatistics_NoUsersFromUS_ReturnsBadRequest()
    {
        // Arrange
        var statisticsServiceMock = new Mock<IStatisticsService>();
        var controller = new StatisticsController(statisticsServiceMock.Object);

        // Create a request with users that are not from the United States
        var randomUserRequest = new RandomUserDataRequest
        {
            Results = new List<RandomUserData>
            {
                new RandomUserData { Nat = "CA" }, // Not from the United States
                new RandomUserData { Nat = "GB" }  // Not from the United States
            }
        };

        // Act
        var result = controller.CalculateStatistics(randomUserRequest, "application/json");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("No users from the United States found", ((BadRequestObjectResult)result).Value);
    }

    [Fact]
    public void CalculateStatistics_WithValidUserData_ReturnsJsonResult()
    {
        // Arrange
        var statisticsServiceMock = new Mock<IStatisticsService>();
        statisticsServiceMock.Setup(service => service.CalculateStatistics(It.IsAny<List<RandomUserData>>()))
            .Returns(new StatisticsResultsResponse()); // Mock the service response

        var controller = new StatisticsController(statisticsServiceMock.Object);
        var randomUserRequest = new RandomUserDataRequest { Results = new List<RandomUserData> { new RandomUserData { Nat = "US" } } };

        // Act
        var result = controller.CalculateStatistics(randomUserRequest, "application/json");

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<StatisticsResultsResponse>(((OkObjectResult)result).Value);
    }

    [Fact]
    public void CalculateStatistics_WithXmlAcceptHeader_ReturnsXmlResult()
    {
        // Arrange
        var statisticsServiceMock = new Mock<IStatisticsService>();
        statisticsServiceMock.Setup(service => service.CalculateStatistics(It.IsAny<List<RandomUserData>>()))
            .Returns(new StatisticsResultsResponse()); // Mock the service response

        var controller = new StatisticsController(statisticsServiceMock.Object);
        var randomUserRequest = new RandomUserDataRequest { Results = new List<RandomUserData> { new RandomUserData { Nat = "US" } } };

        // Act
        var result = controller.CalculateStatistics(randomUserRequest, "application/xml");

        // Assert
        Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/xml", ((FileContentResult)result).ContentType);
        Assert.Equal("statistics.xml", ((FileContentResult)result).FileDownloadName);
    }

    [Fact]
    public void CalculateStatistics_WithPlainTextAcceptHeader_ReturnsPlainTextResult()
    {
        // Arrange
        var statisticsServiceMock = new Mock<IStatisticsService>();
        statisticsServiceMock.Setup(service => service.CalculateStatistics(It.IsAny<List<RandomUserData>>()))
            .Returns(SampleStatisticsResults.GetSampleStatisticsResults()); // Mock the service response

        var controller = new StatisticsController(statisticsServiceMock.Object);
        var randomUserRequest = new RandomUserDataRequest { Results = new List<RandomUserData> { new RandomUserData { Nat = "US" } } };

        // Act
        var result = controller.CalculateStatistics(randomUserRequest, "text/plain");

        // Assert
        Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/plain", ((FileContentResult)result).ContentType);
        Assert.Equal("statistics.txt", ((FileContentResult)result).FileDownloadName);
    }
}
