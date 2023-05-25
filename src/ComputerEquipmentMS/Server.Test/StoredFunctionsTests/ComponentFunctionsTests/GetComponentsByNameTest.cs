using System.Text.RegularExpressions;

namespace Server.Test.StoredFunctionsTests.ComponentFunctionsTests;

public class GetComponentsByNameTest : ComponentFunctionsTest
{
    [Theory]
    [MemberData(nameof(NamePatternsForExistingComponents))]
    public void GetComponentsByExistingNamePatternShouldReturnCorrectComponentsIds(
        Regex namePattern, 
        ICollection<int> expectedComponentIds) 
    {
        var components = Executor.GetComponentsByName(namePattern);
        var componentIds = components.Select(component => component.Id); 
        componentIds.Should().BeEquivalentTo(expectedComponentIds);
    }
    
    public static IEnumerable<object[]> NamePatternsForExistingComponents =>
        new List<object[]>
        {
            new object[] { new Regex(@"^\w+\s*\d+"), new [] {1, 3} },
            new object[] { new Regex(@"\d{4,}"), new [] {1, 2} },
            new object[] { new Regex(@"rtx"), new [] {1} },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NamePatternsForNotExistingComponents))]
    public void GetComponentsByNotExistingNamePatternShouldReturnEmptyCollection(Regex namePattern) 
    {
        var components = Executor.GetComponentsByName(namePattern);
        components.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> NamePatternsForNotExistingComponents =>
        new List<object[]>
        {
            new object[] { new Regex(@"qwerty") },
            new object[] { new Regex(@"123") }
        };
}