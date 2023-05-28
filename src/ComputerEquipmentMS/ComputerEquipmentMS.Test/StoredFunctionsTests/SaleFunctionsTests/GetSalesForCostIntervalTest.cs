using ComputerEquipmentMS.Models.Auxiliary;

namespace ComputerEquipmentMS.Test.StoredFunctionsTests.SaleFunctionsTests;

public class GetSalesForCostIntervalTest : SaleFunctionTestBase
{
    [Theory]
    [MemberData(nameof(TestData))]
    public void SalesForCostIntervalShouldReturnCorrectSaleIds(CostInterval costInterval, ICollection<int> expectedSales)
    {
        var sales = Executor.GetSalesForCostInterval(costInterval);
        sales.Select(s => s.Id).Should().BeEquivalentTo(expectedSales);
    }

    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { new CostInterval(120_000, 130_000), new[] { 1 } },
            new object[] { new CostInterval(121_000, 121_000), new[] { 1 } },
            new object[] { new CostInterval(120_000, 220_000), new[] { 1, 2 } },
            new object[] { new CostInterval(130_000, 200_000), Array.Empty<int>() },
            new object[] { new CostInterval(0, 1), Array.Empty<int>() },
        };
}