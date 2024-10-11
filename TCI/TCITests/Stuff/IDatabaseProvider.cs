using PetaPoco;

namespace TCITests.Stuff;
internal interface IDatabaseProvider
{
    IDatabase GetDatabase();
}