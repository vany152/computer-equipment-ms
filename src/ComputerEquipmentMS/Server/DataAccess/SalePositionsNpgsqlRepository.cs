using Server.Models.Domain;
using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class SalePositionsNpgsqlRepository : AbstractNpgsqlRepository<SalePosition, int>
{
    public SalePositionsNpgsqlRepository(string connectionString, string tableName = SalePositionsTableName) 
        : base(connectionString, tableName, MapDynamicToSalePosition)
    {
    }

    protected override string ConstructAndReturnAddQueryString(SalePosition salePosition) =>
        $"""
            insert into {TableName} (
                sale_id, 
                computer_configuration_id, 
                cost, 
                discount_percentage, 
                warranty_period
            ) values (
                {salePosition.SaleId}, 
                {salePosition.ConfigurationId}, 
                {salePosition.Cost}, 
                {salePosition.DiscountPercentage}, 
                '{salePosition.WarrantyPeriod}'
            ) returning *
        """;

    protected override string ConstructAndReturnEditQueryString(SalePosition salePosition) =>
        $"""
            update sale_positions
            set sale_id = {salePosition.SaleId},
                computer_configuration_id = {salePosition.ConfigurationId},
                cost = {salePosition.Cost},
                discount_percentage = {salePosition.DiscountPercentage},
                warranty_period = '{salePosition.WarrantyPeriod}'
            where id = {salePosition.Id}
        """;

    private static SalePosition MapDynamicToSalePosition(dynamic obj) =>
        new()
        {
            Id = obj.id,
            SaleId = obj.sale_id,
            ConfigurationId = obj.computer_configuration_id,
            Cost = obj.cost,
            DiscountPercentage = obj.discount_percentage,
            WarrantyPeriod = obj.warranty_period,
        };
}
