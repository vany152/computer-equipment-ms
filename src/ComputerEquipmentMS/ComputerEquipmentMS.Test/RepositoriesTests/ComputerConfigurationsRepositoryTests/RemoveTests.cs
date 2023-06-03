using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComputerConfigurationsRepositoryTests;

public class RemoveTests : ComputerConfigurationsRepositoryTestBase
{
     public RemoveTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingConfigurations))]
    public void RemoveExistingConfigurationShouldReturnTrue(int configurationId)
    {
        var removalSuccessful = Repository.Remove(configurationId);
        removalSuccessful.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> ExistingConfigurations =>
        new List<object[]>
        {
            new object[] { 1 },
            new object[] { 2 },
        };
    
    

    [Theory]
    [MemberData(nameof(NonExistingConfigurations))]
    public void RemoveNonExistingConfigurationShouldReturnFalse(int configurationId)
    {
        var removalSuccessful = Repository.Remove(configurationId);
        removalSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingConfigurations =>
        new List<object[]>
        {
            new object[] { 15 },
            new object[] { 45 },
            new object[] { 54 },
        };
    
    
    
    [Theory]
    [MemberData(nameof(RemoveConfigurationShouldNotAffectOtherConfigurationsTestData))]
    public void RemoveExistingConfigurationShouldNotAffectOtherConfiguration(int configurationId, ICollection<ComputerConfiguration> expectedConfigurations)
    {
        var removalSuccessful = Repository.Remove(configurationId);
        var configurations = Repository.GetAll();
        
        removalSuccessful.Should().BeTrue();
        configurations.Should().BeEquivalentTo(expectedConfigurations);
    }
    
    public static IEnumerable<object[]> RemoveConfigurationShouldNotAffectOtherConfigurationsTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                1,
                new []
                {
                    new ComputerConfiguration
                    {
                        Id = 2,
                        Name = "configuration 2",
                        Components = new [] { 1, 3 },
                        WarrantyPeriod = Period.FromYears(2),
                        Margin = 1500,
                    },
                },
            },
            new object[] 
            { 
                2, 
                new []
                {
                    new ComputerConfiguration
                    {
                        Id = 1,
                        Name = "configuration 1",
                        Components = new [] { 1, 2 },
                        WarrantyPeriod = Period.FromYears(1),
                        Margin = 1000,
                    },
                }
            },
        };
}