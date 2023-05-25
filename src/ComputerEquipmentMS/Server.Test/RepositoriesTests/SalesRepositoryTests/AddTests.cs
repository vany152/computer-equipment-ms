using NodaTime;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.SalesRepositoryTests;

public class AddTests : SalesRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(NewSales))]
    public void AddedSaleEqualExpected(Sale newSale, Sale expectedAddedSale)
    {
        var addedSale = Repository.Add(newSale);
        addedSale.Should().BeEquivalentTo(expectedAddedSale);
    }
    
    public static IEnumerable<object[]> NewSales =>
        new List<object[]>
        {
            new object[]
            {
                new Sale
                {
                    CustomerId = 2,
                    SaleMoment = Instant.FromUtc(2022, 10, 10, 12, 00),
                    DiscountPercentage = 5,
                    SalePositionIds = new [] {1},
                },
                new Sale
                {
                    Id = 1,
                    CustomerId = 2,
                    SaleMoment = Instant.FromUtc(2022, 10, 10, 12, 00),
                    DiscountPercentage = 5,
                    SalePositionIds = new [] {1},
                }
            }
        };
    
    
    
    [Theory]
    [MemberData(nameof(Sales))]
    public void AddNewCustomerDoesNotAffectOtherCustomers(Sale newSale, ICollection<Sale> expectedSales)
    {
        FillDbWithTestData();
        
        Repository.Add(newSale);
    
        var sales = Repository.GetAll();
        sales.Should().BeEquivalentTo(expectedSales);
    }
    
    public static IEnumerable<object[]> Sales =>
        new List<object[]>
        {
            new object[]
            {
                new Sale
                {
                    CustomerId = 2,
                    SaleMoment = Instant.FromUtc(2022, 10, 10, 12, 00),
                    DiscountPercentage = 5,
                    SalePositionIds = null,
                },
                new[]
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
                    new Sale
                    {
                        Id = 3,
                        CustomerId = 1,
                        SaleMoment = Instant.FromUtc(2022, 05, 30, 12, 00, 00),
                        DiscountPercentage = 10,
                        SalePositionIds = new []{ 4, 5, 6 }
                    },
                    new Sale
                    {
                        CustomerId = 4,
                        SaleMoment = Instant.FromUtc(2022, 10, 10, 12, 00),
                        DiscountPercentage = 5,
                        SalePositionIds = new [] {1},
                    },
                }
            }
        };
}