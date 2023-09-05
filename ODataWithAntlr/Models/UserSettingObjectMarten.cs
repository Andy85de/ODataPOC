namespace ODataWithSprache.Models;

public class UserSettingObjectMarten
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public DateTime CreatedAt { get; set; }

    public int SortedPropertyIntId { set; get; }

    public string SortedPropertyStringId { set; get; }

    public DateTime UpdateAt { get; set; }

    public string Section { get; set; }

    public int Amount { set; get; }

    public UserSettingObjectMarten(
        DateTime createdAt,
        int sortedPropertyIntId,
        string sortedPropertyStringId,
        DateTime updateAt,
        string section,
        int amount)
    {
        CreatedAt = createdAt;
        SortedPropertyIntId = sortedPropertyIntId;
        SortedPropertyStringId = sortedPropertyStringId;
        UpdateAt = updateAt;
        Section = section;
        Amount = amount;
    }
}
