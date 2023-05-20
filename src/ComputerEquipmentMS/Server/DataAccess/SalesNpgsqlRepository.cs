using Server.Models;

using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class SalesNpgsqlRepository : AbstractNpgsqlRepository<Sale, int>
{
    public SalesNpgsqlRepository(string connectionString, string tableName = SalesTableName) 
        : base(connectionString, tableName, MapDynamicToSale)
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(Sale sale) =>
        $"""    
            insert into {TableName} (customer_id, sale_moment, discount_percentage) 
            values ({sale.CustomerId}, '{sale.SaleMoment:yyyy-MM-dd hh:mm:ss zz}'::timestamptz, {sale.DiscountPercentage})
            returning *
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(Sale sale) =>
        $"""
            update {TableName} 
            set customer_id = {sale.CustomerId},
                sale_moment = '{sale.SaleMoment:yyyy-MM-dd hh:mm:ss zz}'::timestamptz,
                discount_percentage = {sale.DiscountPercentage}
            where id = {sale.Id};
        """;



    private static Sale MapDynamicToSale(dynamic obj) =>
        new ()
        {
            Id = obj.id,
            CustomerId = obj.customer_id,
            SaleMoment = obj.sale_moment,
            DiscountPercentage = obj.discount_percentage
        };
}
