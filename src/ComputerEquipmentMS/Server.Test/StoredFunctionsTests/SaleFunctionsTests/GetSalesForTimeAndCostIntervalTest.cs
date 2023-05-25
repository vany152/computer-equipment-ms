using NodaTime;
using Server.Models.Auxiliary;

namespace Server.Test.StoredFunctionsTests.SaleFunctionsTests;

public class GetSalesForTimeAndCostIntervalTest : SaleFunctionTestBase
{
    [Theory]
    [MemberData(nameof(TestData))]
    public void SalesForTimeAndCostIntervalShouldReturnCorrectSaleIds(TimeInterval timeInterval, CostInterval costInterval, ICollection<int> expectedSales)
    {
        var sales = Executor.GetSalesForTimeAndCostInterval(timeInterval, costInterval);
        sales.Select(s => s.Id).Should().BeEquivalentTo(expectedSales);
    }
    
    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] 
            {
                new TimeInterval(
                    Instant.FromUtc(2020, 01, 01, 12, 00),
                    Instant.FromUtc(2023, 01, 01, 12, 00)), 
                new CostInterval(0, 500_000),
                new [] {1, 2, 3} 
            },
            new object[] 
            {
                new TimeInterval(
                    Instant.FromUtc(2020, 01, 02, 12, 00),
                    Instant.FromUtc(2021, 05, 30, 12, 00)), 
                new CostInterval(120_000, 220_000),
                new [] {1, 2} 
            },
            new object[] 
            {
                new TimeInterval(
                    Instant.FromUtc(2022, 05, 30, 12, 00),
                    Instant.FromUtc(2022, 05, 30, 12, 00)), 
                new CostInterval(200_000, 200_000),
                Array.Empty<int>() 
            },
            new object[] 
            {
                new TimeInterval(
                    Instant.FromUtc(2020, 01, 01, 12, 00),
                    Instant.FromUtc(2023, 01, 01, 12, 00)), 
                new CostInterval(200_000, 300_000),
                new [] {2, 3} 
            }
        };
}