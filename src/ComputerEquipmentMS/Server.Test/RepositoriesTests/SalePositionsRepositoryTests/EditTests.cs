using NodaTime;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.SalePositionsRepositoryTests;

public class EditTests : SalePositionsRepositoryTestBase
{
    public EditTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingSalePositions))]
    public void EditExistingSalePositionShouldCorrectlyEditSalePosition(SalePosition salePositionToEdit, SalePosition expectedEditedSalePosition)
    {
        var editSuccessful = Repository.Edit(salePositionToEdit);
        var editedSalePosition = Repository.GetById(salePositionToEdit.Id);

        editSuccessful.Should().BeTrue();
        editedSalePosition.Should().BeEquivalentTo(expectedEditedSalePosition);
    }
    
    public static IEnumerable<object[]> ExistingSalePositions =>
        new List<object[]>
        {
            new object[] 
            { 
                new SalePosition
                {
                    Id = 1,
                    SaleId = 1,
                    ConfigurationId = 1,
                    Cost = 150_000,
                    DiscountPercentage = 5,
                    WarrantyPeriod = Period.FromYears(2),
                },
                new SalePosition
                {
                    Id = 1,
                    SaleId = 1,
                    ConfigurationId = 1,
                    Cost = 150_000,
                    DiscountPercentage = 5,
                    WarrantyPeriod = Period.FromYears(2),
                }, 
            },
            new object[] 
            { 
                new SalePosition
                {
                    Id = 2,
                    SaleId = 1,
                    ConfigurationId = 2,
                    Cost = 500_000,
                    DiscountPercentage = 5,
                    WarrantyPeriod = Period.FromYears(1),
                }, 
                new SalePosition
                {
                    Id = 2,
                    SaleId = 1,
                    ConfigurationId = 2,
                    Cost = 500_000,
                    DiscountPercentage = 5,
                    WarrantyPeriod = Period.FromYears(1),
                }, 
            },
            new object[] 
            { 
                new SalePosition
                {
                    Id = 3,
                    SaleId = 2,
                    ConfigurationId = 2,
                    Cost = 110_000,
                    DiscountPercentage = 0,
                    WarrantyPeriod = Period.FromDays(180),
                }, 
                new SalePosition
                {
                    Id = 3,
                    SaleId = 2,
                    ConfigurationId = 2,
                    Cost = 110_000,
                    DiscountPercentage = 0,
                    WarrantyPeriod = Period.FromDays(180),
                }, 
            },
        };

    
    
    [Theory]
    [MemberData(nameof(NonExistingSalePositions))]
    public void EditNonExistingSalePositionShouldReturnFalse(SalePosition salePositionToEdit)
    {
        var editSuccessful = Repository.Edit(salePositionToEdit);
        editSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingSalePositions =>
        new List<object[]>
        {
            new object[] { new SalePosition { Id = 15, WarrantyPeriod = Period.Zero }, },
            new object[] { new SalePosition { Id = 45, WarrantyPeriod = Period.Zero }, },
            new object[] { new SalePosition { Id = 54, WarrantyPeriod = Period.Zero }, },
        };
    
    
    
    [Theory]
    [MemberData(nameof(EditSalePositionShouldNotAffectOtherSalePositionsTestData))]
    public void EditExistingSalePositionShouldNotAffectOtherSalePosition(SalePosition salePositionToEdit, ICollection<SalePosition> expectedSalePositions)
    {
        var editSuccessful = Repository.Edit(salePositionToEdit);
        var salePositions = Repository.GetAll();
        
        editSuccessful.Should().BeTrue();
        salePositions.Should().BeEquivalentTo(expectedSalePositions);
    }
    
    public static IEnumerable<object[]> EditSalePositionShouldNotAffectOtherSalePositionsTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                new SalePosition
                {
                    Id = 1,
                    SaleId = 1,
                    ConfigurationId = 1,
                    Cost = 150_000,
                    DiscountPercentage = 5,
                    WarrantyPeriod = Period.FromYears(2),
                },
                new []
                {
                    new SalePosition
                    {
                        Id = 1,
                        SaleId = 1,
                        ConfigurationId = 1,
                        Cost = 150_000,
                        DiscountPercentage = 5,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                    new SalePosition
                    {
                        Id = 2,
                        SaleId = 2,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 3,
                        SaleId = 2,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                    new SalePosition
                    {
                        Id = 4,
                        SaleId = 3,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 5,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 5,
                        SaleId = 3,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                    new SalePosition
                    {
                        Id = 6,
                        SaleId = 3,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 15,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                },
            },
            new object[] 
            { 
                new SalePosition
                {
                    Id = 2,
                    SaleId = 1,
                    ConfigurationId = 2,
                    Cost = 500_000,
                    DiscountPercentage = 5,
                    WarrantyPeriod = Period.FromYears(1),
                },
                new []
                {
                    new SalePosition
                    {
                        Id = 1,
                        SaleId = 1,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 2,
                        SaleId = 1,
                        ConfigurationId = 2,
                        Cost = 500_000,
                        DiscountPercentage = 5,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 3,
                        SaleId = 2,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                    new SalePosition
                    {
                        Id = 4,
                        SaleId = 3,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 5,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 5,
                        SaleId = 3,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                    new SalePosition
                    {
                        Id = 6,
                        SaleId = 3,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 15,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                },
            },
            new object[] 
            { 
                new SalePosition
                {
                    Id = 3,
                    SaleId = 2,
                    ConfigurationId = 2,
                    Cost = 110_000,
                    DiscountPercentage = 0,
                    WarrantyPeriod = Period.FromDays(180),
                },
                new []
                {
                    new SalePosition
                    {
                        Id = 1,
                        SaleId = 1,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 2,
                        SaleId = 2,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 3,
                        SaleId = 2,
                        ConfigurationId = 2,
                        Cost = 110_000,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromDays(180),
                    },
                    new SalePosition
                    {
                        Id = 4,
                        SaleId = 3,
                        ConfigurationId = 1,
                        Cost = 121_000,
                        DiscountPercentage = 5,
                        WarrantyPeriod = Period.FromYears(1),
                    },
                    new SalePosition
                    {
                        Id = 5,
                        SaleId = 3,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 0,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                    new SalePosition
                    {
                        Id = 6,
                        SaleId = 3,
                        ConfigurationId = 2,
                        Cost = 106_500,
                        DiscountPercentage = 15,
                        WarrantyPeriod = Period.FromYears(2),
                    },
                },
            },
        };
}