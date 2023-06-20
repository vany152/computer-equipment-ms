using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using static ComputerEquipmentMS.Constants.DbTableNames;

namespace ComputerEquipmentMS.DataAccess;

public class SalePositionsNpgsqlRepository : AbstractNpgsqlRepository<SalePosition, SalePositionEntity, int>
{
    public SalePositionsNpgsqlRepository(IDbConnectionString connectionString, string tableName = SalePositionsTableName) 
        : base(connectionString, tableName, DynamicToObjectMapper.MapDynamicToSalePosition)
    {
    }

    protected override string ConstructAndReturnAddQueryString(SalePositionEntity salePosition) =>
        throw new NotImplementedException("This method cannot be implemented because of architecture restrictions");


    protected override string ConstructAndReturnEditQueryString(SalePositionEntity salePosition) =>
        $"""
            update sale_positions
            set sale_id = {salePosition.SaleId},
                computer_configuration_id = {salePosition.ConfigurationId},
                cost = {salePosition.Cost},
                discount_percentage = {salePosition.DiscountPercentage},
                warranty_period = '{salePosition.WarrantyPeriod}'
            where id = {salePosition.Id}
        """;
}
