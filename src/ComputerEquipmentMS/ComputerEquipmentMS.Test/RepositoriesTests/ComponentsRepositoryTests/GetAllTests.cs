using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentsRepositoryTests;

public class GetAllTests : ComponentsRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(Components))]
    public void GetAllComponentsOfNonEmptyRepositoryShouldReturnCorrectComponentsCollection(ICollection<Component> expectedComponents)
    {
        FillDbWithTestData();

        var components = Repository.GetAll();
        components.Should().BeEquivalentTo(expectedComponents);
    }

    public static IEnumerable<object[]> Components =>
        new List<object[]>
        {
            new object[]
            {
                new[]
                {
                    new Component
                    {
                        Id = 1,
                        ComponentCategory = 1,
                        ComponentManufacturer = 1,
                        Name = "rtx 4090",
                        Specifications = new ComponentSpecifications { ["fans"] = "3", ["mass"] = "2 kg" },
                        Cost = 90000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 2,
                        ComponentCategory = 2,
                        ComponentManufacturer = 2,
                        Name = "intel core i7 12700",
                        Specifications = new ComponentSpecifications { ["power"] = "100 wt", ["mass"] = "50 g" },
                        Cost = 30000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 3,
                        ComponentCategory = 3,
                        ComponentManufacturer = 3,
                        Name = "samsung 980 evo ssd",
                        Specifications = new ComponentSpecifications { ["capacity"] = "980 gb", ["mass"] = "50 g", ["reading speed"] = "3500 mb/sec" },
                        Cost = 15000,
                        WarrantyPeriod = Period.FromYears(5),
                    },
                }
            }
        };



    [Fact]
    public void GetAllComponentsOfEmptyRepositoryShouldReturnEmptyCollection()
    {
        var components = Repository.GetAll();
        components.Should().BeEmpty();
    }
}