using Marten;
using ODataWithSprache.Models;

namespace ODataWithSprache.Marten;

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