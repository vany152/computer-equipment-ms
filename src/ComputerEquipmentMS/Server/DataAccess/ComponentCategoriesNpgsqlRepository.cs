using Server.Models;
using Server.Models.Domain;
using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class ComponentCategoriesNpgsqlRepository : AbstractNpgsqlRepository<ComponentCategory, int>
{
    public ComponentCategoriesNpgsqlRepository(string connectionString, string tableName = ComponentCategoriesTableName)
        : base(connectionString, tableName)
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(ComponentCategory category) =>
        $"""
            select * from create_component_category('{category.Name}')
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(ComponentCategory category) =>
        $"""
            update {TableName}
            set name = '{category.Name}'
            where id = {category.Id}
        """;
}