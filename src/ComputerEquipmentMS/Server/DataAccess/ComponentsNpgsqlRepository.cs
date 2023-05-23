using System.Text.Json;
using Server.Models;
using Server.Models.Auxiliary;
using Server.Models.Domain;
using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class ComponentsNpgsqlRepository : AbstractNpgsqlRepository<Component, int>
{
    public ComponentsNpgsqlRepository(string connectionString, string tableName = ComponentsTableName) 
        : base(connectionString, tableName, DynamicToObjectMapper.MapDynamicToComponent)
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(Component component)
    {
        var serializedSpecifications = SerializeSpecifications(component.Specifications);

        var addQueryString =  
            $"""
                select * from create_component(
                     {component.ComponentCategoryId}, 
                     {component.ComponentManufacturerId}, 
                    '{component.Name}',
                    '{serializedSpecifications}'::jsonb,
                     {component.Cost},
                    '{component.WarrantyPeriod}'::interval
                )
            """;
        
        return addQueryString;
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(Component component)
    {
        var serializedSpecifications = SerializeSpecifications(component.Specifications);
        
        var editQueryString =  
            $"""
                update {TableName}
                set component_category_id = {component.ComponentCategoryId},
                    component_manufacturer_id = {component.ComponentManufacturerId},
                    name = '{component.Name}',
                    specifications = '{serializedSpecifications}'::jsonb,
                    cost = {component.Cost},
                    warranty_period = '{component.WarrantyPeriod}'::interval
                where id = {component.Id}
            """;
        
        return editQueryString;
    }

    

    private static string SerializeSpecifications(ComponentSpecifications specifications) =>
        JsonSerializer.Serialize(specifications);
}