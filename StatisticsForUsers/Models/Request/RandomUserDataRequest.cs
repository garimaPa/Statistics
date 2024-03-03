namespace StatisticsForUsers.Models.Request;

public class RandomUserDataRequest
{
    public List<RandomUserData>? Results { get; set; }
    public UserInfo? Info { get; set; }
}