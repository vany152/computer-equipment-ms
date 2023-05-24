namespace Server.Test.ComponentFunctionsTests;

public class GetComponentsByCategoryTest : ComponentFunctionsTest
{
    [Theory]
    [MemberData(nameof(ExistingComponentCategoryIds))]
    public void GetComponentsByExistingCategoryShouldReturnCorrectComponentsIds(
        int categoryId, 
        ICollection<int> expectedComponentIds) 
    {
        var components = Executor.GetComponentsByCategory(categoryId);
        var componentIds = components.Select(component => component.Id); 
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> ExistingComponentCategoryIds =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
        };
    
    [Theory]
    [MemberData(nameof(NonExistingComponentCategoryIds))]
    public void GetComponentsByNonExistingCategoryShouldReturnEmptyCollection(int categoryId) 
    {
        var components = Executor.GetComponentsByCategory(categoryId);
        components.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> NonExistingComponentCategoryIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}