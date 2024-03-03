using StatisticsForUsers.Models.Response;
using StatisticsForUsers.Models.Request;

namespace StatisticsForUsers.Service
{
    public class StatisticsService : IStatisticsService
    {
        public StatisticsResultsResponse CalculateStatistics(List<RandomUserData> randomUsers)
        {
            if (randomUsers == null || !randomUsers.Any())
                throw new ArgumentException("Invalid input data.");

            // Total number of users
            int totalUsers = randomUsers.Count;

            // 1. Percentage of gender in each category
            var genderStatistics = randomUsers
                .GroupBy(user => user.Gender.ToLower())
                .ToDictionary(group => group.Key, group => (double)group.Count() / totalUsers * 100);

            // 2. Percentage of first names that start with A-M versus N-Z
            var firstNameStatistics = randomUsers
                .GroupBy(user => user.Name.First.ToLower()[0] <= 'm' ? "A-M" : "N-Z")
                .ToDictionary(group => group.Key, group => (double)group.Count() / totalUsers * 100);

            // 3. Percentage of last names that start with A-M versus N-Z
            var lastNameStatistics = randomUsers
                .GroupBy(user => user.Name.Last.ToLower()[0] <= 'm' ? "A-M" : "N-Z")
                .ToDictionary(group => group.Key, group => (double)group.Count() / totalUsers * 100);

            // 4. Percentage of people in each state, up to the top 10 most populous states
            var stateStatistics = randomUsers
                .GroupBy(user => user.Location.State)
                .OrderByDescending(group => group.Count())
                .Take(10)
                .ToDictionary(group => group.Key, group => (double)group.Count() / totalUsers * 100);

            // 5. Percentage of females in each state, up to the top 10 most populous states
            var femaleStateStatistics = randomUsers
                .Where(user => user.Gender.ToLower() == "female")
                .GroupBy(user => user.Location.State)
                .OrderByDescending(group => group.Count())
                .Take(10)
                .ToDictionary(group => group.Key, group => (double)group.Count() / totalUsers * 100);

            // 6. Percentage of males in each state, up to the top 10 most populous states
            var maleStateStatistics = randomUsers
                .Where(user => user.Gender.ToLower() == "male")
                .GroupBy(user => user.Location.State)
                .OrderByDescending(group => group.Count())
                .Take(10)
                .ToDictionary(group => group.Key, group => (double)group.Count() / totalUsers * 100);

            // 7. Percentage of people in the following age ranges: 0-20, 21-40, 41-60, 61-80, 81-100, 100+
            var ageRangeStatistics = randomUsers
                .GroupBy(user => CalculateAgeRange(user.Dob.Age))
                .ToDictionary(group => group.Key, group => (double)group.Count() / totalUsers * 100);

            return new StatisticsResultsResponse
            {
                PercentageOfMale = genderStatistics.GetValueOrDefault("male", 0),
                PercentageOfFemale = genderStatistics.GetValueOrDefault("female", 0),
                PercentageFirstNameAM = firstNameStatistics.GetValueOrDefault("A-M", 0),
                PercentageFirstNameNZ = firstNameStatistics.GetValueOrDefault("N-Z", 0),
                PercentageLastNameAM = lastNameStatistics.GetValueOrDefault("A-M", 0),
                PercentageLastNameNZ = lastNameStatistics.GetValueOrDefault("N-Z", 0),
                MostPopulousStates = stateStatistics,
                MostPopulousStatesMale = maleStateStatistics,
                MostPopulousStatesFemale = femaleStateStatistics,
                AgeRangeStatistics = ageRangeStatistics
            };
        }

        private string CalculateAgeRange(int age)
        {
            if (age <= 20) return "0-20";
            if (age <= 40) return "21-40";
            if (age <= 60) return "41-60";
            if (age <= 80) return "61-80";
            if (age <= 100) return "81-100";
            return "100+";
        }
    }
}