using Server.DataAccess;
using Server.Models;

namespace Server.Test.RepositoriesTests;

public abstract class RepositoryTestBase<TItem, TId> : TestBase
    where TItem : IIdentifiable<TId>
    where TId : struct 
{
    public required IRepository<TItem, TId> Repository { get; init; }

    protected abstract void FillDbWithTestData();
}
