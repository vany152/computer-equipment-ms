using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.StoredFunctionsTests.SaleFunctionsTests;

public class GetSalesTest : SaleFunctionTestBase
{
    [Theory]
    [MemberData(nameof(SuccessTestData))]
    public void GetSalesShouldReturnAllSales(ICollection<Sale> expectedSales)
    {
        var sales = Executor.GetSales();
        sales.Should().BeEquivalentTo(expectedSales);
    }

    public static IEnumerable<object[]> SuccessTestData =>
        new List<object[]>
        {
            new object[] 
            {
                new []
                {
                    new Sale
                    {
                        Id = 1,
                        SaleMoment = Instant.FromUtc(2021, 05, 30, 12, 00),
                        CustomerId = 1,
                        DiscountPercentage = 0,
                        SalePositions = new[] { 1 }
                    },
                    new Sale
                    {
                        Id = 2,
                        SaleMoment = Instant.FromUtc(2020, 01, 02, 12, 00),
                        CustomerId = 2,
                        DiscountPercentage = 5,
                        SalePositions = new[] { 2, 3 }
                    },
                    new Sale
                    {
                        Id = 3,
                        SaleMoment = Instant.FromUtc(2022, 05, 30, 12, 00),
                        CustomerId = 1,
                        DiscountPercentage = 10,
                        SalePositions = new[] { 4, 5, 6 }
                    }
                }
            }
        };
}