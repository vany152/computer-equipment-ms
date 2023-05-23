﻿using Server.Models;

using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class ComponentManufacturersNpgsqlRepository : AbstractNpgsqlRepository<ComponentManufacturer, int>
{
    public ComponentManufacturersNpgsqlRepository(string connectionString, string tableName = ComponentManufacturersTableName)
        : base(connectionString, tableName, MapDynamicToComponentManufacturer)
    {
    }

    
    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(ComponentManufacturer manufacturer) =>
        $"""
            insert into {TableName} (name)
            values ('{manufacturer.Name}')
            returning *
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(ComponentManufacturer manufacturer) =>
        $"""
            update {TableName}
            set name = '{manufacturer.Name}'
            where id = {manufacturer.Id}
        """;



    private static ComponentManufacturer MapDynamicToComponentManufacturer(dynamic obj) =>
        new()
        {
            Id = obj.id,
            Name = obj.name,
        };
}