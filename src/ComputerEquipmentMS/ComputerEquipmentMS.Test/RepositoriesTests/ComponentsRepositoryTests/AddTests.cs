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
                    ComponentCategory = new ComponentCategory { Id = 2, Name = "processors" },
                    ComponentManufacturer = new ComponentManufacturer { Id = 2, Name = "intel" },
                    Name = "new component",
                    Specifications = new ComponentSpecifications { ["spec"] = "value" },
                    PurchaseDate = Instant.FromUtc(2023, 06, 01, 12, 00, 00),
                    Cost = 15000,
                    WarrantyPeriod = Period.FromYears(1),
                },
                new Component
                {
                    Id = 1,
                    ComponentCategory = new ComponentCategory { Id = 2, Name = "processors" },
                    ComponentManufacturer = new ComponentManufacturer { Id = 2, Name = "intel" },
                    Name = "new component",
                    Specifications = new ComponentSpecifications { ["spec"] = "value" },
                    PurchaseDate = Instant.FromUtc(2023, 06, 01, 12, 00, 00),
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
                    ComponentCategory = new ComponentCategory { Id = 2, Name = "processors" },
                    ComponentManufacturer = new ComponentManufacturer { Id = 2, Name = "intel" },
                    Name = "new component",
                    Specifications = new ComponentSpecifications { ["spec"] = "value" },
                    PurchaseDate = Instant.FromUtc(2023, 06, 01, 12, 00, 00),
                    Cost = 15000,
                    WarrantyPeriod = Period.FromYears(1),
                },
                new[]
                {
                    new Component
                    {
                        Id = 1,
                        ComponentCategory = new ComponentCategory { Id = 1, Name = "video cards" },
                        ComponentManufacturer = new ComponentManufacturer { Id = 1, Name = "nvidia" },
                        Name = "rtx 4090",
                        Specifications = new ComponentSpecifications { ["fans"] = "3", ["mass"] = "2 kg" },
                        PurchaseDate = Instant.FromUtc(2019, 09, 10, 12, 00, 00),
                        Cost = 90000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 2,
                        ComponentCategory = new ComponentCategory { Id = 2, Name = "processors" },
                        ComponentManufacturer = new ComponentManufacturer { Id = 2, Name = "intel" },
                        Name = "intel core i7 12700",
                        Specifications = new ComponentSpecifications { ["power"] = "100 wt", ["mass"] = "50 g" },
                        PurchaseDate = Instant.FromUtc(2019, 10, 15, 12, 00, 00),
                        Cost = 30000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 3,
                        ComponentCategory = new ComponentCategory { Id = 3, Name = "hard drives" },
                        ComponentManufacturer = new ComponentManufacturer { Id = 3, Name = "samsung" },
                        Name = "samsung 980 evo ssd",
                        Specifications = new ComponentSpecifications { ["capacity"] = "980 gb", ["mass"] = "50 g", ["reading speed"] = "3500 mb/sec" },
                        PurchaseDate = Instant.FromUtc(2019, 12, 25, 12, 00, 00),
                        Cost = 15000,
                        WarrantyPeriod = Period.FromYears(5),
                    },
                    new Component
                    {
                        Id = 4,
                        ComponentCategory = new ComponentCategory { Id = 2, Name = "processors" },
                        ComponentManufacturer = new ComponentManufacturer { Id = 2, Name = "intel" },
                        Name = "new component",
                        Specifications = new ComponentSpecifications { ["spec"] = "value" },
                        PurchaseDate = Instant.FromUtc(2023, 06, 01, 12, 00, 00),
                        Cost = 15000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                }
            }
        };
}