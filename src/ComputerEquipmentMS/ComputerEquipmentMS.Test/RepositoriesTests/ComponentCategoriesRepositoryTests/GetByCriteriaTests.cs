using ComputerEquipmentMS.Models.Domain;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentCategoriesRepositoryTests;

public class GetByCriteriaTests : ComponentCategoriesRepositoryTestBase
{
    public GetByCriteriaTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(GetCategoriesByIdData))]
    public void GetByIdShouldReturnCorrectCategoryIds(int id, IEnumerable<int> expectedCategoryIds)
    {
        var categories = Repository.GetByCriteria(category => category.Id == id);
        var categoryIds = GetCategoryIds(categories);
        categoryIds.Should().BeEquivalentTo(expectedCategoryIds);
    }
    
    public static IEnumerable<object[]> GetCategoriesByIdData =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
            new object[] { 45, Array.Empty<int>() },
            new object[] { 54, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetCategoriesByNameData))]
    public void GetByNameShouldReturnCorrectCategoryIds(string name, IEnumerable<int> expectedCategoryIds)
    {
        var categories = Repository.GetByCriteria(category => category.Name == name);
        var categoryIds = GetCategoryIds(categories);
        categoryIds.Should().BeEquivalentTo(expectedCategoryIds);    
    }
    
    public static IEnumerable<object[]> GetCategoriesByNameData =>
        new List<object[]>
        {
            new object[] { "video cards", new [] {1} },
            new object[] { "processors", new [] {2} },
            new object[] { "hard drives", new [] {3} },
            new object[] { "name", Array.Empty<int>() },
        };



    private static IEnumerable<int> GetCategoryIds(IEnumerable<ComponentCategory> categories) =>
        categories.Select(category => category.Id);
    
}