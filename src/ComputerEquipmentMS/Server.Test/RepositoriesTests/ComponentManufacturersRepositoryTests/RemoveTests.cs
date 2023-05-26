using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentManufacturersRepositoryTests;

public class RemoveTests : ComponentManufacturersRepositoryTestBase
{
     public RemoveTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingManufacturers))]
    public void RemoveExistingManufacturerShouldReturnTrue(int manufacturerId)
    {
        var removalSuccessful = Repository.Remove(manufacturerId);
        removalSuccessful.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> ExistingManufacturers =>
        new List<object[]>
        {
            new object[] { 1 },
            new object[] { 2 },
            new object[] { 3 },
        };
    
    

    [Theory]
    [MemberData(nameof(NonExistingManufacturers))]
    public void RemoveNonExistingManufacturerShouldReturnFalse(int manufacturerId)
    {
        var removalSuccessful = Repository.Remove(manufacturerId);
        removalSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingManufacturers =>
        new List<object[]>
        {
            new object[] { 15 },
            new object[] { 45 },
            new object[] { 54 },
        };
    
    
    
    [Theory]
    [MemberData(nameof(RemoveManufacturerShouldNotAffectOtherManufacturersTestData))]
    public void RemoveExistingManufacturerShouldNotAffectOtherManufacturer(int manufacturerId, ICollection<ComponentManufacturer> expectedManufacturers)
    {
        var removalSuccessful = Repository.Remove(manufacturerId);
        var manufacturers = Repository.GetAll();
        
        removalSuccessful.Should().BeTrue();
        manufacturers.Should().BeEquivalentTo(expectedManufacturers);
    }
    
    public static IEnumerable<object[]> RemoveManufacturerShouldNotAffectOtherManufacturersTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                1,
                new []
                {
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
                2, 
                new []
                {
                    new ComponentManufacturer
                    {
                        Id = 1,
                        Name = "nvidia",
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
                3, 
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
                } 
            },
        };
}