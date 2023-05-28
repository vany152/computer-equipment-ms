using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.SalesRepositoryTests;

public class GetByIdTests : SalesRepositoryTestBase
{
    public GetByIdTests()
    {
        base.FillDbWithTestData();
    }

    [Theory]
    [MemberData(nameof(ExistingIds))]
    public void GetByExistingIdShouldReturnCorrectSale(int saleId, Sale expectedSale)
    {
        var sale = Repository.GetById(saleId);
        sale.Should().BeEquivalentTo(expectedSale);
    }
    
    public static IEnumerable<object[]> ExistingIds =>
        new List<object[]>
        {
            new object[] 
            { 
                1, new Sale
                {
                    Id = 1,
                    CustomerId = 1,
                    SaleMoment = Instant.FromUtc(2021, 05, 30, 12, 00, 00),
                    DiscountPercentage = 0,
                    SalePositionIds = new[] { 1 }
                } 
            },
            new object[] 
            { 
                2, new Sale
                {
                    Id = 2,
                    CustomerId = 2,
                    SaleMoment = Instant.FromUtc(2020, 01, 02, 12, 00, 00),
                    DiscountPercentage = 5,
                    SalePositionIds = new[] { 2, 3 }
                } 
            },
            new object[] 
            { 
                3, new Sale
                {
                    Id = 3,
                    CustomerId = 1,
                    SaleMoment = Instant.FromUtc(2022, 05, 30, 12, 00, 00),
                    DiscountPercentage = 10,
                    SalePositionIds = new List<int>{ 4, 5, 6 }
                } 
            },
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingIds))]
    public void GetCustomerByNonExistingIdShouldReturnNull(int saleId)
    {
        var sale = Repository.GetById(saleId);
        sale.Should().BeNull();
    }

    public static IEnumerable<object[]> NonExistingIds =>
        new List<object[]>
        {
            new object[] { 45 },
            new object[] { 54 },
        };
}