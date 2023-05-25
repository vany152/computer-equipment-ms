using NodaTime;
using Server.Models.Auxiliary;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.CustomersRepositoryTests;

public class AddTests : CustomersRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(NewCustomers))]
    public void AddedCustomerEqualExpected(Customer newCustomer, Customer expectedAddedCustomer)
    {
        var addedCustomer = Repository.Add(newCustomer);
        addedCustomer.Should().BeEquivalentTo(expectedAddedCustomer);
    }
    
    public static IEnumerable<object[]> NewCustomers =>
        new List<object[]>
        {
            new object[]
            {
                new Customer
                {
                    Name = "New customer",
                    Contacts = new Contacts { [ContactType.Email] = "new email" },
                    RegistrationDate = new LocalDate(2023, 05, 25, CalendarSystem.Iso)
                },
                new Customer
                {
                    Id = 1,
                    Name = "New customer",
                    Contacts = new Contacts { [ContactType.Email] = "new email" },
                    RegistrationDate = new LocalDate(2023, 05, 25),
                }
            }
        };



    [Theory]
    [MemberData(nameof(Customers))]
    public void AddNewCustomerDoesNotAffectOtherCustomers(Customer newCustomer, ICollection<Customer> expectedCustomers)
    {
        FillDbWithTestData();
        
        Repository.Add(newCustomer);
        var customers = Repository.GetAll();
        customers.Should().BeEquivalentTo(expectedCustomers);
    }

    public static IEnumerable<object[]> Customers =>
        new List<object[]>
        {
            new object[]
            {
                new Customer
                {
                    Name = "New customer",
                    Contacts = new Contacts { [ContactType.Email] = "new email" },
                    RegistrationDate = new LocalDate(2023, 05, 25)
                },
                new[]
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
                        Id = 3,
                        Name = "Alexander",
                        Contacts = new Contacts { [ContactType.Phone] = "8-800-555-35-35" },
                        RegistrationDate = new LocalDate(2022, 07, 15)
                    },
                    new Customer
                    {
                        Id = 4,
                        Name = "Natali",
                        Contacts = new Contacts
                            { [ContactType.Phone] = "8-800-500-30-30", [ContactType.Email] = "annagmail@gmail.com" },
                        RegistrationDate = new LocalDate(2022, 07, 15)
                    },
                    new Customer
                    {
                        Id = 5,
                        Name = "New customer",
                        Contacts = new Contacts { [ContactType.Email] = "new email" },
                        RegistrationDate = new LocalDate(2023, 05, 25)
                    },
                }
            }
        };
}