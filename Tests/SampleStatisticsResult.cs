using StatisticsForUsers.Models.Response;

public static class SampleStatisticsResults
{
    public static StatisticsResultsResponse GetSampleStatisticsResults()
    {
        return new StatisticsResultsResponse
        {
            PercentageOfMale = 45.5,
            PercentageOfFemale = 54.5,
            PercentageFirstNameAM = 60.0,
            PercentageLastNameAM = 40.0,
            MostPopulousStates = new Dictionary<string, double>
            {
                { "California", 15.2 },
                { "Texas", 12.5 },
                { "Florida", 9.8 },
            },
            MostPopulousStatesFemale = new Dictionary<string, double>
            {
                { "California", 16.5 },
                { "Texas", 11.8 },
                { "Florida", 10.2 },
            },
            MostPopulousStatesMale = new Dictionary<string, double>
            {
                { "California", 14.8 },
                { "Texas", 13.2 },
                { "Florida", 8.5 },
            },
            AgeRangeStatistics = new Dictionary<string, double>
            {
                { "0-10", 14.8 },
                { "11-20", 13.2 },
                { "21-30", 8.5 },
            }
        };
    }
}