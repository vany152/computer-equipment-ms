using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComputerConfigurationsRepositoryTests;

public class GetByIdTests : ComputerConfigurationsRepositoryTestBase
{
    public GetByIdTests()
    {
        base.FillDbWithTestData();
    }

    [Theory]
    [MemberData(nameof(ExistingIds))]
    public void GetConfigurationByExistingIdShouldReturnCorrectConfiguration(int configurationId, ComputerConfiguration expectedConfiguration)
    {
        var configuration = Repository.GetById(configurationId);
        configuration.Should().BeEquivalentTo(expectedConfiguration);
    }
    
    public static IEnumerable<object[]> ExistingIds =>
        new List<object[]>
        {
            new object[] 
            { 
                1, new ComputerConfiguration
                {
                    Id = 1,
                    Name = "configuration 1",
                    Components = new [] { 1, 2 },
                    WarrantyPeriod = Period.FromYears(1),
                    Margin = 1000,
                },
            },
            new object[] 
            { 
                2, new ComputerConfiguration
                {
                    Id = 2,
                    Name = "configuration 2",
                    Components = new [] { 1, 3 },
                    WarrantyPeriod = Period.FromYears(2),
                    Margin = 1500,
                },
            },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingIds))]
    public void GetConfigurationByNonExistingIdShouldReturnNull(int configurationId)
    {
        var configuration = Repository.GetById(configurationId);
        configuration.Should().BeNull();
    }

    public static IEnumerable<object[]> NonExistingIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}