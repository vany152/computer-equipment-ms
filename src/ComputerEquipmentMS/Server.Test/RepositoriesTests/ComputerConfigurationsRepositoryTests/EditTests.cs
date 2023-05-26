using NodaTime;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComputerConfigurationsRepositoryTests;

public class EditTests : ComputerConfigurationsRepositoryTestBase
{
    public EditTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingConfigurations))]
    public void EditExistingConfigurationShouldCorrectlyEditConfiguration(ComputerConfiguration configurationToEdit, ComputerConfiguration expectedEditedConfiguration)
    {
        var editSuccessful = Repository.Edit(configurationToEdit);
        var editedConfiguration = Repository.GetById(configurationToEdit.Id);

        editedConfiguration.Should().BeEquivalentTo(expectedEditedConfiguration);
    }
    
    public static IEnumerable<object[]> ExistingConfigurations =>
        new List<object[]>
        {
            new object[] 
            { 
                new ComputerConfiguration
                {
                    Id = 1,
                    Name = "new configurations name",
                    ComponentIds = new [] { 1, 3 },
                    WarrantyPeriod = Period.FromDays(1),
                    Margin = 5,
                },
                new ComputerConfiguration
                {
                    Id = 1,
                    Name = "new configurations name",
                    ComponentIds = new [] { 1, 3 },
                    WarrantyPeriod = Period.FromDays(1),
                    Margin = 5,
                }, 
            },
            new object[] 
            { 
                new ComputerConfiguration
                {
                    Id = 2,
                    Name = "qwerty",
                    ComponentIds = new [] { 1 },
                    WarrantyPeriod = Period.FromYears(50),
                    Margin = 50000,
                }, 
                new ComputerConfiguration
                {
                    Id = 2,
                    Name = "qwerty",
                    ComponentIds = new [] { 1 },
                    WarrantyPeriod = Period.FromYears(50),
                    Margin = 50000,
                }, 
            },
        };

    
    
    [Theory]
    [MemberData(nameof(NonExistingConfigurations))]
    public void EditNonExistingConfigurationShouldReturnFalse(ComputerConfiguration configurationToEdit)
    {
        var editSuccessful = Repository.Edit(configurationToEdit);
        editSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingConfigurations =>
        new List<object[]>
        {
            new object[] { new ComputerConfiguration { Id = 15, Name = string.Empty, WarrantyPeriod = Period.Zero, ComponentIds = Array.Empty<int>() }, },
            new object[] { new ComputerConfiguration { Id = 45, Name = string.Empty, WarrantyPeriod = Period.Zero, ComponentIds = Array.Empty<int>() }, },
            new object[] { new ComputerConfiguration { Id = 54, Name = string.Empty, WarrantyPeriod = Period.Zero, ComponentIds = Array.Empty<int>() }, },
        };
    
    
    
    [Theory]
    [MemberData(nameof(EditConfigurationShouldNotAffectOtherConfigurationsTestData))]
    public void EditExistingConfigurationShouldNotAffectOtherConfiguration(ComputerConfiguration configurationToEdit, ICollection<ComputerConfiguration> expectedConfigurations)
    {
        var editSuccessful = Repository.Edit(configurationToEdit);
        var configurations = Repository.GetAll();
        
        configurations.Should().BeEquivalentTo(expectedConfigurations);
    }
    
    public static IEnumerable<object[]> EditConfigurationShouldNotAffectOtherConfigurationsTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                new ComputerConfiguration
                {
                    Id = 1,
                    Name = "new configurations name",
                    ComponentIds = new [] { 1, 3 },
                    WarrantyPeriod = Period.FromDays(1),
                    Margin = 5,
                },
                new []
                {
                    new ComputerConfiguration
                    {
                        Id = 1,
                        Name = "new configurations name",
                        ComponentIds = new [] { 1, 3 },
                        WarrantyPeriod = Period.FromDays(1),
                        Margin = 5,
                    },
                    new ComputerConfiguration
                    {
                        Id = 2,
                        Name = "configuration 2",
                        ComponentIds = new [] { 1, 3 },
                        WarrantyPeriod = Period.FromYears(2),
                        Margin = 1500,
                    },
                },
            },
            new object[] 
            { 
                new ComputerConfiguration
                {
                    Id = 2,
                    Name = "qwerty",
                    ComponentIds = new [] { 1 },
                    WarrantyPeriod = Period.FromYears(50),
                    Margin = 50000,
                },
                new []
                {
                    new ComputerConfiguration
                    {
                        Id = 1,
                        Name = "configuration 1",
                        ComponentIds = new [] { 1, 2 },
                        WarrantyPeriod = Period.FromYears(1),
                        Margin = 1000,
                    },
                    new ComputerConfiguration
                    {
                        Id = 2,
                        Name = "qwerty",
                        ComponentIds = new [] { 1 },
                        WarrantyPeriod = Period.FromYears(50),
                        Margin = 50000,
                    },
                },
            },
        };
}