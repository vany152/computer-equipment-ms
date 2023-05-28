namespace ComputerEquipmentMS.Test.StoredFunctionsTests.ConfigurationFunctionsTests;

public class GetSalesOfConfigurationTest : ConfigurationFunctionsTestBase
{
    [Theory]
    [MemberData(nameof(IdsOfConfigurationsWereSold))]
    public void GetSalesOfConfigurationsWereSoldShouldReturnNotEmptyArray(int configurationId, ICollection<int> expectedSaleIds) 
    {
        var salePositions = Executor.GetSalesOfConfiguration(configurationId);
        var salePositionIds = salePositions.Select(sp => sp.SalePositionId);
        salePositionIds.Should().BeEquivalentTo(expectedSaleIds);
    }
    
    public static IEnumerable<object[]> IdsOfConfigurationsWereSold =>
        new List<object[]>
        {
            new object[] { 1, new [] {1, 2, 4} },
            new object[] { 2, new [] {3, 5, 6} },
        };
    
    
    
    [Theory]
    [MemberData(nameof(IdsOfConfigurationsWereNotSold))]
    public void GetSalesOfConfigurationsWereNotSoldShouldReturnEmptyList(int configurationId) 
    {
        var saleIds = Executor.GetSalesOfConfiguration(configurationId);
        saleIds.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> IdsOfConfigurationsWereNotSold =>
        new List<object[]>
        {
            new object[] { 3 },
        };
    
    
    
    [Theory]
    [MemberData(nameof(IdsOfNonExistingConfigurations))]
    public void GetSalesOfNonExistingConfigurationsShouldReturnEmptyList(int configurationId) 
    {
        var saleIds = Executor.GetSalesOfConfiguration(configurationId);
        saleIds.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> IdsOfNonExistingConfigurations =>
        new List<object[]>
        {
            new object[] { 45 },
        };
}