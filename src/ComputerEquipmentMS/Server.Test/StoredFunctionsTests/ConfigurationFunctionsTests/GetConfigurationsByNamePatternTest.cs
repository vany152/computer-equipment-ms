using System.Text.RegularExpressions;

namespace Server.Test.StoredFunctionsTests.ConfigurationFunctionsTests;

public class GetConfigurationsByNamePatternTest : ConfigurationFunctionsTestBase
{
    [Theory]
    [MemberData(nameof(NamePatternsForExistingConfigurations))]
    public void GetConfigurationsByExistingNamePatternShouldReturnCorrectConfigurationIds(
        Regex namePattern, 
        ICollection<int> expectedConfigurationIds) 
    {
        var configurations = Executor.GetConfigurationsByNamePattern(namePattern);
        var configurationIds = configurations.Select(config => config.Id); 
        configurationIds.Should().BeEquivalentTo(expectedConfigurationIds);
    }
    
    public static IEnumerable<object[]> NamePatternsForExistingConfigurations =>
        new List<object[]>
        {
            new object[] { new Regex(@"config.*"), new [] {1, 2, 3} },
            new object[] { new Regex(@".*\d"), new [] {1, 2, 3} },
            new object[] { new Regex(@"1"), new [] {1} },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NamePatternsForNotExistingConfigurations))]
    public void GetConfigurationsByNotExistingNamePatternShouldReturnEmptyCollection(Regex namePattern) 
    {
        var configurations = Executor.GetConfigurationsByNamePattern(namePattern);
        var configurationIds = configurations.Select(config => config.Id); 
        configurationIds.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> NamePatternsForNotExistingConfigurations =>
        new List<object[]>
        {
            new object[] { new Regex(@"qwerty") },
            new object[] { new Regex(@"123") }
        };
}