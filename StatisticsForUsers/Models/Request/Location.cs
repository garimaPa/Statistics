namespace StatisticsForUsers.Models.Request;

public class Location
{
    public Street? Street { get; set; }
    public string? City { get; set; }
    public string State { get; set; }
    public string? Country { get; set; }
    public int PostCode { get; set; }
    public Coordinates? Coordinates { get; set; }
    public TimeZone? TimeZone { get; set; }
}