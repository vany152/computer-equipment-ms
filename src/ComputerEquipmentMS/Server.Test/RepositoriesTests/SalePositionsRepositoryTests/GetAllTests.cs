using NodaTime;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.SalePositionsRepositoryTests;

public class GetAllTests : SalePositionsRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(SalePositions))]
    public void GetAllSalePositionsOfNonEmptyRepositoryShouldReturnCorrectSalePositionsCollection(ICollection<SalePosition> expectedSalePositions)
    {
        FillDbWithTestData();

        var salePositions = Repository.GetAll();
        salePositions.Should().BeEquivalentTo(expectedSalePositions);
    }

    public static IEnumerable<object[]> SalePositions =>
        new List<object[]>
        {
            new object[]
            {
                new[]
                {
                    new SalePosition
                    {
                        Id = 1,
                        SaleId = 1,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 2,
                        SaleId = 2,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 3,
                        SaleId = 2,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                    new SalePosition
                    {
                        Id = 4,
                        SaleId = 3,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 5,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 5,
                        SaleId = 3,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                    new SalePosition
                    {
                        Id = 6,
                        SaleId = 3,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 15,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                }
            }
        };



    [Fact]
    public void GetAllSalePositionsOfEmptyRepositoryShouldReturnEmptyCollection()
    {
        var salePositions = Repository.GetAll();
        salePositions.Should().BeEmpty();
    }
}