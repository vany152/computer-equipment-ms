using NodaTime;
using Server.Models.Auxiliary;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentsRepositoryTests;

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
                        ComponentCategoryId = 1,
                        ComponentManufacturerId = 1,
                        Name = "rtx 4090",
                        Specifications = new ComponentSpecifications { ["fans"] = "3", ["mass"] = "2 kg" },
                        Cost = 90000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 2,
                        ComponentCategoryId = 2,
                        ComponentManufacturerId = 2,
                        Name = "intel core i7 12700",
                        Specifications = new ComponentSpecifications { ["power"] = "100 wt", ["mass"] = "50 g" },
                        Cost = 30000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 3,
                        ComponentCategoryId = 3,
                        ComponentManufacturerId = 3,
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