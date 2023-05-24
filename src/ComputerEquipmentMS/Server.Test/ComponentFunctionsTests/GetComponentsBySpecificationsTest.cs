using Server.Models.Auxiliary;

namespace Server.Test.ComponentFunctionsTests;

public class GetComponentsBySpecificationsTest : ComponentFunctionsTest
{
    [Theory]
    [MemberData(nameof(ExistingContacts))]
    public void GetComponentsByExistingSpecificationsShouldReturnCorrectComponentIds(
        ComponentSpecifications specifications, 
        ICollection<int> expectedComponentIds) 
    {
        var components = Executor.GetComponentsBySpecifications(specifications);
        var componentIds = components.Select(component => component.Id); 
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> ExistingContacts =>
        new List<object[]>
        {
            new object[] { new ComponentSpecifications { ["mass"] = "50 g" }, new [] {2, 3} },
            new object[] { new ComponentSpecifications { ["mass"] = "2 kg" }, new [] {1} },
            new object[] { new ComponentSpecifications { ["fans"] = "3" }, new [] {1} },
            new object[] { new ComponentSpecifications { ["power"] = "100 wt" }, new [] {2} },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingContacts))]
    public void GetComponentsByNotExistingSpecificationShouldReturnEmptyCollection(ComponentSpecifications specifications) 
    {
        var components = Executor.GetComponentsBySpecifications(specifications);
        components.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> NonExistingContacts =>
        new List<object[]>
        {
            new object[] { new ComponentSpecifications { ["mass"] = "15 kg" } },
            new object[] { new ComponentSpecifications { ["resolution"] = "1920 x 1080" } },
        };
}