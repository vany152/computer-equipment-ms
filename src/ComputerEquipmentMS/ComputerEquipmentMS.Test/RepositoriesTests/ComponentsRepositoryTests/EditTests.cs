using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentsRepositoryTests;

public class EditTests : ComponentsRepositoryTestBase
{
    public EditTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingComponents))]
    public void EditExistingComponentShouldCorrectlyEditComponent(Component componentToEdit, Component expectedEditedComponent)
    {
        var editSuccessful = Repository.Edit(componentToEdit);
        var editedComponent = Repository.GetById(componentToEdit.Id);

        editSuccessful.Should().BeTrue();
        editedComponent.Should().BeEquivalentTo(expectedEditedComponent);
    }
    
    public static IEnumerable<object[]> ExistingComponents =>
        new List<object[]>
        {
            new object[] 
            { 
                new Component
                {
                    Id = 1,
                    ComponentCategoryId = 3,
                    ComponentManufacturerId = 2,
                    Name = "new name",
                    Specifications = new ComponentSpecifications { ["new spec"] = "val", ["mass"] = "50 kg" },
                    Cost = 10,
                    WarrantyPeriod = Period.FromYears(5),
                },
                new Component
                {
                    Id = 1,
                    ComponentCategoryId = 3,
                    ComponentManufacturerId = 2,
                    Name = "new name",
                    Specifications = new ComponentSpecifications { ["new spec"] = "val", ["mass"] = "50 kg" },
                    Cost = 10,
                    WarrantyPeriod = Period.FromYears(5),
                }, 
            },
        };

    
    
    [Theory]
    [MemberData(nameof(NonExistingComponents))]
    public void EditNonExistingComponentShouldReturnFalse(Component componentToEdit)
    {
        var editSuccessful = Repository.Edit(componentToEdit);
        editSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingComponents =>
        new List<object[]>
        {
            new object[] { new Component { Id = 15, Name = string.Empty, Specifications = new ComponentSpecifications(), WarrantyPeriod = Period.Zero }, },
            new object[] { new Component { Id = 45, Name = string.Empty, Specifications = new ComponentSpecifications(), WarrantyPeriod = Period.Zero }, },
            new object[] { new Component { Id = 54, Name = string.Empty, Specifications = new ComponentSpecifications(), WarrantyPeriod = Period.Zero }, },
        };
    
    
    
    [Theory]
    [MemberData(nameof(EditComponentShouldNotAffectOtherComponentsTestData))]
    public void EditExistingComponentShouldNotAffectOtherComponent(Component componentToEdit, ICollection<Component> expectedComponents)
    {
        var editSuccessful = Repository.Edit(componentToEdit);
        var components = Repository.GetAll();
        
        editSuccessful.Should().BeTrue();
        components.Should().BeEquivalentTo(expectedComponents);
    }
    
    public static IEnumerable<object[]> EditComponentShouldNotAffectOtherComponentsTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                new Component
                {
                    Id = 1,
                    ComponentCategoryId = 3,
                    ComponentManufacturerId = 2,
                    Name = "new name",
                    Specifications = new ComponentSpecifications { ["new spec"] = "val", ["mass"] = "50 kg" },
                    Cost = 10,
                    WarrantyPeriod = Period.FromYears(5),
                },
                new []
                {
                    new Component
                    {
                        Id = 1,
                        ComponentCategoryId = 3,
                        ComponentManufacturerId = 2,
                        Name = "new name",
                        Specifications = new ComponentSpecifications { ["new spec"] = "val", ["mass"] = "50 kg" },
                        Cost = 10,
                        WarrantyPeriod = Period.FromYears(5),
                    },
                    new Component
                    {
                        Id = 2,
                        ComponentCategoryId = 2,
                        ComponentManufacturerId = 2,
                        Name = "intel core i7 12700",
                        Specifications = new ComponentSpecifications { ["power"] = "100 wt", ["mass"] = "50 g" },
                        Cost = 30000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 3,
                        ComponentCategoryId = 3,
                        ComponentManufacturerId = 3,
                        Name = "samsung 980 evo ssd",
                        Specifications = new ComponentSpecifications { ["capacity"] = "980 gb", ["mass"] = "50 g", ["reading speed"] = "3500 mb/sec" },
                        Cost = 15000,
                        WarrantyPeriod = Period.FromYears(5),
                    },
                },
            },
        };
}