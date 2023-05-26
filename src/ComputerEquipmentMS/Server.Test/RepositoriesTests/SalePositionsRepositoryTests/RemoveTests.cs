using NodaTime;
using Server.Models.Domain;

namespace Server.Test.RepositoriesTests.SalePositionsRepositoryTests;

public class RemoveTests : SalePositionsRepositoryTestBase
{
     public RemoveTests()
    {
        base.FillDbWithTestData();
    }
    
    [Theory]
    [MemberData(nameof(ExistingSalePositions))]
    public void RemoveExistingSalePositionShouldReturnTrue(int salePositionId)
    {
        var removalSuccessful = Repository.Remove(salePositionId);
        removalSuccessful.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> ExistingSalePositions =>
        new List<object[]>
        {
            new object[] { 1 },
            new object[] { 2 },
            new object[] { 3 },
        };
    
    

    [Theory]
    [MemberData(nameof(NonExistingSalePositions))]
    public void RemoveNonExistingSalePositionShouldReturnFalse(int salePositionId)
    {
        var removalSuccessful = Repository.Remove(salePositionId);
        removalSuccessful.Should().BeFalse();
    }
    
    public static IEnumerable<object[]> NonExistingSalePositions =>
        new List<object[]>
        {
            new object[] { 15 },
            new object[] { 45 },
            new object[] { 54 },
        };
    
    
    
    [Theory]
    [MemberData(nameof(RemoveSalePositionShouldNotAffectOtherSalePositionsTestData))]
    public void RemoveExistingSalePositionShouldNotAffectOtherSalePosition(int salePositionId, ICollection<SalePosition> expectedSalePositions)
    {
        var removalSuccessful = Repository.Remove(salePositionId);
        var salePositions = Repository.GetAll();
        
        removalSuccessful.Should().BeTrue();
        salePositions.Should().BeEquivalentTo(expectedSalePositions);
    }
    
    public static IEnumerable<object[]> RemoveSalePositionShouldNotAffectOtherSalePositionsTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                1,
                new []
                {
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
                2, 
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
                }
            },
            new object[] 
            { 
                3, 
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
                } 
            },
        };
}