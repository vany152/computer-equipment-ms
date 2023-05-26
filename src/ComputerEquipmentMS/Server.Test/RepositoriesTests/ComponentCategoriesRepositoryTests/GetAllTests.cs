using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentCategoriesRepositoryTests;

public class GetAllTests : ComponentCategoriesRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(ComponentCategories))]
    public void GetAllComponentCategoriesOfNonEmptyRepositoryShouldReturnCorrectComponentCategoriesCollection(ICollection<ComponentCategory> expectedComponentCategories)
    {
        FillDbWithTestData();

        var componentCategories = Repository.GetAll();
        componentCategories.Should().BeEquivalentTo(expectedComponentCategories);
    }

    public static IEnumerable<object[]> ComponentCategories =>
        new List<object[]>
        {
            new object[]
            {
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
                }
            }
        };



    [Fact]
    public void GetAllComponentCategoriesOfEmptyRepositoryShouldReturnEmptyCollection()
    {
        var componentCategories = Repository.GetAll();
        componentCategories.Should().BeEmpty();
    }
}