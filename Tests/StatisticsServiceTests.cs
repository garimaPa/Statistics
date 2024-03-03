using StatisticsForUsers.Models.Request;
using StatisticsForUsers.Service;

public class StatisticsServiceTests
{
    [Fact]
    public void CalculateStatistics_WithValidData_ReturnsStatisticsResultsResponse()
    {
        // Arrange
        var statisticsService = new StatisticsService();
        var randomUsers = GenerateRandomUsers(100);

        // Act
        var result = statisticsService.CalculateStatistics(randomUsers);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.PercentageOfMale + result.PercentageOfFemale);
        Assert.Equal(100, result.PercentageFirstNameAM + result.PercentageFirstNameNZ);
        Assert.Equal(100, result.PercentageLastNameAM + result.PercentageLastNameNZ);
        Assert.NotEmpty(result.MostPopulousStates);
        Assert.NotEmpty(result.MostPopulousStatesMale);
        Assert.NotEmpty(result.MostPopulousStatesFemale);
        Assert.NotEmpty(result.AgeRangeStatistics);
    }

    [Fact]
    public void CalculateStatistics_WithInvalidData_ThrowsArgumentException()
    {
        // Arrange
        var statisticsService = new StatisticsService();
        List<RandomUserData> randomUsers = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => statisticsService.CalculateStatistics(randomUsers));
    }

    private List<RandomUserData> GenerateRandomUsers(int count)
    {
        var random = new Random();
        var randomUsers = new List<RandomUserData>();

        for (int i = 0; i < count; i++)
        {
            var user = new RandomUserData
            {
                Gender = random.Next(2) == 0 ? "male" : "female",
                Name = new Name
                {
                    First = "John",
                    Last = "Doe"
                },
                Location = new Location
                {
                    State = "California"
                },
                Dob = new Dob
                {
                    Age = random.Next(100)
                }
            };

            randomUsers.Add(user);
        }

        return randomUsers;
    }
}