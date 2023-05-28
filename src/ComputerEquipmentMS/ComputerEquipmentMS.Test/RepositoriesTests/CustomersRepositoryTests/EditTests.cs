using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.CustomersRepositoryTests;

public class EditTests : CustomersRepositoryTestBase
{
    public EditTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingCustomers))]
    public void EditExistingCustomerShouldCorrectlyEditCustomer(Customer customerToEdit, Customer expectedEditedCustomer)
    {
        var editSuccessful = Repository.Edit(customerToEdit);
        var editedCustomer = Repository.GetById(customerToEdit.Id);

        editSuccessful.Should().BeTrue();
        editedCustomer.Should().BeEquivalentTo(expectedEditedCustomer);
    }
    
    public static IEnumerable<object[]> ExistingCustomers =>
        new List<object[]>
        {
            new object[] 
            { 
                new Customer
                {
                    Id = 1,
                    Name = "Nikolay",
                    Contacts = new Contacts { [ContactType.Email] = "new email" },
                    RegistrationDate = new LocalDate(2021, 05, 30)
                },
                new Customer
                {
                    Id = 1,
                    Name = "Nikolay",
                    Contacts = new Contacts { [ContactType.Email] = "new email" },
                    RegistrationDate = new LocalDate(2021, 05, 30)
                }, 
            },
            new object[] 
            { 
                new Customer
                {
                    Id = 2,
                    Name = "Nina",
                    Contacts = null,
                    RegistrationDate = new LocalDate(2015, 05, 05)
                }, 
                new Customer
                {
                    Id = 2,
                    Name = "Nina",
                    Contacts = null,
                    RegistrationDate = new LocalDate(2015, 05, 05)
                }, 
            },
            new object[] 
            { 
                new Customer
                {
                    Id = 3,
                    Name = "Ivan",
                    Contacts = new Contacts { [ContactType.Email] = "new_email@gmail.com" },
                    RegistrationDate = new LocalDate(2022, 07, 15)
                }, 
                new Customer
                {
                    Id = 3,
                    Name = "Ivan",
                    Contacts = new Contacts { [ContactType.Email] = "new_email@gmail.com" },
                    RegistrationDate = new LocalDate(2022, 07, 15)
                }, 
            },
        };

    
    
    [Theory]
    [MemberData(nameof(NonExistingCustomers))]
    public void EditNonExistingCustomerShouldReturnFalse(Customer customerToEdit)
    {
        var editSuccessful = Repository.Edit(customerToEdit);
        editSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingCustomers =>
        new List<object[]>
        {
            new object[] { new Customer { Id = 15, }, },
            new object[] { new Customer { Id = 45, }, },
            new object[] { new Customer { Id = 54, }, },
        };
    
    
    
    [Theory]
    [MemberData(nameof(EditCustomerShouldNotAffectOtherCustomersTestData))]
    public void EditExistingCustomerShouldNotAffectOtherCustomer(Customer customerToEdit, ICollection<Customer> expectedCustomers)
    {
        var editSuccessful = Repository.Edit(customerToEdit);
        var customers = Repository.GetAll();
        
        editSuccessful.Should().BeTrue();
        customers.Should().BeEquivalentTo(expectedCustomers);
    }
    
    public static IEnumerable<object[]> EditCustomerShouldNotAffectOtherCustomersTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                new Customer
                {
                    Id = 1,
                    Name = "Nikolay",
                    Contacts = new Contacts { [ContactType.Email] = "new email" },
                    RegistrationDate = new LocalDate(2021, 05, 30)
                },
                new []
                {
                    new Customer
                    {
                        Id = 1,
                        Name = "Nikolay",
                        Contacts = new Contacts { [ContactType.Email] = "new email" },
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
                        Contacts = new Contacts { [ContactType.Phone] = "8-800-500-30-30", [ContactType.Email] = "annagmail@gmail.com" },
                        RegistrationDate = new LocalDate(2022, 07, 15)
                    }
                },
            },
            new object[] 
            { 
                new Customer
                {
                    Id = 2,
                    Name = "Nina",
                    Contacts = null,
                    RegistrationDate = new LocalDate(2015, 05, 05)
                }, 
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
                        Name = "Nina",
                        Contacts = null,
                        RegistrationDate = new LocalDate(2015, 05, 05)
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
                new Customer
                {
                    Id = 3,
                    Name = "Ivan",
                    Contacts = new Contacts { [ContactType.Email] = "new_email@gmail.com" },
                    RegistrationDate = new LocalDate(2022, 07, 15)
                }, 
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
                        Id = 3,
                        Name = "Ivan",
                        Contacts = new Contacts { [ContactType.Email] = "new_email@gmail.com" },
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
        };
}