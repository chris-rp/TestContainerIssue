using NUnit.Framework;
using PetaPoco;
using System.Reflection;
using TCITests.Stuff;

namespace TCITests;
public class Tests
{
    private readonly string someTable = typeof(SomeTablePoco).GetCustomAttribute<TableNameAttribute>()!.Value;

    private DatabaseProvider databaseProvider;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        //Create repository with test-database-connection and default MS Sql provider
        databaseProvider = new DatabaseProvider(SetupDatabaseForTests.ConnectionString);
    }

    [TearDown]
    public async Task TearDown()
    {
        //Reset table after each test
        using var database = databaseProvider.GetDatabase();
        await database.ExecuteAsync($"DELETE FROM {someTable} WHERE Id>@0;", 1);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        //Reset table after all tests
        using var database = databaseProvider.GetDatabase();
        await database.ExecuteAsync($"TRUNCATE TABLE {someTable};");
    }

    [Test]
    public void GetShouldReturnUser()
    {
        using var database = databaseProvider.GetDatabase();
        database.Insert(new SomeTablePoco { Id = 1, Name = "Test" });

        var result = database.Fetch<SomeTablePoco>();

        Assert.That(result.Count, Is.EqualTo(1));
    }
}
