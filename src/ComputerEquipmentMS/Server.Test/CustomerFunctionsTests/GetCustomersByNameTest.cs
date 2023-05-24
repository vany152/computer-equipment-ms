using System.Text.RegularExpressions;

namespace Server.Test.CustomerFunctionsTests;

public class GetCustomersByNameTest : CustomerFunctionsTestBase
{
    [Theory]
    [MemberData(nameof(NamePatternsForExistingCustomers))]
    public void GetCustomersByExistingNamePatternShouldReturnCorrectCustomersIds(
        Regex namePattern, 
        ICollection<int> expectedCustomerIds) 
    {
        var customers = Executor.GetCustomersByNamePattern(namePattern);
        var customerIds = customers.Select(customer => customer.Id); 
        customerIds.Should().BeEquivalentTo(expectedCustomerIds);
    }
    
    public static IEnumerable<object[]> NamePatternsForExistingCustomers =>
        new List<object[]>
        {
            new object[] { new Regex(@".*"), new [] {1, 2, 3} },
            new object[] { new Regex(@"^\w{4}$"), new [] {1, 2} },
            new object[] { new Regex(@"A.*"), new [] {2, 3} },
            new object[] { new Regex(@"Alexander"), new [] {3} },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NamePatternsForNotExistingCustomers))]
    public void GetCustomersByNotExistingNamePatternShouldReturnEmptyCollection(Regex namePattern) 
    {
        var customers = Executor.GetCustomersByNamePattern(namePattern);
        customers.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> NamePatternsForNotExistingCustomers =>
        new List<object[]>
        {
            new object[] { new Regex(@"qwerty") },
            new object[] { new Regex(@"123") }
        };
}