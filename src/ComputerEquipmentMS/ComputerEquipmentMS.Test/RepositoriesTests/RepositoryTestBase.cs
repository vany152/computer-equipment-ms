using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models;

namespace ComputerEquipmentMS.Test.RepositoriesTests;

public abstract class RepositoryTestBase<TItem, TId> : TestBase
    where TItem : IIdentifiable<TId>
    where TId : struct 
{
    public required IRepository<TItem, TId> Repository { get; init; }

    protected abstract void FillDbWithTestData();
}
