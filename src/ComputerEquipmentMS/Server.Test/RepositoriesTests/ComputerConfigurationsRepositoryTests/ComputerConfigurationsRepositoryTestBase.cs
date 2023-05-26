using Server.DataAccess;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComputerConfigurationsRepositoryTests;

public abstract class ComputerConfigurationsRepositoryTestBase : RepositoryTestBase<ComputerConfiguration, int>
{
    protected ComputerConfigurationsRepositoryTestBase()
    {
        Repository = new ComputerConfigurationsNpgsqlRepository(ConnectionString);
    }
    
    protected override void FillDbWithTestData() =>
        Container.ExecScriptAsync(
            """
            select create_component_category('video cards');
            select create_component_category('processors');
            select create_component_category('hard drives');

            select create_component_manufacturer('nvidia');
            select create_component_manufacturer('intel');
            select create_component_manufacturer('samsung');

            select create_component(1, 1, 'rtx 4090', '{"fans": "3", "mass": "2 kg"}'::jsonb, 90000, '12 months'::interval);
            select create_component(2, 2, 'intel core i7 12700', '{"power": "100 wt", "mass": "50 g"}'::jsonb, 30000, '12 months'::interval);
            select create_component(3, 3, 'samsung 980 evo ssd', '{"capacity": "980 gb", "reading speed": "3500 mb/sec", "mass": "50 g"}', 15000, '30 months'::interval);

            select create_configuration('configuration 1', '1 year'::interval, 1000, '{1, 2}');
            select create_configuration('configuration 2', '2 years'::interval, 1500, '{1, 3}');
            """
        ).Wait();
}