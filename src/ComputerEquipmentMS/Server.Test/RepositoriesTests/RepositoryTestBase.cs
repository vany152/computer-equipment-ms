using Server.DataAccess;
using Server.Models;

namespace Server.Test.RepositoriesTests;

public abstract class RepositoryTestBase<TItem, TId> : TestBase
    where TItem : IIdentifiable<TId>
    where TId : struct 
{
    protected IRepository<TItem, TId> Repository;

    protected RepositoryTestBase(IRepository<TItem, TId> repository)
    {
        Repository = repository;
    }
}