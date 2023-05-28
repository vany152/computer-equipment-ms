using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.SalesRepositoryTests;

public class GetByCriteriaTests : SalesRepositoryTestBase
{
    public GetByCriteriaTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(GetSalesByIdData))]
    public void GetByIdShouldReturnCorrectSaleIds(int id, IEnumerable<int> expectedSaleIds)
    {
        var sales = Repository.GetByCriteria(customer => customer.Id == id);
        var saleIds = GetSaleIds(sales);
        saleIds.Should().BeEquivalentTo(expectedSaleIds);
    }
    
    public static IEnumerable<object[]> GetSalesByIdData =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
            new object[] { 45, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetSalesByCustomerId))]
    public void GetByCustomerIdShouldReturnCorrectSaleIds(int customerId, IEnumerable<int> expectedSaleIds)
    {
        var sales = Repository.GetByCriteria(sale => sale.CustomerId == customerId);
        var saleIds = GetSaleIds(sales);
        saleIds.Should().BeEquivalentTo(expectedSaleIds);    
    }
    
    public static IEnumerable<object[]> GetSalesByCustomerId =>
        new List<object[]>
        {
            new object[] { 1, new [] {1, 3} },
            new object[] { 2, new [] {2} },
            new object[] { 45, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetSalesBySaleMoment))]
    public void GetBySaleMomentShouldReturnCorrectSaleIds(Instant saleMoment, IEnumerable<int> expectedSaleIds)
    {
        var sales = Repository.GetByCriteria(sale => sale.SaleMoment == saleMoment);
        var saleIds = GetSaleIds(sales);
        saleIds.Should().BeEquivalentTo(expectedSaleIds);
    }
    
    public static IEnumerable<object[]> GetSalesBySaleMoment =>
        new List<object[]>
        {
            new object[] { Instant.FromUtc(2021, 05, 30, 12, 00), new [] {1} },
            new object[] { Instant.FromUtc(2020, 01, 02, 12, 00), new [] {2} },
            new object[] { Instant.FromUtc(2022, 05, 30, 12, 00), new [] {3} },
            new object[] { Instant.FromUtc(2000, 01, 01, 12, 00), Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetSalesByDiscountPercentage))]
    public void GetByDiscountPercentageShouldReturnCorrectSaleIds(short discountPercentage, IEnumerable<int> expectedSaleIds)
    {
        var sales = Repository.GetByCriteria(customer => customer.DiscountPercentage == discountPercentage);
        var saleIds = GetSaleIds(sales);
        saleIds.Should().BeEquivalentTo(expectedSaleIds);
    }
    
    public static IEnumerable<object[]> GetSalesByDiscountPercentage =>
        new List<object[]>
        {
            new object[] { 0, new [] {1} },
            new object[] { 5, new [] {2} },
            new object[] { 10, new [] {3} },
            new object[] { 15, Array.Empty<int>() },
        };
    
    
    
    private static IEnumerable<int> GetSaleIds(IEnumerable<Sale> sales) =>
        sales.Select(sale => sale.Id);
    
}