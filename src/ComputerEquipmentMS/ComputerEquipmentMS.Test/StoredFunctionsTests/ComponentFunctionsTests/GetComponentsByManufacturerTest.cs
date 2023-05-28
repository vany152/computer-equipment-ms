namespace ComputerEquipmentMS.Test.StoredFunctionsTests.ComponentFunctionsTests;

public class GetComponentsByManufacturerTest : ComponentFunctionsTest
{
    [Theory]
    [MemberData(nameof(ExistingComponentManufacturerIds))]
    public void GetComponentsByExistingManufacturerShouldReturnCorrectComponentsIds(
        int manufacturerUd, 
        ICollection<int> expectedComponentIds) 
    {
        var components = Executor.GetComponentsByManufacturer(manufacturerUd);
        var componentIds = components.Select(component => component.Id); 
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> ExistingComponentManufacturerIds =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
        };
    
    [Theory]
    [MemberData(nameof(NonExistingComponentManufacturerIds))]
    public void GetComponentsByNonExistingManufacturerShouldReturnEmptyCollection(int manufacturerId) 
    {
        var components = Executor.GetComponentsByManufacturer(manufacturerId);
        components.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> NonExistingComponentManufacturerIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}