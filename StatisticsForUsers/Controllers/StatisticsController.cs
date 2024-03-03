using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using StatisticsForUsers.Models.Request;
using StatisticsForUsers.Models.Response;
using StatisticsForUsers.Service;

namespace StatisticsForUsers.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController : Controller
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpPost]
    public IActionResult CalculateStatistics([FromBody] RandomUserDataRequest randomUserRequest, [FromHeader(Name = "Accept")] string acceptHeader)
    {
        // Extract the list of users from the response
        List<RandomUserData> randomUsers = randomUserRequest?.Results;

        if (randomUsers == null || randomUsers.Count == 0)
        {
            return BadRequest("Invalid or empty user data");
        }

        // Filter users by nationality (nat=us)
        List<RandomUserData> usUsers = randomUsers
            .Where(user => user.Nat?.ToLower() == "us")
            .ToList();

        if (usUsers.Count == 0)
        {
            return BadRequest("No users from the United States found");
        }

        // Call the StatisticsService to calculate statistics
        StatisticsResultsResponse result = _statisticsService.CalculateStatistics(randomUsers);

        // Return the results in the requested format
        return GetResultByAcceptHeader(result, acceptHeader);
    }

    private IActionResult GetResultByAcceptHeader(StatisticsResultsResponse result, string acceptHeader)
    {
        if (acceptHeader?.ToLower() == "application/xml")
        {
            // Return XML format
            var xmlResult = SerializeToXml(result);
            return File(Encoding.UTF8.GetBytes(xmlResult), "application/xml", "statistics.xml");
        }
        else if (acceptHeader?.ToLower() == "text/plain")
        {
            // Return plain text format
            var plainTextResult = ConvertToPlainText(result);
            return File(Encoding.UTF8.GetBytes(plainTextResult), "text/plain", "statistics.txt");
        }
        else
        {
            // Default to JSON format
            return Ok(result);
        }
    }

    private string SerializeToXml(StatisticsResultsResponse result)
    {
        var serializer = new XmlSerializer(typeof(StatisticsResultsResponse));
        using (var stream = new StringWriter())
        {
            serializer.Serialize(stream, result);
            return stream.ToString();
        }
    }

    private string ConvertToPlainText(StatisticsResultsResponse result)
    {
        StringBuilder plainText = new StringBuilder();

        plainText.AppendLine($"Percentage of Male: {result.PercentageOfMale}%");
        plainText.AppendLine($"Percentage of Female: {result.PercentageOfFemale}%");
        plainText.AppendLine($"Percentage of first names that start with A-M versus N-Z: {result.PercentageFirstNameAM}%");
        plainText.AppendLine($"Percentage of last names that start with A-M versus N-Z: {result.PercentageLastNameAM}%");
        plainText.AppendLine("Percentage of people in each state, up to the top 10 most populous states:");
        foreach (var entry in result.MostPopulousStates)
        {
            plainText.AppendLine($"{entry.Key}: {entry.Value}%");
        }
        plainText.AppendLine("Percentage of females in each state, up to the top 10 most populous states:");
        foreach (var entry in result.MostPopulousStatesFemale)
        {
            plainText.AppendLine($"{entry.Key}: {entry.Value}%");
        }
        plainText.AppendLine("Percentage of males in each state, up to the top 10 most populous states:");
        foreach (var entry in result.MostPopulousStatesMale)
        {
            plainText.AppendLine($"{entry.Key}: {entry.Value}%");
        }
        plainText.AppendLine("Percentage of people in the following age ranges:");
        foreach (var entry in result.AgeRangeStatistics)
        {
            plainText.AppendLine($"{entry.Key}: {entry.Value}%");
        }

        return plainText.ToString();
    }
}