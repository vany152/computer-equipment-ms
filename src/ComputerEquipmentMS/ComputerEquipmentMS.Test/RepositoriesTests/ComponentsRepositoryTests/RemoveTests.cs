﻿using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComponentsRepositoryTests;

public class RemoveTests : ComponentsRepositoryTestBase
{
     public RemoveTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingComponents))]
    public void RemoveExistingComponentShouldReturnTrue(int componentId)
    {
        var removalSuccessful = Repository.Remove(componentId);
        removalSuccessful.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> ExistingComponents =>
        new List<object[]>
        {
            new object[] { 1 },
            new object[] { 2 },
            new object[] { 3 },
        };
    
    

    [Theory]
    [MemberData(nameof(NonExistingComponents))]
    public void RemoveNonExistingComponentShouldReturnFalse(int componentId)
    {
        var removalSuccessful = Repository.Remove(componentId);
        removalSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingComponents =>
        new List<object[]>
        {
            new object[] { 15 },
            new object[] { 45 },
            new object[] { 54 },
        };
    
    
    
    [Theory]
    [MemberData(nameof(RemoveComponentShouldNotAffectOtherComponentsTestData))]
    public void RemoveExistingComponentShouldNotAffectOtherComponent(int componentId, ICollection<Component> expectedComponents)
    {
        var removalSuccessful = Repository.Remove(componentId);
        var components = Repository.GetAll();
        
        removalSuccessful.Should().BeTrue();
        components.Should().BeEquivalentTo(expectedComponents);
    }
    
    public static IEnumerable<object[]> RemoveComponentShouldNotAffectOtherComponentsTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                1,
                new []
                {
                    new Component
                    {
                        Id = 2,
                        ComponentCategory = 2,
                        ComponentManufacturer = 2,
                        Name = "intel core i7 12700",
                        Specifications = new ComponentSpecifications { ["power"] = "100 wt", ["mass"] = "50 g" },
                        Cost = 30000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 3,
                        ComponentCategory = 3,
                        ComponentManufacturer = 3,
                        Name = "samsung 980 evo ssd",
                        Specifications = new ComponentSpecifications { ["capacity"] = "980 gb", ["mass"] = "50 g", ["reading speed"] = "3500 mb/sec" },
                        Cost = 15000,
                        WarrantyPeriod = Period.FromYears(5),
                    },
                },
            },
            new object[] 
            { 
                2, 
                new []
                {
                    new Component
                    {
                        Id = 1,
                        ComponentCategory = 1,
                        ComponentManufacturer = 1,
                        Name = "rtx 4090",
                        Specifications = new ComponentSpecifications { ["fans"] = "3", ["mass"] = "2 kg" },
                        Cost = 90000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 3,
                        ComponentCategory = 3,
                        ComponentManufacturer = 3,
                        Name = "samsung 980 evo ssd",
                        Specifications = new ComponentSpecifications { ["capacity"] = "980 gb", ["mass"] = "50 g", ["reading speed"] = "3500 mb/sec" },
                        Cost = 15000,
                        WarrantyPeriod = Period.FromYears(5),
                    },
                }
            },
            new object[] 
            { 
                3, 
                new []
                {
                    new Component
                    {
                        Id = 1,
                        ComponentCategory = 1,
                        ComponentManufacturer = 1,
                        Name = "rtx 4090",
                        Specifications = new ComponentSpecifications { ["fans"] = "3", ["mass"] = "2 kg" },
                        Cost = 90000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new Component
                    {
                        Id = 2,
                        ComponentCategory = 2,
                        ComponentManufacturer = 2,
                        Name = "intel core i7 12700",
                        Specifications = new ComponentSpecifications { ["power"] = "100 wt", ["mass"] = "50 g" },
                        Cost = 30000,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                } 
            },
        };
}