namespace ComputerEquipmentMS.Test.StoredFunctionsTests.ConfigurationFunctionsTests;

public class CalculateConfigurationCostTest : ConfigurationFunctionsTestBase
{
    [Theory]
    [MemberData(nameof(IdsOfExistingConfigurations))]
    public void CostOfExistingConfigurationShouldBePositiveNumber(int configurationId, decimal expectedCost) 
    {
        var cost = Executor.CalculateConfigurationCost(configurationId);
        cost.Should().Be(expectedCost);
    }
    
    public static IEnumerable<object[]> IdsOfExistingConfigurations =>
        new List<object[]>
        {
            new object[] { 1, 121_000m },
            new object[] { 2, 106_500m },
        };
    
    
    
    [Theory]
    [MemberData(nameof(IdsOfNonExistingConfigurations))]
    public void CostOfNonExistingConfigurationShouldBeMinusOne(int configurationId) 
    {
        var cost = Executor.CalculateConfigurationCost(configurationId);
        cost.Should().Be(-1);
    }
    
    public static IEnumerable<object[]> IdsOfNonExistingConfigurations =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}