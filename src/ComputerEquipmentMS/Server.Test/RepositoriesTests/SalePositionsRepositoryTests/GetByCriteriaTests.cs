using NodaTime;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.SalePositionsRepositoryTests;

public class GetByCriteriaTests : SalePositionsRepositoryTestBase
{
    public GetByCriteriaTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(GetSalePositionsByIdData))]
    public void GetByIdShouldReturnCorrectSalePositionIds(int id, IEnumerable<int> expectedSalePositionIds)
    {
        var salePositions = Repository.GetByCriteria(sp => sp.Id == id);
        var salePositionIds = GetSalePositionIds(salePositions);
        salePositionIds.Should().BeEquivalentTo(expectedSalePositionIds);
    }
    
    public static IEnumerable<object[]> GetSalePositionsByIdData =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
            new object[] { 4, new [] {4} },
            new object[] { 5, new [] {5} },
            new object[] { 6, new [] {6} },
            new object[] { 45, Array.Empty<int>() },
            new object[] { 54, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetSalePositionsBySaleIdData))]
    public void GetBySaleIdShouldReturnCorrectSalePositionIds(int saleId, IEnumerable<int> expectedSalePositionIds)
    {
        var salePositions = Repository.GetByCriteria(salePosition => salePosition.SaleId == saleId);
        var salePositionIds = GetSalePositionIds(salePositions);
        salePositionIds.Should().BeEquivalentTo(expectedSalePositionIds);    
    }
    
    public static IEnumerable<object[]> GetSalePositionsBySaleIdData =>
        new List<object[]>
        {
            new object[] { 1, new[] { 1 } },
            new object[] { 2, new[] { 2, 3 } },
            new object[] { 3, new[] { 4, 5, 6 } },
            new object[] { 45, Array.Empty<int>() },
            new object[] { 54, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetSalePositionsByComputerConfigurationIdData))]
    public void GetByComputerConfigurationIdShouldReturnCorrectSalePositionIds(int configurationId, IEnumerable<int> expectedSalePositionIds)
    {
        var salePositions = Repository.GetByCriteria(sp => sp.ConfigurationId == configurationId);
        var salePositionIds = GetSalePositionIds(salePositions);
        salePositionIds.Should().BeEquivalentTo(expectedSalePositionIds);
    }
    
    public static IEnumerable<object[]> GetSalePositionsByComputerConfigurationIdData =>
        new List<object[]>
        {
            new object[] { 1, new[] { 1, 2, 4 } },
            new object[] { 2, new[] { 3, 5, 6 } },
            new object[] { 45, Array.Empty<int>() },
            new object[] { 54, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetSalePositionsByCostData))]
    public void GetByCostShouldReturnCorrectSalePositionIds(decimal cost, IEnumerable<int> expectedSalePositionIds)
    {
        var salePositions = Repository.GetByCriteria(sp => sp.Cost == cost);
        var salePositionIds = GetSalePositionIds(salePositions);
        salePositionIds.Should().BeEquivalentTo(expectedSalePositionIds);
    }
    
    public static IEnumerable<object[]> GetSalePositionsByCostData =>
        new List<object[]>
        {
            new object[] { 121_000, new[] { 1, 2, 4 } },
            new object[] { 106_500, new[] { 3, 5, 6 } },
            new object[] { 100, Array.Empty<int>() },
            new object[] { 1_000_000, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetSalePositionsByDiscountPercentageData))]
    public void GetByDiscountPercentageShouldReturnCorrectSalePositionIds(short discountPercentage, IEnumerable<int> expectedSalePositionIds)
    {
        var salePositions = Repository.GetByCriteria(sp => sp.DiscountPercentage == discountPercentage);
        var salePositionIds = GetSalePositionIds(salePositions);
        salePositionIds.Should().BeEquivalentTo(expectedSalePositionIds);
    }
    
    public static IEnumerable<object[]> GetSalePositionsByDiscountPercentageData =>
        new List<object[]>
        {
            new object[] { 0, new[] { 1, 2, 3, 5 } },
            new object[] { 5, new[] { 4 } },
            new object[] { 15, new[] { 6 } },
            new object[] { 50, Array.Empty<int>() },
            new object[] { 150, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetSalePositionsByWarrantyPeriodData))]
    public void GetByWarrantyPeriodShouldReturnCorrectSalePositionIds(Period warrantyPeriod, IEnumerable<int> expectedSalePositionIds)
    {
        var salePositions = Repository.GetByCriteria(sp => sp.WarrantyPeriod == warrantyPeriod);
        var salePositionIds = GetSalePositionIds(salePositions);
        salePositionIds.Should().BeEquivalentTo(expectedSalePositionIds);
    }
    
    public static IEnumerable<object[]> GetSalePositionsByWarrantyPeriodData =>
        new List<object[]>
        {
            new object[] { Period.FromYears(1), new[] { 1, 2, 4 } },
            new object[] { Period.FromYears(2), new[] { 3, 5, 6 } },
            new object[] { Period.FromYears(5), Array.Empty<int>() },
            new object[] { Period.FromDays(3), Array.Empty<int>() },
        };

    

    private static IEnumerable<int> GetSalePositionIds(IEnumerable<SalePosition> salePositions) =>
        salePositions.Select(salePosition => salePosition.Id);
    
}