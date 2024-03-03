namespace StatisticsForUsers.Models.Request;

public class UserInfo
{
    public string? Seed { get; set; }
    public int Results { get; set; }
    public int Page { get; set; }
    public string? Version { get; set; }
}