using Server.Models.Domain;
using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class SalesNpgsqlRepository : AbstractNpgsqlRepository<Sale, int>
{
    public SalesNpgsqlRepository(string connectionString, string tableName = SalesTableName) 
        : base(connectionString, tableName, DynamicToObjectMapper.MapDynamicToSale)
    {
    }

    protected override string ConstructAndReturnGetAllQueryString() =>
        "select * from get_sales()";

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(Sale sale) =>
        throw new NotImplementedException("This method cannot be implemented because of architecture restrictions");

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(Sale sale) =>
        $"""
            update {TableName} 
            set customer_id = {sale.CustomerId},
                sale_moment = '{sale.SaleMoment}'::timestamptz,
                discount_percentage = {sale.DiscountPercentage}::smallint
            where id = {sale.Id};
        """;
}
