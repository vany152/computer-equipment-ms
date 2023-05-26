using NodaTime;
using Server.Models.Auxiliary;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentsRepositoryTests;

public class GetByIdTests : ComponentsRepositoryTestBase
{
    public GetByIdTests()
    {
        base.FillDbWithTestData();
    }

    [Theory]
    [MemberData(nameof(ExistingIds))]
    public void GetComponentByExistingIdShouldReturnCorrectComponent(int componentId, Component expectedComponent)
    {
        var component = Repository.GetById(componentId);
        component.Should().BeEquivalentTo(expectedComponent);
    }
    
    public static IEnumerable<object[]> ExistingIds =>
        new List<object[]>
        {
            new object[] 
            { 
                1, new Component
                {
                    Id = 1,
                    ComponentCategoryId = 1,
                    ComponentManufacturerId = 1,
                    Name = "rtx 4090",
                    Specifications = new ComponentSpecifications { ["fans"] = "3", ["mass"] = "2 kg" },
                    Cost = 90000,
                    WarrantyPeriod = Period.FromYears(1),
                } 
            },
            new object[] 
            { 
                2, new Component
                {
                    Id = 2,
                    ComponentCategoryId = 2,
                    ComponentManufacturerId = 2,
                    Name = "intel core i7 12700",
                    Specifications = new ComponentSpecifications { ["power"] = "100 wt", ["mass"] = "50 g" },
                    Cost = 30000,
                    WarrantyPeriod = Period.FromYears(1),
                } 
            },
            new object[] 
            { 
                3, new Component
                {
                    Id = 3,
                    ComponentCategoryId = 3,
                    ComponentManufacturerId = 3,
                    Name = "samsung 980 evo ssd",
                    Specifications = new ComponentSpecifications { ["capacity"] = "980 gb", ["mass"] = "50 g", ["reading speed"] = "3500 mb/sec" },
                    Cost = 15000,
                    WarrantyPeriod = Period.FromYears(5),
                } 
            },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingIds))]
    public void GetComponentByNonExistingIdShouldReturnNull(int componentId)
    {
        var component = Repository.GetById(componentId);
        component.Should().BeNull();
    }

    public static IEnumerable<object[]> NonExistingIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}