using System.Xml.Serialization;

namespace StatisticsForUsers.Models.Response;

public class StatisticsResultsResponse
{
    public double PercentageOfMale { get; set; }
    public double PercentageOfFemale { get; set; }
    public double PercentageFirstNameAM { get; set; }
    public double PercentageFirstNameNZ { get; set; }
    public double PercentageLastNameAM { get; set; }
    public double PercentageLastNameNZ { get; set; }
    [XmlIgnore]
    public Dictionary<string, double> MostPopulousStates { get; set; }
    [XmlIgnore]
    public Dictionary<string, double> MostPopulousStatesMale { get; set; }
    [XmlIgnore]
    public Dictionary<string, double> MostPopulousStatesFemale { get; set; }
    [XmlIgnore]
    public Dictionary<string, double> AgeRangeStatistics { get; set; }
}