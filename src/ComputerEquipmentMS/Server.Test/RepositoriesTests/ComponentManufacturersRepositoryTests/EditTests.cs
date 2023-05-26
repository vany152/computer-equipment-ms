using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentManufacturersRepositoryTests;

public class EditTests : ComponentManufacturersRepositoryTestBase
{
    public EditTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingManufacturers))]
    public void EditExistingManufacturerShouldCorrectlyEditManufacturer(ComponentManufacturer manufacturerToEdit, ComponentManufacturer expectedEditedManufacturer)
    {
        var editSuccessful = Repository.Edit(manufacturerToEdit);
        var editedManufacturer = Repository.GetById(manufacturerToEdit.Id);

        editSuccessful.Should().BeTrue();
        editedManufacturer.Should().BeEquivalentTo(expectedEditedManufacturer);
    }
    
    public static IEnumerable<object[]> ExistingManufacturers =>
        new List<object[]>
        {
            new object[] 
            { 
                new ComponentManufacturer
                {
                    Id = 1,
                    Name = "new manufacturer #1",
                },
                new ComponentManufacturer
                {
                    Id = 1,
                    Name = "new manufacturer #1",
                }, 
            },
            new object[] 
            { 
                new ComponentManufacturer
                {
                    Id = 2,
                    Name = "new manufacturer #2",
                }, 
                new ComponentManufacturer
                {
                    Id = 2,
                    Name = "new manufacturer #2",
                }, 
            },
            new object[] 
            { 
                new ComponentManufacturer
                {
                    Id = 3,
                    Name = "new manufacturer #3",
                }, 
                new ComponentManufacturer
                {
                    Id = 3,
                    Name = "new manufacturer #3",
                }, 
            },
        };

    
    
    [Theory]
    [MemberData(nameof(NonExistingManufacturers))]
    public void EditNonExistingManufacturerShouldReturnFalse(ComponentManufacturer manufacturerToEdit)
    {
        var editSuccessful = Repository.Edit(manufacturerToEdit);
        editSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingManufacturers =>
        new List<object[]>
        {
            new object[] { new ComponentManufacturer { Id = 15, Name = string.Empty }, },
            new object[] { new ComponentManufacturer { Id = 45, Name = string.Empty }, },
            new object[] { new ComponentManufacturer { Id = 54, Name = string.Empty }, },
        };
    
    
    
    [Theory]
    [MemberData(nameof(EditManufacturerShouldNotAffectOtherManufacturersTestData))]
    public void EditExistingManufacturerShouldNotAffectOtherManufacturer(ComponentManufacturer manufacturerToEdit, ICollection<ComponentManufacturer> expectedManufacturers)
    {
        var editSuccessful = Repository.Edit(manufacturerToEdit);
        var manufacturers = Repository.GetAll();
        
        editSuccessful.Should().BeTrue();
        manufacturers.Should().BeEquivalentTo(expectedManufacturers);
    }
    
    public static IEnumerable<object[]> EditManufacturerShouldNotAffectOtherManufacturersTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                new ComponentManufacturer
                {
                    Id = 1,
                    Name = "new manufacturer #1",
                },
                new []
                {
                    new ComponentManufacturer
                    {
                        Id = 1,
                        Name = "new manufacturer #1",
                    },
                    new ComponentManufacturer
                    {
                        Id = 2,
                        Name = "intel",
                    },
                    new ComponentManufacturer
                    {
                        Id = 3,
                        Name = "samsung",
                    },
                },
            },
            new object[] 
            { 
                new ComponentManufacturer
                {
                    Id = 2,
                    Name = "new manufacturer #2",
                }, 
                new []
                {
                    new ComponentManufacturer
                    {
                        Id = 1,
                        Name = "nvidia",
                    },
                    new ComponentManufacturer
                    {
                        Id = 2,
                        Name = "new manufacturer #2",
                    },
                    new ComponentManufacturer
                    {
                        Id = 3,
                        Name = "samsung",
                    },
                }
            },
            new object[] 
            { 
                new ComponentManufacturer
                {
                    Id = 3,
                    Name = "new manufacturer #3",
                }, 
                new []
                {
                    new ComponentManufacturer
                    {
                        Id = 1,
                        Name = "nvidia",
                    },
                    new ComponentManufacturer
                    {
                        Id = 2,
                        Name = "intel",
                    },
                    new ComponentManufacturer
                    {
                        Id = 3,
                        Name = "new manufacturer #3",
                    },
                } 
            },
        };
}