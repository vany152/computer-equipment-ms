using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentManufacturersRepositoryTests;

public class AddTests : ComponentManufacturersRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(NewManufacturers))]
    public void AddedManufacturerEqualExpected(ComponentManufacturer newManufacturer, ComponentManufacturer expectedAddedManufacturer)
    {
        var addedManufacturer = Repository.Add(newManufacturer);
        addedManufacturer.Should().BeEquivalentTo(expectedAddedManufacturer);
    }
    
    public static IEnumerable<object[]> NewManufacturers =>
        new List<object[]>
        {
            new object[]
            {
                new ComponentManufacturer
                {
                    Name = "new manufacturer",
                },
                new ComponentManufacturer
                {
                    Id = 1,
                    Name = "new manufacturer"
                }
            }
        };



    [Theory]
    [MemberData(nameof(Manufacturers))]
    public void AddNewManufacturerDoesNotAffectOtherManufacturers(ComponentManufacturer newManufacturer, ICollection<ComponentManufacturer> expectedManufacturers)
    {
        FillDbWithTestData();
        
        Repository.Add(newManufacturer);
        var manufacturers = Repository.GetAll();
        manufacturers.Should().BeEquivalentTo(expectedManufacturers);
    }

    public static IEnumerable<object[]> Manufacturers =>
        new List<object[]>
        {
            new object[]
            {
                new ComponentManufacturer
                {
                    Name = "new manufacturer",
                },
                new[]
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
                        Name = "samsung",
                    },
                    new ComponentManufacturer
                    {
                        Id = 4,
                        Name = "new manufacturer"
                    }
                }
            }
        };
}