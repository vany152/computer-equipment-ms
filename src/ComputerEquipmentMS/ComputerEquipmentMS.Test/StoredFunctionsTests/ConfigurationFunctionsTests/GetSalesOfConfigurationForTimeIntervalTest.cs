using ComputerEquipmentMS.Models.Auxiliary;
using NodaTime;

namespace ComputerEquipmentMS.Test.StoredFunctionsTests.ConfigurationFunctionsTests;

public class GetSalesOfConfigurationForTimeIntervalTest : ConfigurationFunctionsTestBase
{
    [Theory]
    [MemberData(nameof(IdsOfConfigurationsWereSold))]
    public void GetSalesOfConfigurationsWereSoldShouldReturnCorrectResult(
        int configurationId, 
        TimeInterval timeInterval, 
        ICollection<int> expectedSaleIds) 
    {
        var salePositions = Executor.GetSalesOfConfigurationForTimeInterval(configurationId, timeInterval);
        var salePositionIds = salePositions.Select(sp => sp.SalePositionId);
        salePositionIds.Should().BeEquivalentTo(expectedSaleIds);
    }
    
    public static IEnumerable<object[]> IdsOfConfigurationsWereSold =>
        new List<object[]>
        {
            new object[]
            {
                1,
                new TimeInterval(
                    Instant.FromUtc(2020, 01, 02, 12, 00),
                    Instant.FromUtc(2021, 05, 30, 12, 00)),
                new [] {1, 2}
            },
            new object[]
            {
                1, 
                new TimeInterval(
                    Instant.FromUtc(2022, 05, 30, 12, 00),
                    Instant.FromUtc(2022, 05, 30, 12, 00)),
                new [] {4}
            },
            new object[]
            {
                2, 
                new TimeInterval(
                    Instant.FromUtc(2020, 01, 02, 12, 00),
                    Instant.FromUtc(2020, 01, 02, 12, 00)),
                new [] {3}
            },
            new object[]
            {
                2, 
                new TimeInterval(
                    Instant.FromUtc(2023, 01, 01, 12, 00),
                    Instant.FromUtc(2024, 01, 01, 12, 00)),
                Array.Empty<int>()
            },
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