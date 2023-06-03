using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using static ComputerEquipmentMS.DataAccess.Constants.DbTableNames;

namespace ComputerEquipmentMS.DataAccess;

public class SalesNpgsqlRepository : AbstractNpgsqlRepository<Sale, SaleEntity, int>
{
    public SalesNpgsqlRepository(IDbConnectionString connectionString, string tableName = SalesTableName) 
        : base(connectionString, tableName, DynamicToObjectMapper.MapDynamicToSale)
    {
    }

    protected override string ConstructAndReturnGetAllQueryString() =>
        "select * from get_sales()";

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(SaleEntity sale) =>
        throw new NotImplementedException("This method cannot be implemented because of architecture restrictions");

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(SaleEntity sale) =>
        $"""
            update {TableName} 
            set customer_id = {sale.CustomerId},
                sale_moment = '{sale.SaleMoment}'::timestamptz,
                discount_percentage = {sale.DiscountPercentage}::smallint
            where id = {sale.Id};
        """;
}
