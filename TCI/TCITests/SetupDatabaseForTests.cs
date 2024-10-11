using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;
using NUnit.Framework;
using Testcontainers.MsSql;

namespace TCITests;
[SetUpFixture]
public class SetupDatabaseForTests : IDisposable
{
    private const string testDatabaseName = "TestContainerIssueDatabase";
    private string dacpacPath;
    private static string? connectionString;
    private IAsyncDisposable? _disposable;

    public SetupDatabaseForTests()
    {
        var path = Directory.GetCurrentDirectory();
        dacpacPath = Path.Combine(path, "Dacpacs\\DatabaseSSDT.dacpac");
        connectionString = InitializeDocker().GetAwaiter().GetResult();
        Deploy(connectionString, upgradeExisting: true);
    }

    public static SqlConnection TestDatabaseConnection => new(connectionString);
    public static string ConnectionString => connectionString;

    private async Task<string> InitializeDocker()
    {
        var testDatabase = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2022-latest").Build();
        _disposable = testDatabase;
        await testDatabase.StartAsync().ConfigureAwait(false);
        var hostPort = testDatabase.GetMappedPublicPort(1433);
        var connectionString = testDatabase
        .GetConnectionString()
        .Replace("Database=master", $"Database={testDatabaseName}"); //Connect to new test database rather than master
        Deploy(connectionString, upgradeExisting: true);
        return connectionString;
    }

    private void Deploy(string connectionString, bool upgradeExisting)
    {
        var dacpac = DacPackage.Load(dacpacPath);
        var dacOptions = new DacDeployOptions { CreateNewDatabase = true, AllowDropBlockingAssemblies = true };
        var dacServiceInstance = new DacServices(connectionString);
        dacServiceInstance.Deploy(dacpac, testDatabaseName, upgradeExisting, dacOptions);
    }

    public void Dispose()
    {
        _disposable?.DisposeAsync().GetAwaiter().GetResult();
    }
}