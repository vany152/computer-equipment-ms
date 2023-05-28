using ComputerEquipmentMS.Models.Domain;
using static ComputerEquipmentMS.DataAccess.Constants.DbTableNames;

namespace ComputerEquipmentMS.DataAccess;

public class ComputerConfigurationsNpgsqlRepository : AbstractNpgsqlRepository<ComputerConfiguration, int>
{
    public ComputerConfigurationsNpgsqlRepository(string connectionString, string tableName = ComputerConfigurationsTableName) 
        : base(connectionString, tableName, DynamicToObjectMapper.MapDynamicToComputerConfiguration)
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
        $$"""
            select * from edit_configuration(
                 {{configuration.Id}},
                '{{configuration.Name}}', 
                '{{configuration.WarrantyPeriod}}', 
                 {{configuration.Margin}}, 
                '{{{string.Join(", ", configuration.ComponentIds)}}}'::integer[] 
            );
        """;
}