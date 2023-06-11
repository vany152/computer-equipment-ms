using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.Models.Domain;
using static ComputerEquipmentMS.Constants.DbTableNames;

namespace ComputerEquipmentMS.DataAccess;

public class ComputerConfigurationsNpgsqlRepository : AbstractNpgsqlRepository<ComputerConfiguration, ComputerConfigurationEntity, int>
{
    public ComputerConfigurationsNpgsqlRepository(IDbConnectionString connectionString, string tableName = ComputerConfigurationsTableName) 
        : base(connectionString, tableName, obj => DynamicToObjectMapper.MapDynamicToComputerConfiguration(obj))
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
    protected override string ConstructAndReturnAddQueryString(ComputerConfigurationEntity configuration) =>
        $$"""
            select * from create_configuration(
                '{{configuration.Name}}', 
                '{{configuration.WarrantyPeriod}}', 
                 {{configuration.Margin}}, 
                '{{{string.Join(", ", configuration.ComponentIds)}}}'::integer[]
            )
        """;

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(ComputerConfigurationEntity configuration) =>
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