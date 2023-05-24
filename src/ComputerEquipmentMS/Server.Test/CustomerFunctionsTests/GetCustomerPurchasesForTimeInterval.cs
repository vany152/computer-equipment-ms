using NodaTime;
using Server.Models.Auxiliary;

namespace Server.Test.CustomerFunctionsTests;

public class GetCustomerPurchasesForTimeInterval : CustomerFunctionsTestBase
{
    [Theory]
    [MemberData(nameof(ExistingCustomerIdsWithPurchases))]
    public void GetPurchasesOfExistingCustomerWithPurchasesForTimeIntervalShouldReturnCorrectSalesIds(
        int customerId, 
        TimeInterval timeInterval,
        ICollection<int> expectedSaleIds) 
    {
        var sales = Executor.GetCustomersPurchases(customerId, timeInterval);
        var saleIds = sales.Select(customer => customer.Id); 
        saleIds.Should().BeEquivalentTo(expectedSaleIds);
    }
    
    public static IEnumerable<object[]> ExistingCustomerIdsWithPurchases =>
        new List<object[]>
        {
            new object[]
            {
                1,
                new TimeInterval(
                    Instant.FromUtc(2021, 05, 30, 12, 00),
                    Instant.FromUtc(2022, 05, 30, 12, 00)),
                new [] {1, 3}
            },
            new object[]
            {
                1,
                new TimeInterval(
                    Instant.FromUtc(2022, 04, 30, 12, 00),
                    Instant.FromUtc(2022, 06, 30, 12, 00)),
                new [] {3}
            },
            new object[]
            {
                2,
                new TimeInterval(
                    Instant.FromUtc(2019, 10, 10, 12, 00),
                    Instant.FromUtc(2022, 11, 30, 12, 00)),
                new [] {2}
            },
            new object[]
            {
                2,
                new TimeInterval(
                    Instant.FromUtc(1999, 10, 10, 12, 00),
                    Instant.FromUtc(2010, 11, 30, 12, 00)),
                Array.Empty<int>()
            },
        };

    [Theory]
    [MemberData(nameof(ExistingCustomerIdsWithNoPurchases))]
    public void GetPurchasesOfExistingCustomerWithNoPurchasesShouldReturnEmptyCollection(
        int customerId, 
        TimeInterval timeInterval) 
    {
        var sales = Executor.GetCustomersPurchases(customerId, timeInterval);
        sales.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> ExistingCustomerIdsWithNoPurchases =>
        new List<object[]>
        {
            new object[]
            {
                3,
                new TimeInterval(
                    Instant.FromUtc(2022, 04, 30, 12, 00),
                    Instant.FromUtc(2022, 06, 30, 12, 00)),
            },
        };
    
    [Theory]
    [MemberData(nameof(NonExistingCustomerIds))]
    public void GetPurchasesOfNonExistingCustomerShouldReturnEmptyCollection(int customerId, TimeInterval timeInterval) 
    {
        var sales = Executor.GetCustomersPurchases(customerId);
        sales.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> NonExistingCustomerIds =>
        new List<object[]>
        {
            new object[]
            {
                45,
                new TimeInterval(
                    Instant.FromUtc(2022, 04, 30, 12, 00),
                    Instant.FromUtc(2022, 06, 30, 12, 00)),
            },
            new object[] {
                54,
                new TimeInterval(
                    Instant.FromUtc(2022, 04, 30, 12, 00),
                    Instant.FromUtc(2022, 06, 30, 12, 00)),
            },
        };
}