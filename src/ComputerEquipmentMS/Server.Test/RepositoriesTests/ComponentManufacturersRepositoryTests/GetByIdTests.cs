using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentManufacturersRepositoryTests;

public class GetByIdTests : ComponentManufacturersRepositoryTestBase
{
    public GetByIdTests()
    {
        base.FillDbWithTestData();
    }

    [Theory]
    [MemberData(nameof(ExistingIds))]
    public void GetManufacturerByExistingIdShouldReturnCorrectManufacturer(int manufacturerId, ComponentManufacturer expectedManufacturer)
    {
        var manufacturer = Repository.GetById(manufacturerId);
        manufacturer.Should().BeEquivalentTo(expectedManufacturer);
    }
    
    public static IEnumerable<object[]> ExistingIds =>
        new List<object[]>
        {
            new object[] 
            { 
                1, new ComponentManufacturer
                {
                    Id = 1,
                    Name = "nvidia",
                } 
            },
            new object[] 
            { 
                2, new ComponentManufacturer
                {
                    Id = 2,
                    Name = "intel",
                } 
            },
            new object[] 
            { 
                3, new ComponentManufacturer
                {
                    Id = 3,
                    Name = "samsung",
                } 
            },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingIds))]
    public void GetManufacturerByNonExistingIdShouldReturnNull(int manufacturerId)
    {
        var manufacturer = Repository.GetById(manufacturerId);
        manufacturer.Should().BeNull();
    }

    public static IEnumerable<object[]> NonExistingIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}