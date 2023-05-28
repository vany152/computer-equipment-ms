using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.SalesRepositoryTests;

public class RemoveTests : SalesRepositoryTestBase
{
     public RemoveTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingIds))]
    public void RemoveExistingSaleShouldReturnTrue(int saleId)
    {
        var removalSuccessful = Repository.Remove(saleId);
        removalSuccessful.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> ExistingIds =>
        new List<object[]>
        {
            new object[] { 1 },
            new object[] { 2 },
            new object[] { 3 },
        };
    
    

    [Theory]
    [MemberData(nameof(NonExistingIds))]
    public void RemoveNonExistingSaleShouldReturnFalse(int saleId)
    {
        var removalSuccessful = Repository.Remove(saleId);
        removalSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingIds =>
        new List<object[]>
        {
            new object[] { 15 },
            new object[] { 45 },
            new object[] { 54 },
        };
    
    
    
    [Theory]
    [MemberData(nameof(RemoveCustomerShouldNotAffectOtherCustomersTestData))]
    public void RemoveExistingSaleShouldNotAffectOtherSales(int saleId, ICollection<Sale> expectedSales)
    {
        var removalSuccessful = Repository.Remove(saleId);
        var customers = Repository.GetAll();
        
        removalSuccessful.Should().BeTrue();
        customers.Should().BeEquivalentTo(expectedSales);
    }
    
    public static IEnumerable<object[]> RemoveCustomerShouldNotAffectOtherCustomersTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                1,
                new []
                {
                    new Sale
                    {
                        Id = 2,
                        CustomerId = 2,
                        SaleMoment = Instant.FromUtc(2020, 01, 02, 12, 00, 00),
                        DiscountPercentage = 5,
                        SalePositionIds = new[] { 2, 3 }
                    },
                    new Sale
                    {
                        Id = 3,
                        CustomerId = 1,
                        SaleMoment = Instant.FromUtc(2022, 05, 30, 12, 00, 00),
                        DiscountPercentage = 10,
                        SalePositionIds = new List<int>{ 4, 5, 6 }
                    },
                },
            },
            new object[] 
            { 
                2, 
                new []
                {
                    new Sale
                    {
                        Id = 1,
                        CustomerId = 1,
                        SaleMoment = Instant.FromUtc(2021, 05, 30, 12, 00, 00),
                        DiscountPercentage = 0,
                        SalePositionIds = new[] { 1 }
                    },
                    new Sale
                    {
                        Id = 3,
                        CustomerId = 1,
                        SaleMoment = Instant.FromUtc(2022, 05, 30, 12, 00, 00),
                        DiscountPercentage = 10,
                        SalePositionIds = new List<int>{ 4, 5, 6 }
                    },
                }
            },
            new object[] 
            { 
                3, 
                new []
                {
                    new Sale
                    {
                        Id = 1,
                        CustomerId = 1,
                        SaleMoment = Instant.FromUtc(2021, 05, 30, 12, 00, 00),
                        DiscountPercentage = 0,
                        SalePositionIds = new[] { 1 }
                    },
                    new Sale
                    {
                        Id = 2,
                        CustomerId = 2,
                        SaleMoment = Instant.FromUtc(2020, 01, 02, 12, 00, 00),
                        DiscountPercentage = 5,
                        SalePositionIds = new[] { 2, 3 }
                    },
                } 
            },
        };
}