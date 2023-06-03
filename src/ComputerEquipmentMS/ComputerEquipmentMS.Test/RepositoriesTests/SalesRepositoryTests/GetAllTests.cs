﻿using ComputerEquipmentMS.Models.Domain;
using NodaTime;

namespace ComputerEquipmentMS.Test.RepositoriesTests.SalesRepositoryTests;

public class GetAllTests : SalesRepositoryTestBase
{
    [Theory]
    [MemberData(nameof(Sales))]
    public void GetAllSalesOfNonEmptyRepositoryShouldReturnCorrectSalesCollection(ICollection<Sale> expectedSales)
    {
        FillDbWithTestData();

        var sales = Repository.GetAll();
        sales.Should().BeEquivalentTo(expectedSales);
    }

    public static IEnumerable<object[]> Sales =>
        new List<object[]>
        {
            new object[]
            {
                new[]
                {
                    new Sale
                    {
                        Id = 1,
                        CustomerId = 1,
                        SaleMoment = Instant.FromUtc(2021, 05, 30, 12, 00, 00),
                        DiscountPercentage = 0,
                        SalePositions = new[] { 1 }
                    },
                    new Sale
                    {
                        Id = 2,
                        CustomerId = 2,
                        SaleMoment = Instant.FromUtc(2020, 01, 02, 12, 00, 00),
                        DiscountPercentage = 5,
                        SalePositions = new[] { 2, 3 }
                    },
                    new Sale
                    {
                        Id = 3,
                        CustomerId = 1,
                        SaleMoment = Instant.FromUtc(2022, 05, 30, 12, 00, 00),
                        DiscountPercentage = 10,
                        SalePositions = new List<int>{ 4, 5, 6 }
                    },
                }
            }
        };



    [Fact]
    public void GetAllSalesOfEmptyRepositoryShouldReturnEmptyCollection()
    {
        var sales = Repository.GetAll();
        sales.Should().BeEmpty();
    }
}