using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentsRepositoryTests;

public class GetByCriteriaTests : ComponentsRepositoryTestBase
{
    public GetByCriteriaTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(GetComponentsByIdData))]
    public void GetByIdShouldReturnCorrectComponentIds(int id, IEnumerable<int> expectedComponentIds)
    {
        var components = Repository.GetByCriteria(component => component.Id == id);
        var componentIds = GetComponentIds(components);
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> GetComponentsByIdData =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
            new object[] { 45, Array.Empty<int>() },
            new object[] { 54, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetComponentsByCategoryIdData))]
    public void GetByCategoryIdShouldReturnCorrectComponentIds(int id, IEnumerable<int> expectedComponentIds)
    {
        var components = Repository.GetByCriteria(component => component.ComponentCategory == id);
        var componentIds = GetComponentIds(components);
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> GetComponentsByCategoryIdData =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
            new object[] { 45, Array.Empty<int>() },
            new object[] { 54, Array.Empty<int>() },
        };
    
    [Theory]
    [MemberData(nameof(GetComponentsByManufacturerIdData))]
    public void GetByManufacturerIdShouldReturnCorrectComponentIds(int id, IEnumerable<int> expectedComponentIds)
    {
        var components = Repository.GetByCriteria(component => component.ComponentManufacturer == id);
        var componentIds = GetComponentIds(components);
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> GetComponentsByManufacturerIdData =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
            new object[] { 45, Array.Empty<int>() },
            new object[] { 54, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetComponentsByNameData))]
    public void GetByNameShouldReturnCorrectComponentIds(string name, IEnumerable<int> expectedComponentIds)
    {
        var components = Repository.GetByCriteria(component => component.Name == name);
        var componentIds = GetComponentIds(components);
        componentIds.Should().BeEquivalentTo(expectedComponentIds);    
    }
    
    public static IEnumerable<object[]> GetComponentsByNameData =>
        new List<object[]>
        {
            new object[] { "rtx 4090", new [] {1} },
            new object[] { "intel core i7 12700", new [] {2} },
            new object[] { "samsung 980 evo ssd", new [] {3} },
            new object[] { "name", Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetComponentsBySpecificationsData))]
    public void GetBySpecificationsShouldReturnCorrectComponentIds(ComponentSpecifications specifications, IEnumerable<int> expectedComponentIds)
    {
        var components = Repository.GetByCriteria(component => Util.DictionariesEqual(component.Specifications, specifications));
        var componentIds = GetComponentIds(components);
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> GetComponentsBySpecificationsData =>
        new List<object[]>
        {
            new object[] { new ComponentSpecifications { ["fans"] = "3", ["mass"] = "2 kg" }, new [] {1} },
            new object[] { new ComponentSpecifications { ["power"] = "100 wt", ["mass"] = "50 g" }, new [] {2} },
            new object[] { new ComponentSpecifications { ["capacity"] = "980 gb", ["mass"] = "50 g", ["reading speed"] = "3500 mb/sec" }, new [] { 3 } },
            new object[] { new ComponentSpecifications { ["spec"] = "somespec", ["fans"] = "3" }, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetComponentsByCostDateData))]
    public void GetByCostShouldReturnCorrectComponentIds(decimal cost, IEnumerable<int> expectedComponentIds)
    {
        var components = Repository.GetByCriteria(component => component.Cost == cost);
        var componentIds = GetComponentIds(components);
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> GetComponentsByCostDateData =>
        new List<object[]>
        {
            new object[] { 90000, new [] {1} },
            new object[] { 30000, new [] {2} },
            new object[] { 15000, new [] {3} },
            new object[] { 5, Array.Empty<int>() },
            new object[] { 500_000, Array.Empty<int>() },
        };

    [Theory]
    [MemberData(nameof(GetSalePositionsByWarrantyPeriodData))]
    public void GetByWarrantyPeriodShouldReturnCorrectComponentIds(Period warrantyPeriod, IEnumerable<int> expectedComponentIds)
    {
        var components = Repository.GetByCriteria(sp => sp.WarrantyPeriod == warrantyPeriod);
        var componentIds = GetComponentIds(components);
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> GetSalePositionsByWarrantyPeriodData =>
        new List<object[]>
        {
            new object[] { Period.FromYears(1), new[] { 1, 2 } },
            new object[] { Period.FromYears(5), new[] { 3, } },
            new object[] { Period.FromYears(15), Array.Empty<int>() },
            new object[] { Period.FromDays(3), Array.Empty<int>() },
        };
    
    

    private static IEnumerable<int> GetComponentIds(IEnumerable<Component> components) =>
        components.Select(component => component.Id);
    
}