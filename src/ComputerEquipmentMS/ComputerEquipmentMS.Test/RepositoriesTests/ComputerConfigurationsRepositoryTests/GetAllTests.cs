using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComputerConfigurationsRepositoryTests;

public class GetAllTests : ComputerConfigurationsRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(ComputerConfigurations))]
    public void GetAllConfigurationsOfNonEmptyRepositoryShouldReturnCorrectConfigurationsCollection(ICollection<ComputerConfiguration> expectedConfigurations)
    {
        FillDbWithTestData();

        var configuration = Repository.GetAll();
        configuration.Should().BeEquivalentTo(expectedConfigurations);
    }

    public static IEnumerable<object[]> ComputerConfigurations =>
        new List<object[]>
        {
            new object[]
            {
                new[]
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
                        Name = "configuration 2",
                        ComponentIds = new [] { 1, 3 },
                        WarrantyPeriod = Period.FromYears(2),
                        Margin = 1500,
                    },
                }
            }
        };



    [Fact]
    public void GetAllComputerConfigurationsOfEmptyRepositoryShouldReturnEmptyCollection()
    {
        var configuration = Repository.GetAll();
        configuration.Should().BeEmpty();
    }
}