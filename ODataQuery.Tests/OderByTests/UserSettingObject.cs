namespace ODataQueryTests.OderByTests;

public class UserSettingObject
{
    public UserSettingObject(string sectionName, string createdAtMonth)
    {
        SectionName = sectionName;
        CreatedAtMonth = createdAtMonth;
    }

    public string Id { get; init; }

    public string SectionName { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedAtMonth { set; get; }

    public UserSettingObject()
    {
        
    }
    
}
