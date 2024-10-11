using PetaPoco;

namespace TCITests.Stuff;
internal class DatabaseProvider : IDatabaseProvider
{
    private readonly string connectionString;
    private readonly string providerName;

    public DatabaseProvider(string connectionString, string providerName = "Microsoft.Data.SqlClient")
    {
        this.connectionString = connectionString;
        this.providerName = providerName;
    }

    public IDatabase GetDatabase()
    {
        return new Database(connectionString, providerName);
    }
}