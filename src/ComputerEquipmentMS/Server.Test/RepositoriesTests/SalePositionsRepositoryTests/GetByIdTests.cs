using NodaTime;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.SalePositionsRepositoryTests;

public class GetByIdTests : SalePositionsRepositoryTestBase
{
    public GetByIdTests()
    {
        base.FillDbWithTestData();
    }

    [Theory]
    [MemberData(nameof(ExistingIds))]
    public void GetSalePositionByExistingIdShouldReturnCorrectSalePosition(int salePositionId, SalePosition expectedSalePosition)
    {
        var salePosition = Repository.GetById(salePositionId);
        salePosition.Should().BeEquivalentTo(expectedSalePosition);
    }
    
    public static IEnumerable<object[]> ExistingIds =>
        new List<object[]>
        {
            new object[] 
            { 
                1, new SalePosition
                {
                    Id = 1,
                    SaleId = 1,
                    ConfigurationId = 1,
                    Cost = 121_000,
                    DiscountPercentage = 0,
                    WarrantyPeriod = Period.FromYears(1),
                } 
            },
            new object[] 
            { 
                2, new SalePosition
                {
                    Id = 2,
                    SaleId = 2,
                    ConfigurationId = 1,
                    Cost = 121_000,
                    DiscountPercentage = 0,
                    WarrantyPeriod = Period.FromYears(1),
                } 
            },
            new object[] 
            { 
                3, new SalePosition
                {
                    Id = 3,
                    SaleId = 2,
                    ConfigurationId = 2,
                    Cost = 106_500,
                    DiscountPercentage = 0,
                    WarrantyPeriod = Period.FromYears(2),
                } 
            },
            new object[] 
            { 
                4, new SalePosition
                {
                    Id = 4,
                    SaleId = 3,
                    ConfigurationId = 1,
                    Cost = 121_000,
                    DiscountPercentage = 5,
                    WarrantyPeriod = Period.FromYears(1),
                } 
            },
            new object[] 
            { 
                5, new SalePosition
                {
                    Id = 5,
                    SaleId = 3,
                    ConfigurationId = 2,
                    Cost = 106_500,
                    DiscountPercentage = 0,
                    WarrantyPeriod = Period.FromYears(2),
                } 
            },
            new object[] 
            { 
                6, new SalePosition
                {
                    Id = 6,
                    SaleId = 3,
                    ConfigurationId = 2,
                    Cost = 106_500,
                    DiscountPercentage = 15,
                    WarrantyPeriod = Period.FromYears(2),
                } 
            },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingIds))]
    public void GetSalePositionByNonExistingIdShouldReturnNull(int salePositionId)
    {
        var salePosition = Repository.GetById(salePositionId);
        salePosition.Should().BeNull();
    }

    public static IEnumerable<object[]> NonExistingIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}