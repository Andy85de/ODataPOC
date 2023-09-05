namespace ODataQueryTests.ValidatorTests.Models;

public class UserProfileSettingObject
{
    public DateTime CreatedAt { get; set; }

    public string Id { get; set; }

    public int CounterShort { get; set; }
    
    public long CounterLong { set; get; }
    
    public DateTimeOffset UpdatedAt { set; get; }
}
