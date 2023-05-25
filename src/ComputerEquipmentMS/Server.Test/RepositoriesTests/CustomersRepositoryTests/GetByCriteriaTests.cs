using NodaTime;
using Server.Models.Auxiliary;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.CustomersRepositoryTests;

public class GetByCriteriaTests : CustomersRepositoryTestBase
{
    public GetByCriteriaTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(GetCustomersByIdData))]
    public void GetByIdShouldReturnCorrectCustomerIds(int id, IEnumerable<int> expectedCustomerIds)
    {
        var customers = Repository.GetByCriteria(customer => customer.Id == id);
        var customerIds = GetCustomerIds(customers);
        customerIds.Should().BeEquivalentTo(expectedCustomerIds);
    }
    
    public static IEnumerable<object[]> GetCustomersByIdData =>
        new List<object[]>
        {
            new object[] { 1, new [] {1} },
            new object[] { 2, new [] {2} },
            new object[] { 3, new [] {3} },
            new object[] { 4, new [] {4} },
            new object[] { 45, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetCustomersByNameData))]
    public void GetByNameShouldReturnCorrectCustomerIds(string name, IEnumerable<int> expectedCustomerIds)
    {
        var customers = Repository.GetByCriteria(customer => customer.Name == name);
        var customerIds = GetCustomerIds(customers);
        customerIds.Should().BeEquivalentTo(expectedCustomerIds);    
    }
    
    public static IEnumerable<object[]> GetCustomersByNameData =>
        new List<object[]>
        {
            new object[] { "Oleg", new [] {1} },
            new object[] { "Anna", new [] {2} },
            new object[] { "Alexander", new [] {3} },
            new object[] { "Natali", new [] {4} },
            new object[] { "name", Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetCustomersByContactsData))]
    public void GetByContactsShouldReturnCorrectCustomerIds(Contacts? contacts, IEnumerable<int> expectedCustomerIds)
    {
        var customers = Repository.GetByCriteria(customer => Util.DictionariesEqual(customer.Contacts, contacts));
        var customerIds = GetCustomerIds(customers);
        customerIds.Should().BeEquivalentTo(expectedCustomerIds);
    }
    
    public static IEnumerable<object?[]> GetCustomersByContactsData =>
        new List<object?[]>
        {
            new object?[] { null, new [] {1} },
            new object?[] { new Contacts { [ContactType.Email] = "annagmail@gmail.com" }, new [] {2} },
            new object?[] { new Contacts { [ContactType.Phone] = "8-800-555-35-35" }, new [] { 3 } },
            new object?[] { new Contacts { [ContactType.Phone] = "8-800-500-30-30", [ContactType.Email] = "annagmail@gmail.com" }, new [] {4} },
            new object?[] { new Contacts { [ContactType.Phone] = "phone" }, Array.Empty<int>() },
        };
    
    
    
    [Theory]
    [MemberData(nameof(GetCustomersByRegistrationDateData))]
    public void GetByRegistrationDateShouldReturnCorrectCustomerIds(LocalDate registrationDate, IEnumerable<int> expectedCustomerIds)
    {
        var customers = Repository.GetByCriteria(customer => customer.RegistrationDate == registrationDate);
        var customerIds = GetCustomerIds(customers);
        customerIds.Should().BeEquivalentTo(expectedCustomerIds);
    }
    
    public static IEnumerable<object[]> GetCustomersByRegistrationDateData =>
        new List<object[]>
        {
            new object[] { new LocalDate(2021, 05, 30), new [] {1} },
            new object[] { new LocalDate(2020, 01, 02), new [] {2} },
            new object[] { new LocalDate(2022, 07, 15), new [] {3, 4} },
            new object[] { new LocalDate(2000, 01, 01), Array.Empty<int>() },
        };

    

    private static IEnumerable<int> GetCustomerIds(IEnumerable<Customer> customers) =>
        customers.Select(customer => customer.Id);
    
}