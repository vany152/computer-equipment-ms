using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentManufacturersRepositoryTests;

public class GetByCriteriaTests : ComponentManufacturersRepositoryTestBase
{
    public GetByCriteriaTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(GetManufacturersByIdData))]
    public void GetByIdShouldReturnCorrectManufacturerIds(int id, IEnumerable<int> expectedManufacturerIds)
    {
        var manufacturers = Repository.GetByCriteria(manufacturer => manufacturer.Id == id);
        var manufacturerIds = GetManufacturerIds(manufacturers);
        manufacturerIds.Should().BeEquivalentTo(expectedManufacturerIds);
    }
    
    public static IEnumerable<object[]> GetManufacturersByIdData =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
            new object[] { 45, Array.Empty<int>() },
            new object[] { 54, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetManufacturersByNameData))]
    public void GetByNameShouldReturnCorrectManufacturerIds(string name, IEnumerable<int> expectedManufacturerIds)
    {
        var manufacturers = Repository.GetByCriteria(manufacturer => manufacturer.Name == name);
        var manufacturerIds = GetManufacturerIds(manufacturers);
        manufacturerIds.Should().BeEquivalentTo(expectedManufacturerIds);    
    }
    
    public static IEnumerable<object[]> GetManufacturersByNameData =>
        new List<object[]>
        {
            new object[] { "nvidia", new [] {1} },
            new object[] { "intel", new [] {2} },
            new object[] { "samsung", new [] {3} },
            new object[] { "name", Array.Empty<int>() },
        };



    private static IEnumerable<int> GetManufacturerIds(IEnumerable<ComponentManufacturer> manufacturers) =>
        manufacturers.Select(manufacturer => manufacturer.Id);
    
}