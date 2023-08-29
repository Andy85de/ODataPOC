using Marten;
using ODataQueryTests.MartenTests.Models;
using ODataQueryTests.OderByTests;

namespace ODataQueryTests.MartenTests;

public class RegisterObject : MartenRegistry
{
    public RegisterObject()
    {
        For<UserSettingObjectMarten>()
            .DocumentAlias("testQueries")
            .Identity(p => p.Id)
            .Duplicate(p => p.CreatedAt)
            .Duplicate(p => p.UpdateAt)
            .DatabaseSchemaName("test")
            .Metadata(p => p.DisableInformationalFields());
    }
}
