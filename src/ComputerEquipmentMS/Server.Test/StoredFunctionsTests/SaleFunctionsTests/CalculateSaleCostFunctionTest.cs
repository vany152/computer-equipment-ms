namespace Server.Test.StoredFunctionsTests.SaleFunctionsTests;

public class CalculateSaleCostFunctionTest : SaleFunctionTestBase
{
    [Theory]
    [MemberData(nameof(TestData))]
    public void SaleCostShouldBeCorrect(int saleId, decimal expectedCost)
    {
        var result = Executor.CalculateSaleCost(saleId);
        result.Should().Be(expectedCost);
    }

    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { 1, 121_000m },
            new object[] { 2, 216_125m },
            new object[] { 3, 280_777.5m },
            new object[] { 4, -1 }, // sale with id = 4 does not exist
        };
}