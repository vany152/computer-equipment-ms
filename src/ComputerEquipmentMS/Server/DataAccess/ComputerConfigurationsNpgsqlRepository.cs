using Server.Models;

using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class ComputerConfigurationsNpgsqlRepository : AbstractNpgsqlRepository<ComputerConfiguration, int>
{
    public ComputerConfigurationsNpgsqlRepository(string connectionString, string tableName = ComputerConfigurationsTableName) 
        : base(connectionString, tableName, MapDynamicToComputerConfiguration)
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnGetAllQueryString() => 
        $"""
            select id,
                   name, 
                   warranty_period,
                   margin,
                   get_configurations_component_ids(id) as component_ids
            from {TableName}
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(ComputerConfiguration configuration) =>
        $$"""
            select * from create_configuration(
                '{{configuration.Name}}', 
                '{{configuration.WarrantyPeriod}}', 
                 {{configuration.Margin}}, 
                '{{{string.Join(", ", configuration.ComponentIds)}}}'::integer[]
            )
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(ComputerConfiguration configuration) =>
        $"""
            update {TableName}
            set name = '{configuration.Name}',
                warranty_period = '{configuration.WarrantyPeriod}',
                margin = {configuration.Margin}
            where id = {configuration.Id}
        """;



    private static ComputerConfiguration MapDynamicToComputerConfiguration(dynamic obj) =>
        new()
        {
            Id = obj.id,
            Name = obj.name,
            WarrantyPeriod = obj.warranty_period,
            Margin = obj.margin,
            ComponentIds = obj.component_ids
        };
}