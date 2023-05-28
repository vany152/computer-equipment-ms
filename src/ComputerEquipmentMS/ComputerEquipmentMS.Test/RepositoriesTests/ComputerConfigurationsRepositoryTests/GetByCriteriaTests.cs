using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.ComputerConfigurationsRepositoryTests;

public class GetByCriteriaTests : ComputerConfigurationsRepositoryTestBase
{
    public GetByCriteriaTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(GetConfigurationsByIdData))]
    public void GetByIdShouldReturnCorrectConfigurationIds(int id, IEnumerable<int> expectedConfigurationIds)
    {
        var configuration = Repository.GetByCriteria(configuration => configuration.Id == id);
        var configurationIds = GetConfigurationIds(configuration);
        configurationIds.Should().BeEquivalentTo(expectedConfigurationIds);
    }
    
    public static IEnumerable<object[]> GetConfigurationsByIdData =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 45, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetConfigurationsByNameData))]
    public void GetByNameShouldReturnCorrectConfigurationIds(string name, IEnumerable<int> expectedConfigurationIds)
    {
        var configuration = Repository.GetByCriteria(configuration => configuration.Name == name);
        var configurationIds = GetConfigurationIds(configuration);
        configurationIds.Should().BeEquivalentTo(expectedConfigurationIds);    
    }
    
    public static IEnumerable<object[]> GetConfigurationsByNameData =>
        new List<object[]>
        {
            new object[] { "configuration 1", new [] {1} },
            new object[] { "configuration 2", new [] {2} },
            new object[] { "computer 1", Array.Empty<int>() },
            new object[] { "computer 2", Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetSalePositionsByWarrantyPeriodData))]
    public void GetByWarrantyPeriodShouldReturnCorrectConfigurationIds(Period warrantyPeriod, IEnumerable<int> expectedConfigurationIds)
    {
        var configurations = Repository.GetByCriteria(sp => sp.WarrantyPeriod == warrantyPeriod);
        var configurationIds = GetConfigurationIds(configurations);
        configurationIds.Should().BeEquivalentTo(expectedConfigurationIds);
    }
    
    public static IEnumerable<object[]> GetSalePositionsByWarrantyPeriodData =>
        new List<object[]>
        {
            new object[] { Period.FromYears(1), new[] { 1, } },
            new object[] { Period.FromYears(2), new[] { 2, } },
            new object[] { Period.FromYears(5), Array.Empty<int>() },
            new object[] { Period.FromDays(3), Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetConfigurationsByMarginData))]
    public void GetByMarginShouldReturnCorrectConfigurationIds(decimal margin, IEnumerable<int> expectedConfigurationIds)
    {
        var configuration = Repository.GetByCriteria(configuration => configuration.Margin == margin);
        var configurationIds = GetConfigurationIds(configuration);
        configurationIds.Should().BeEquivalentTo(expectedConfigurationIds);
    }
    
    public static IEnumerable<object[]> GetConfigurationsByMarginData =>
        new List<object[]>
        {
            new object[] { 1000, new [] {1} },
            new object[] { 1500, new [] {2} },
            new object[] { 5000, Array.Empty<int>() },
            new object[] { 33000, Array.Empty<int>() },
        };

    

    private static IEnumerable<int> GetConfigurationIds(IEnumerable<ComputerConfiguration> configurations) =>
        configurations.Select(configuration => configuration.Id);
    
}