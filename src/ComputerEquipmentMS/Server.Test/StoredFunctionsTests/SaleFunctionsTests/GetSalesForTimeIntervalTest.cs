using NodaTime;
using Server.Models.Auxiliary;

namespace Server.Test.StoredFunctionsTests.SaleFunctionsTests;

public class GetSalesForTimeIntervalTest : SaleFunctionTestBase
{
    [Theory]
    [MemberData(nameof(TestData))]
    public void SalesForTimeIntervalShouldReturnCorrectSaleIds(TimeInterval timeInterval, ICollection<int> expectedSales)
    {
        var sales = Executor.GetSalesForTimeInterval(timeInterval);
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
                new [] {1, 2, 3} 
            },
            new object[] 
            {
                new TimeInterval(
                    Instant.FromUtc(2020, 01, 02, 12, 00),
                    Instant.FromUtc(2021, 05, 30, 12, 00)), 
                new [] {1, 2} 
            },
            new object[] 
            {
                new TimeInterval(
                    Instant.FromUtc(2022, 05, 30, 12, 00),
                    Instant.FromUtc(2022, 05, 30, 12, 00)), 
                new [] {3} 
            },
            new object[] 
            {
                new TimeInterval(
                    Instant.FromUtc(2023, 01, 1, 12, 00),
                    Instant.FromUtc(2023, 01, 01, 12, 00)),
                Array.Empty<int>()
            }
        };
}