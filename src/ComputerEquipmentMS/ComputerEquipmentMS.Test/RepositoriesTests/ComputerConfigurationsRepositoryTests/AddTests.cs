using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComputerConfigurationsRepositoryTests;

public class AddTests : ComputerConfigurationsRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(NewConfigurations))]
    public void AddedConfigurationEqualExpected(ComputerConfiguration newConfiguration, ComputerConfiguration expectedAddedConfiguration)
    {
        /*
         * We cannot use base.FillDbWithTestData() because it creates new configurations,
         * but we need this table empty
         */
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
            """
        ).Wait();
        
        var addedConfiguration = Repository.Add(newConfiguration);
        addedConfiguration.Should().BeEquivalentTo(expectedAddedConfiguration);
    }
    
    public static IEnumerable<object[]> NewConfigurations =>
        new List<object[]>
        {
            new object[]
            {
                new ComputerConfiguration
                {
                    Name = "New configuration",
                    Components = new [] { 1, 2 },
                    WarrantyPeriod = Period.FromYears(1),
                    Margin = 2000,
                },
                new ComputerConfiguration
                {
                    Id = 1,
                    Name = "New configuration",
                    Components = new [] { 1, 2 },
                    WarrantyPeriod = Period.FromYears(1),
                    Margin = 2000,
                },
            }
        };



    [Theory]
    [MemberData(nameof(Configurations))]
    public void AddNewConfigurationDoesNotAffectOtherConfigurations(ComputerConfiguration newConfiguration, ICollection<ComputerConfiguration> expectedConfigurations)
    {
        FillDbWithTestData();
        
        Repository.Add(newConfiguration);
        var configurations = Repository.GetAll();
        configurations.Should().BeEquivalentTo(expectedConfigurations);
    }

    public static IEnumerable<object[]> Configurations =>
        new List<object[]>
        {
            new object[]
            {
                new ComputerConfiguration
                {
                    Name = "New configuration",
                    Components = new [] { 1, 2 },
                    WarrantyPeriod = Period.FromYears(1),
                    Margin = 2000,
                },
                new[]
                {
                    new ComputerConfiguration
                    {
                        Id = 1,
                        Name = "configuration 1",
                        Components = new [] { 1, 2 },
                        WarrantyPeriod = Period.FromYears(1),
                        Margin = 1000,
                    },
                    new ComputerConfiguration
                    {
                        Id = 2,
                        Name = "configuration 2",
                        Components = new [] { 1, 3 },
                        WarrantyPeriod = Period.FromYears(2),
                        Margin = 1500,
                    },
                    new ComputerConfiguration
                    {
                        Id = 3,
                        Name = "New configuration",
                        Components = new [] { 1, 2 },
                        WarrantyPeriod = Period.FromYears(1),
                        Margin = 2000,
                    },
                }
            }
        };
}