using ComputerEquipmentMS.Models.Domain;
using static ComputerEquipmentMS.DataAccess.Constants.DbTableNames;

namespace ComputerEquipmentMS.DataAccess;

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