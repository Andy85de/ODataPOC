using Marten;
using Microsoft.Extensions.DependencyInjection;
using ODataQueryTests.MartenTests.Models;

namespace ODataQueryTests.MartenTests;

public class MartenRequest
{
    public IDocumentSession? _martenStorage;
    
    public MartenRequest()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddMarten(
            options =>
            {
                options.Connection("User ID=root;Password=1;Host=localhost;Port=5432;Database=QueryTests;");
                options.DatabaseSchemaName = "test";

            }).ApplyAllDatabaseChangesOnStartup();

        var provider = serviceCollection.BuildServiceProvider();
        var documentStore = provider.GetService<IDocumentStore>();

        _martenStorage = documentStore?.LightweightSession();
    }

    [Theory]
    [MemberData(nameof(_testObjectToStore))]
    async Task CreateObjectInMarten(List<UserSettingObjectMarten> list)
    {
        _martenStorage?.InsertObjects(list);
       await _martenStorage?.SaveChangesAsync()!;
    }

    [Fact]
    public async Task QueryData()
    {
        var entities = _martenStorage.Query<UserSettingObjectMarten>("WHERE data ->> 'Id' = '095010f1-8ab4-4fa7-ae09-4c53743e0115' "
                                                                                                + "ORDER BY data->>'SortedPropertyStringId asc',"
                                                                                                + "data ->> 'SortedPropertyIntId asc',"
                                                                                                + "data ->> 'CreatedAt' ");
        

    }



    public static List<object[]> _testObjectToStore = new List<object[]>
    {
        new object[]
        {
            new List<UserSettingObjectMarten>
            {
                new UserSettingObjectMarten(
                    DateTime.Now.AddMonths(-1),
                    1,
                    "1",
                    DateTime.Now.AddDays(-1),
                    "First Month"),
                new UserSettingObjectMarten(
                    DateTime.Now.AddMonths(-2),
                    2,
                    "2",
                    DateTime.Now.AddDays(-2),
                    "Second Month"),
                new UserSettingObjectMarten(
                    DateTime.Now.AddMonths(-3),
                    3,
                    "3",
                    DateTime.Now.AddDays(-3),
                    "Third Month"),
                new UserSettingObjectMarten(
                    DateTime.Now.AddMonths(-4),
                    4,
                    "4",
                    DateTime.Now.AddDays(-4),
                    "Forthe Month")

            }
        }

    };

}
