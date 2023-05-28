using ComputerEquipmentMS.Models.Domain;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentCategoriesRepositoryTests;

public class RemoveTests : ComponentCategoriesRepositoryTestBase
{
     public RemoveTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingCategories))]
    public void RemoveExistingCategoryShouldReturnTrue(int categoryId)
    {
        var removalSuccessful = Repository.Remove(categoryId);
        removalSuccessful.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> ExistingCategories =>
        new List<object[]>
        {
            new object[] { 1 },
            new object[] { 2 },
            new object[] { 3 },
        };
    
    

    [Theory]
    [MemberData(nameof(NonExistingCategories))]
    public void RemoveNonExistingCategoryShouldReturnFalse(int categoryId)
    {
        var removalSuccessful = Repository.Remove(categoryId);
        removalSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingCategories =>
        new List<object[]>
        {
            new object[] { 15 },
            new object[] { 45 },
            new object[] { 54 },
        };
    
    
    
    [Theory]
    [MemberData(nameof(RemoveCategoryShouldNotAffectOtherCategoriesTestData))]
    public void RemoveExistingCategoryShouldNotAffectOtherCategory(int categoryId, ICollection<ComponentCategory> expectedCategories)
    {
        var removalSuccessful = Repository.Remove(categoryId);
        var categories = Repository.GetAll();
        
        removalSuccessful.Should().BeTrue();
        categories.Should().BeEquivalentTo(expectedCategories);
    }
    
    public static IEnumerable<object[]> RemoveCategoryShouldNotAffectOtherCategoriesTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                1,
                new []
                {
                    new ComponentCategory
                    {
                        Id = 2,
                        Name = "processors",
                    },
                    new ComponentCategory
                    {
                        Id = 3,
                        Name = "hard drives",
                    },
                },
            },
            new object[] 
            { 
                2, 
                new []
                {
                    new ComponentCategory
                    {
                        Id = 1,
                        Name = "video cards",
                    },
                    new ComponentCategory
                    {
                        Id = 3,
                        Name = "hard drives",
                    },
                }
            },
            new object[] 
            { 
                3, 
                new []
                {
                    new ComponentCategory
                    {
                        Id = 1,
                        Name = "video cards",
                    },
                    new ComponentCategory
                    {
                        Id = 2,
                        Name = "processors",
                    },
                } 
            },
        };
}