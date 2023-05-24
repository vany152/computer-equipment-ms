namespace Server.Test.CustomerFunctionsTests;

public class GetCustomersPurchasesTest : CustomerFunctionsTestBase
{
    [Theory]
    [MemberData(nameof(ExistingCustomerIdsWithPurchases))]
    public void GetPurchasesOfExistingCustomerWithPurchasesShouldReturnCorrectSalesIds(
        int customerId, 
        ICollection<int> expectedSaleIds) 
    {
        var sales = Executor.GetCustomersPurchases(customerId);
        var saleIds = sales.Select(customer => customer.Id); 
        saleIds.Should().BeEquivalentTo(expectedSaleIds);
    }
    
    public static IEnumerable<object[]> ExistingCustomerIdsWithPurchases =>
        new List<object[]>
        {
            new object[] { 1, new [] {1, 3} },
            new object[] { 2, new [] {2} },
        };
    
    [Theory]
    [MemberData(nameof(ExistingCustomerIdsWithNoPurchases))]
    public void GetPurchasesOfExistingCustomerWithNoPurchasesShouldReturnEmptyCollection(int customerId) 
    {
        var sales = Executor.GetCustomersPurchases(customerId);
        sales.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> ExistingCustomerIdsWithNoPurchases =>
        new List<object[]>
        {
            new object[] { 3 },
        };
    
    [Theory]
    [MemberData(nameof(NonExistingCustomerIds))]
    public void GetPurchasesOfNonExistingCustomerShouldReturnEmptyCollection(int customerId) 
    {
        var sales = Executor.GetCustomersPurchases(customerId);
        sales.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> NonExistingCustomerIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}