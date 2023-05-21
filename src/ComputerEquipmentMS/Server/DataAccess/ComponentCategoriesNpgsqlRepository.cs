using Server.Models;

using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class ComponentCategoriesNpgsqlRepository : AbstractNpgsqlRepository<ComponentCategory, int>
{
    public ComponentCategoriesNpgsqlRepository(string connectionString, string tableName = ComponentCategoriesTableName)
        : base(connectionString, tableName, MapDynamicToComponentCategory)
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(ComponentCategory category) =>
        $"""
            insert into {TableName} (name)
            values ('{category.Name}')
            returning *
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(ComponentCategory category) =>
        $"""
            update {TableName}
            set name = '{category.Name}'
            where id = {category.Id}
        """;



    private static ComponentCategory MapDynamicToComponentCategory(dynamic obj) =>
        new()
        {
            Id = obj.id,
            Name = obj.name,
        };
}