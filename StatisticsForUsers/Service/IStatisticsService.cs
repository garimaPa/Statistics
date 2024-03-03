using StatisticsForUsers.Models.Request;
using StatisticsForUsers.Models.Response;

namespace StatisticsForUsers.Service
{
    public interface IStatisticsService
    {
        StatisticsResultsResponse CalculateStatistics(List<RandomUserData> randomUsers);
    }
}