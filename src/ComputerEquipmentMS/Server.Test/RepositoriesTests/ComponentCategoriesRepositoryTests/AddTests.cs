using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentCategoriesRepositoryTests;

public class AddTests : ComponentCategoriesRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(NewCategories))]
    public void AddedCategoryEqualExpected(ComponentCategory newCategory, ComponentCategory expectedAddedCategory)
    {
        var addedCategory = Repository.Add(newCategory);
        addedCategory.Should().BeEquivalentTo(expectedAddedCategory);
    }
    
    public static IEnumerable<object[]> NewCategories =>
        new List<object[]>
        {
            new object[]
            {
                new ComponentCategory
                {
                    Name = "new category",
                },
                new ComponentCategory
                {
                    Id = 1,
                    Name = "new category"
                }
            }
        };



    [Theory]
    [MemberData(nameof(Categories))]
    public void AddNewCategoryDoesNotAffectOtherCategories(ComponentCategory newCategory, ICollection<ComponentCategory> expectedCategories)
    {
        FillDbWithTestData();
        
        Repository.Add(newCategory);
        var categories = Repository.GetAll();
        categories.Should().BeEquivalentTo(expectedCategories);
    }

    public static IEnumerable<object[]> Categories =>
        new List<object[]>
        {
            new object[]
            {
                new ComponentCategory
                {
                    Name = "new category",
                },
                new[]
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
                    new ComponentCategory
                    {
                        Id = 3,
                        Name = "hard drives",
                    },
                    new ComponentCategory
                    {
                        Id = 4,
                        Name = "new category"
                    }
                }
            }
        };
}