using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.SalesRepositoryTests;

public class EditTests : SalesRepositoryTestBase
{
    public EditTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingSales))]
    public void EditExistingSaleShouldCorrectlyEditSale(Sale saleToEdit, Sale expectedEditedSale)
    {
        var editSuccessful = Repository.Edit(saleToEdit);
        var editedSale = Repository.GetById(saleToEdit.Id);
    
        editSuccessful.Should().BeTrue();
        editedSale.Should().BeEquivalentTo(expectedEditedSale);
    }
    
    public static IEnumerable<object[]> ExistingSales =>
        new List<object[]>
        {
            new object[] 
            { 
                new Sale
                {
                    Id = 1,
                    CustomerId = 2,
                    SaleMoment = Instant.FromUtc(2022, 04, 04, 12, 00, 00),
                    DiscountPercentage = 10,
                    SalePositionIds = new[] { 1 }
                },
                new Sale
                {
                    Id = 1,
                    CustomerId = 2,
                    SaleMoment = Instant.FromUtc(2022, 04, 04, 12, 00, 00),
                    DiscountPercentage = 10,
                    SalePositionIds = new[] { 1 }
                }, 
            },
            new object[] 
            { 
                new Sale
                {
                    Id = 2,
                    CustomerId = 1,
                    SaleMoment = Instant.FromUtc(2000, 01, 02, 12, 00, 00),
                    DiscountPercentage = 0,
                    SalePositionIds = new[] { 2, 3 }
                }, 
                new Sale
                {
                    Id = 2,
                    CustomerId = 1,
                    SaleMoment = Instant.FromUtc(2000, 01, 02, 12, 00, 00),
                    DiscountPercentage = 0,
                    SalePositionIds = new[] { 2, 3 }
                }, 
            }
        };
    
    
    
    [Theory]
    [MemberData(nameof(NonExistingSales))]
    public void EditNonExistingSaleShouldReturnFalse(Sale saleToEdit)
    {
        var editSuccessful = Repository.Edit(saleToEdit);
        editSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingSales =>
        new List<object[]>
        {
            new object[] { new Sale { Id = 15, }, },
            new object[] { new Sale { Id = 45, }, },
            new object[] { new Sale { Id = 54, }, },
        };
    
    
    
    [Theory]
    [MemberData(nameof(EditSalesShouldNotAffectOtherCustomersTestData))]
    public void EditExistingSaleShouldNotAffectOtherSales(Sale saleToEdit, ICollection<Sale> expectedSales)
    {
        var editSuccessful = Repository.Edit(saleToEdit);
        var sales = Repository.GetAll();
        
        editSuccessful.Should().BeTrue();
        sales.Should().BeEquivalentTo(expectedSales);
    }
    
    public static IEnumerable<object[]> EditSalesShouldNotAffectOtherCustomersTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                new Sale
                {
                    Id = 1,
                    CustomerId = 2,
                    SaleMoment = Instant.FromUtc(2022, 04, 04, 12, 00, 00),
                    DiscountPercentage = 10,
                },
                new []
                {
                    new Sale
                    {
                        Id = 1,
                        CustomerId = 2,
                        SaleMoment = Instant.FromUtc(2022, 04, 04, 12, 00, 00),
                        DiscountPercentage = 10,
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
                        SalePositionIds = new List<int>{ 4, 5, 6 }
                    },
                },
            },
            new object[] 
            { 
                new Sale
                {
                    Id = 2,
                    CustomerId = 1,
                    SaleMoment = Instant.FromUtc(2000, 01, 02, 12, 00, 00),
                    DiscountPercentage = 0,
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
                        CustomerId = 1,
                        SaleMoment = Instant.FromUtc(2000, 01, 02, 12, 00, 00),
                        DiscountPercentage = 0,
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
                }
            },
        };
}