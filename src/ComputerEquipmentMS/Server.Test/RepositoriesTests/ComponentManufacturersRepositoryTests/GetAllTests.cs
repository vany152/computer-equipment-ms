using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.ComponentManufacturersRepositoryTests;

public class GetAllTests : ComponentManufacturersRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(ComponentManufacturers))]
    public void GetAllComponentManufacturersOfNonEmptyRepositoryShouldReturnCorrectComponentManufacturersCollection(ICollection<ComponentCategory> expectedComponentManufacturers)
    {
        FillDbWithTestData();

        var componentManufacturers = Repository.GetAll();
        componentManufacturers.Should().BeEquivalentTo(expectedComponentManufacturers);
    }

    public static IEnumerable<object[]> ComponentManufacturers =>
        new List<object[]>
        {
            new object[]
            {
                new[]
                {
                    new ComponentCategory
                    {
                        Id = 1,
                        Name = "nvidia",
                    },
                    new ComponentCategory
                    {
                        Id = 2,
                        Name = "intel",
                    },
                    new ComponentCategory
                    {
                        Id = 3,
                        Name = "samsung",
                    },
                }
            }
        };



    [Fact]
    public void GetAllComponentManufacturersOfEmptyRepositoryShouldReturnEmptyCollection()
    {
        var componentManufacturers = Repository.GetAll();
        componentManufacturers.Should().BeEmpty();
    }
}