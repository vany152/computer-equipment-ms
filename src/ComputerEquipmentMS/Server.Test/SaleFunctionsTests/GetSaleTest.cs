using NodaTime;
using Server.Models.Domain;

namespace Server.Test.SaleFunctionsTests;

public class GetSaleTest : SaleFunctionTestBase
{
    [Theory]
    [MemberData(nameof(SuccessTestData))]
    public void GetSaleWithExistingIdShouldReturnSale(int saleId, Sale expectedSale)
    {
        var sale = Executor.GetSale(saleId);
        sale.Should().BeEquivalentTo(expectedSale);
    }

    public static IEnumerable<object?[]> SuccessTestData =>
        new List<object?[]>
        {
            new object?[] 
            {
                1, 
                new Sale
                {
                    Id = 1,
                    SaleMoment = Instant.FromUtc(2021, 05, 30, 12, 00),
                    CustomerId = 1,
                    DiscountPercentage = 0,
                    SalePositionIds = new []{1}
                }
            },
            new object?[] 
            {
                2, 
                new Sale
                {
                    Id = 2,
                    SaleMoment = Instant.FromUtc(2020, 01, 02, 12, 00),
                    CustomerId = 2,
                    DiscountPercentage = 5,
                    SalePositionIds = new []{2, 3}
                }
            },
            new object?[] 
            {
                3, 
                new Sale
                {
                    Id = 3,
                    SaleMoment = Instant.FromUtc(2022, 05, 30, 12, 00),
                    CustomerId = 1,
                    DiscountPercentage = 10,
                    SalePositionIds = new []{4, 5, 6} 
                }
            }
        };
    
    

    [Theory]
    [MemberData(nameof(FailureTestData))]
    public void GetSaleWithNonExistingIdShouldReturnNull(int saleId)
    {
        var sale = Executor.GetSale(saleId);
        sale.Should().BeNull();
    }
    
    public static IEnumerable<object[]> FailureTestData =>
        new List<object[]>
        {
            new object []{5},
            new object []{6},
        };
}