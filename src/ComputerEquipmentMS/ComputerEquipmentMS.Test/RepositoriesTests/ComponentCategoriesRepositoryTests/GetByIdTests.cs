using ComputerEquipmentMS.Models.Domain;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentCategoriesRepositoryTests;

public class GetByIdTests : ComponentCategoriesRepositoryTestBase
{
    public GetByIdTests()
    {
        base.FillDbWithTestData();
    }

    [Theory]
    [MemberData(nameof(ExistingIds))]
    public void GetCategoryByExistingIdShouldReturnCorrectCategory(int categoryId, ComponentCategory expectedCategory)
    {
        var category = Repository.GetById(categoryId);
        category.Should().BeEquivalentTo(expectedCategory);
    }
    
    public static IEnumerable<object[]> ExistingIds =>
        new List<object[]>
        {
            new object[] 
            { 
                1, new ComponentCategory
                {
                    Id = 1,
                    Name = "video cards",
                } 
            },
            new object[] 
            { 
                2, new ComponentCategory
                {
                    Id = 2,
                    Name = "processors",
                } 
            },
            new object[] 
            { 
                3, new ComponentCategory
                {
                    Id = 3,
                    Name = "hard drives",
                } 
            },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingIds))]
    public void GetCategoryByNonExistingIdShouldReturnNull(int categoryId)
    {
        var category = Repository.GetById(categoryId);
        category.Should().BeNull();
    }

    public static IEnumerable<object[]> NonExistingIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}