using NodaTime;
using Server.Models.Auxiliary;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.CustomersRepositoryTests;

public class RemoveTests : CustomersRepositoryTestBase
{
     public RemoveTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingCustomers))]
    public void RemoveExistingCustomerShouldReturnTrue(int customerId)
    {
        var removalSuccessful = Repository.Remove(customerId);
        removalSuccessful.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> ExistingCustomers =>
        new List<object[]>
        {
            new object[] { 1 },
            new object[] { 2 },
            new object[] { 3 },
        };
    
    

    [Theory]
    [MemberData(nameof(NonExistingCustomers))]
    public void RemoveNonExistingCustomerShouldReturnFalse(int customerId)
    {
        var removalSuccessful = Repository.Remove(customerId);
        removalSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingCustomers =>
        new List<object[]>
        {
            new object[] { 15 },
            new object[] { 45 },
            new object[] { 54 },
        };
    
    
    
    [Theory]
    [MemberData(nameof(RemoveCustomerShouldNotAffectOtherCustomersTestData))]
    public void RemoveExistingCustomerShouldNotAffectOtherCustomer(int customerId, ICollection<Customer> expectedCustomers)
    {
        var removalSuccessful = Repository.Remove(customerId);
        var customers = Repository.GetAll();
        
        removalSuccessful.Should().BeTrue();
        customers.Should().BeEquivalentTo(expectedCustomers);
    }
    
    public static IEnumerable<object[]> RemoveCustomerShouldNotAffectOtherCustomersTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                1,
                new []
                {
                    new Customer
                    {
                        Id = 2,
                        Name = "Anna",
                        Contacts = new Contacts { [ContactType.Email] = "annagmail@gmail.com" },
                        RegistrationDate = new LocalDate(2020, 01, 02)
                    },
                    new Customer
                    {
                        Id = 3,
                        Name = "Alexander",
                        Contacts = new Contacts { [ContactType.Phone] = "8-800-555-35-35" },
                        RegistrationDate = new LocalDate(2022, 07, 15)
                    },
                    new Customer
                    {
                        Id = 4,
                        Name = "Natali",
                        Contacts = new Contacts { [ContactType.Phone] = "8-800-500-30-30", [ContactType.Email] = "annagmail@gmail.com" },
                        RegistrationDate = new LocalDate(2022, 07, 15)
                    }
                },
            },
            new object[] 
            { 
                2, 
                new []
                {
                    new Customer
                    {
                        Id = 1,
                        Name = "Oleg",
                        Contacts = null,
                        RegistrationDate = new LocalDate(2021, 05, 30)
                    },
                    new Customer
                    {
                        Id = 3,
                        Name = "Alexander",
                        Contacts = new Contacts { [ContactType.Phone] = "8-800-555-35-35" },
                        RegistrationDate = new LocalDate(2022, 07, 15)
                    },
                    new Customer
                    {
                        Id = 4,
                        Name = "Natali",
                        Contacts = new Contacts { [ContactType.Phone] = "8-800-500-30-30", [ContactType.Email] = "annagmail@gmail.com" },
                        RegistrationDate = new LocalDate(2022, 07, 15)
                    }
                }
            },
            new object[] 
            { 
                3, 
                new []
                {
                    new Customer
                    {
                        Id = 1,
                        Name = "Oleg",
                        Contacts = null,
                        RegistrationDate = new LocalDate(2021, 05, 30)
                    },
                    new Customer
                    {
                        Id = 2,
                        Name = "Anna",
                        Contacts = new Contacts { [ContactType.Email] = "annagmail@gmail.com" },
                        RegistrationDate = new LocalDate(2020, 01, 02)
                    },
                    new Customer
                    {
                        Id = 4,
                        Name = "Natali",
                        Contacts = new Contacts { [ContactType.Phone] = "8-800-500-30-30", [ContactType.Email] = "annagmail@gmail.com" },
                        RegistrationDate = new LocalDate(2022, 07, 15)
                    }
                } 
            },
        };
}