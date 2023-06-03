using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using static ComputerEquipmentMS.DataAccess.Constants.DbTableNames;

namespace ComputerEquipmentMS.DataAccess;

public class ComponentCategoriesNpgsqlRepository : AbstractNpgsqlRepository<ComponentCategory, ComponentCategoryEntity, int>
{
    public ComponentCategoriesNpgsqlRepository(IDbConnectionString connectionString, string tableName = ComponentCategoriesTableName)
        : base(connectionString, tableName)
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(ComponentCategoryEntity category) =>
        $"""
            select * from create_component_category('{category.Name}')
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(ComponentCategoryEntity category) =>
        $"""
            update {TableName}
            set name = '{category.Name}'
            where id = {category.Id}
        """;
}