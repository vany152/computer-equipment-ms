using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using static ComputerEquipmentMS.DataAccess.Constants.DbTableNames;

namespace ComputerEquipmentMS.DataAccess;

public class ComponentManufacturersNpgsqlRepository : AbstractNpgsqlRepository<ComponentManufacturer, ComponentManufacturerEntity, int>
{
    public ComponentManufacturersNpgsqlRepository(IDbConnectionString connectionString, string tableName = ComponentManufacturersTableName)
        : base(connectionString, tableName)
    {
    }

    
    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(ComponentManufacturerEntity manufacturer) =>
        $"""
            select * from create_component_manufacturer('{manufacturer.Name}')
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(ComponentManufacturerEntity manufacturer) =>
        $"""
            update {TableName}
            set name = '{manufacturer.Name}'
            where id = {manufacturer.Id}
        """;
}