using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentsRepositoryTests;

public class AddTests : ComponentsRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(NewComponents))]
    public void AddedComponentEqualExpected(Component newComponent, Component expectedAddedComponent)
    {
        /*
         * We cannot use base.FillDbWithTestData() because it creates new configurations,
         * but we need this table empty
         */
        Container.ExecScriptAsync(
            """
            select create_component_category('video cards');
            select create_component_category('processors');
            select create_component_category('hard drives');

            select create_component_manufacturer('nvidia');
            select create_component_manufacturer('intel');
            select create_component_manufacturer('samsung');
            """
        ).Wait();
        
        var addedComponent = Repository.Add(newComponent);
        addedComponent.Should().BeEquivalentTo(expectedAddedComponent);
    }
    
    public static IEnumerable<object[]> NewComponents =>
        new List<object[]>
        {
            new object[]
            {
                new Component
                {
                    ComponentCategoryId = 2,
                    ComponentManufacturerId = 2,
                    Name = "new component",
                    Specifications = new ComponentSpecifications { ["spec"] = "value" },
                    Cost = 15000,
                    WarrantyPeriod = Period.FromYears(1),
                },
                new Component
                {
                    Id = 1,
                    ComponentCategoryId = 2,
                    ComponentManufacturerId = 2,
                    Name = "new component",
                    Specifications = new ComponentSpecifications { ["spec"] = "value" },
                    Cost = 15000,
                    WarrantyPeriod = Period.FromYears(1),
                }
            }
        };



    [Theory]
    [MemberData(nameof(Components))]
    public void AddNewComponentDoesNotAffectOtherComponents(Component newComponent, ICollection<Component> expectedComponents)
    {
        FillDbWithTestData();
        
        Repository.Add(newComponent);
        var components = Repository.GetAll();
        components.Should().BeEquivalentTo(expectedComponents);
    }

    public static IEnumerable<object[]> Components =>
        new List<object[]>
        {
            new object[]
            {
                new Component
                {
                    ComponentCategoryId = 2,
                    ComponentManufacturerId = 2,
                    Name = "new component",
                    Specifications = new ComponentSpecifications { ["spec"] = "value" },
                    Cost = 15000,
                    WarrantyPeriod = Period.FromYears(1),
                },
                new[]
                {
                    new Component
                    {
                        Id = 1,
                        ComponentCategoryId = 1,
                        ComponentManufacturerId = 1,
                        Name = "rtx 4090",
                        Specifications = new ComponentSpecifications { ["fans"] = "3", ["mass"] = "2 kg" },
                        Cost = 90000,
                        WarrantyPeriod = Period.FromYears(1),
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
                    new Component
                    {
                        Id = 4,
                        ComponentCategoryId = 2,
                        ComponentManufacturerId = 2,
                        Name = "new component",
                        Specifications = new ComponentSpecifications { ["spec"] = "value" },
                        Cost = 15000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                }
            }
        };
}