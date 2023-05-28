using ComputerEquipmentMS.Models.Domain;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentCategoriesRepositoryTests;

public class EditTests : ComponentCategoriesRepositoryTestBase
{
    public EditTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingCategories))]
    public void EditExistingCategoryShouldCorrectlyEditCategory(ComponentCategory categoryToEdit, ComponentCategory expectedEditedCategory)
    {
        var editSuccessful = Repository.Edit(categoryToEdit);
        var editedCategory = Repository.GetById(categoryToEdit.Id);

        editSuccessful.Should().BeTrue();
        editedCategory.Should().BeEquivalentTo(expectedEditedCategory);
    }
    
    public static IEnumerable<object[]> ExistingCategories =>
        new List<object[]>
        {
            new object[] 
            { 
                new ComponentCategory
                {
                    Id = 1,
                    Name = "new category #1",
                },
                new ComponentCategory
                {
                    Id = 1,
                    Name = "new category #1",
                }, 
            },
            new object[] 
            { 
                new ComponentCategory
                {
                    Id = 2,
                    Name = "new category #2",
                }, 
                new ComponentCategory
                {
                    Id = 2,
                    Name = "new category #2",
                }, 
            },
            new object[] 
            { 
                new ComponentCategory
                {
                    Id = 3,
                    Name = "new category #3",
                }, 
                new ComponentCategory
                {
                    Id = 3,
                    Name = "new category #3",
                }, 
            },
        };

    
    
    [Theory]
    [MemberData(nameof(NonExistingCategories))]
    public void EditNonExistingCategoryShouldReturnFalse(ComponentCategory categoryToEdit)
    {
        var editSuccessful = Repository.Edit(categoryToEdit);
        editSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingCategories =>
        new List<object[]>
        {
            new object[] { new ComponentCategory { Id = 15, Name = string.Empty }, },
            new object[] { new ComponentCategory { Id = 45, Name = string.Empty }, },
            new object[] { new ComponentCategory { Id = 54, Name = string.Empty }, },
        };
    
    
    
    [Theory]
    [MemberData(nameof(EditCategoryShouldNotAffectOtherCategoriesTestData))]
    public void EditExistingCategoryShouldNotAffectOtherCategory(ComponentCategory categoryToEdit, ICollection<ComponentCategory> expectedCategories)
    {
        var editSuccessful = Repository.Edit(categoryToEdit);
        var categories = Repository.GetAll();
        
        editSuccessful.Should().BeTrue();
        categories.Should().BeEquivalentTo(expectedCategories);
    }
    
    public static IEnumerable<object[]> EditCategoryShouldNotAffectOtherCategoriesTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                new ComponentCategory
                {
                    Id = 1,
                    Name = "new category #1",
                },
                new []
                {
                    new ComponentCategory
                    {
                        Id = 1,
                        Name = "new category #1",
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
                },
            },
            new object[] 
            { 
                new ComponentCategory
                {
                    Id = 2,
                    Name = "new category #2",
                }, 
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
                        Name = "new category #2",
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
                new ComponentCategory
                {
                    Id = 3,
                    Name = "new category #3",
                }, 
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
                    new ComponentCategory
                    {
                        Id = 3,
                        Name = "new category #3",
                    },
                } 
            },
        };
}