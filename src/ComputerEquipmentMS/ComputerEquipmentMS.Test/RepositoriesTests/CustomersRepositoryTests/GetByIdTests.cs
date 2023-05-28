using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.CustomersRepositoryTests;

public class GetByIdTests : CustomersRepositoryTestBase
{
    public GetByIdTests()
    {
        base.FillDbWithTestData();
    }

    [Theory]
    [MemberData(nameof(ExistingIds))]
    public void GetCustomerByExistingIdShouldReturnCorrectCustomer(int customerId, Customer expectedCustomer)
    {
        var customer = Repository.GetById(customerId);
        customer.Should().BeEquivalentTo(expectedCustomer);
    }
    
    public static IEnumerable<object[]> ExistingIds =>
        new List<object[]>
        {
            new object[] 
            { 
                1, new Customer
                {
                    Id = 1,
                    Name = "Oleg",
                    Contacts = null,
                    RegistrationDate = new LocalDate(2021, 05, 30)
                } 
            },
            new object[] 
            { 
                2, new Customer
                {
                    Id = 2,
                    Name = "Anna",
                    Contacts = new Contacts { [ContactType.Email] = "annagmail@gmail.com" },
                    RegistrationDate = new LocalDate(2020, 01, 02)
                } 
            },
            new object[] 
            { 
                3, new Customer
                {
                    Id = 3,
                    Name = "Alexander",
                    Contacts = new Contacts { [ContactType.Phone] = "8-800-555-35-35" },
                    RegistrationDate = new LocalDate(2022, 07, 15)
                } 
            },
            new object[] 
            { 
                4, new Customer
                {
                    Id = 4,
                    Name = "Natali",
                    Contacts = new Contacts { [ContactType.Phone] = "8-800-500-30-30", [ContactType.Email] = "annagmail@gmail.com" },
                    RegistrationDate = new LocalDate(2022, 07, 15)
                } 
            },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingIds))]
    public void GetCustomerByNonExistingIdShouldReturnNull(int customerId)
    {
        var customer = Repository.GetById(customerId);
        customer.Should().BeNull();
    }

    public static IEnumerable<object[]> NonExistingIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}