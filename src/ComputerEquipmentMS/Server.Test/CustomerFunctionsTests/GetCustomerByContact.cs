using Server.Models.Auxiliary;

namespace Server.Test.CustomerFunctionsTests;

public class GetCustomerByContact : CustomerFunctionsTestBase
{
    [Theory]
    [MemberData(nameof(ExistingContacts))]
    public void GetCustomersByExistingContactShouldReturnCorrectCustomerIds(
        ContactType contactType, 
        string contact, 
        ICollection<int> expectedCustomerIds) 
    {
        var customers = Executor.GetCustomersByContact(contactType, contact);
        var customerIds = customers.Select(customer => customer.Id); 
        customerIds.Should().BeEquivalentTo(expectedCustomerIds);
    }
    
    public static IEnumerable<object[]> ExistingContacts =>
        new List<object[]>
        {
            new object[] { ContactType.Email, "annagmail@gmail.com", new [] {2} },
            new object[] { ContactType.Phone, "8-800-555-35-35", new [] {3} },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingContacts))]
    public void GetCustomersByNotExistingContactShouldReturnEmptyCollection(ContactType contactType, string contact) 
    {
        var customers = Executor.GetCustomersByContact(contactType, contact);
        customers.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> NonExistingContacts =>
        new List<object[]>
        {
            new object[] { ContactType.Email, "email" },
            new object[] { ContactType.Phone, "phone" },
        };
}