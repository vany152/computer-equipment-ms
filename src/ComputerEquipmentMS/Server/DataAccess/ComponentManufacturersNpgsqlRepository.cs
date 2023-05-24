using Server.Models.Domain;
using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class ComponentManufacturersNpgsqlRepository : AbstractNpgsqlRepository<ComponentManufacturer, int>
{
    public ComponentManufacturersNpgsqlRepository(string connectionString, string tableName = ComponentManufacturersTableName)
        : base(connectionString, tableName)
    {
    }

    
    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(ComponentManufacturer manufacturer) =>
        $"""
            select * from create_component_manufacturer('{manufacturer.Name}')
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(ComponentManufacturer manufacturer) =>
        $"""
            update {TableName}
            set name = '{manufacturer.Name}'
            where id = {manufacturer.Id}
        """;
}