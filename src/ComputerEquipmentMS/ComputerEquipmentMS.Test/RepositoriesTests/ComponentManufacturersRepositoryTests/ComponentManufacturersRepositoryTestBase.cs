using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentManufacturersRepositoryTests;

public abstract class ComponentManufacturersRepositoryTestBase : RepositoryTestBase<ComponentManufacturer, int>
{
    protected ComponentManufacturersRepositoryTestBase()
    {
        Repository = new ComponentManufacturersNpgsqlRepository(ConnectionString);
    }
    
    protected override void FillDbWithTestData() =>
        Container.ExecScriptAsync(
            """
            select create_component_manufacturer('nvidia');
            select create_component_manufacturer('intel');
            select create_component_manufacturer('samsung');
            """
        ).Wait();
}