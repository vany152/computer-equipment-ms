using System.Runtime.Serialization;
using System.Text.Json;
using Server.Models;

using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class ComponentsNpgsqlRepository : AbstractNpgsqlRepository<Component, int>
{
    public ComponentsNpgsqlRepository(string connectionString, string tableName = ComponentsTableName) 
        : base(connectionString, tableName, MapDynamicToComponent)
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(Component component)
    {
        var serializedSpecifications = SerializeSpecifications(component.Specifications);
        
        var addQueryString =  
            $"""
                insert into {TableName} (
                    component_category_id, 
                    component_manufacturer_id, 
                    name, 
                    specifications, 
                    cost,
                    warranty_period                                       
                ) values (
                     {component.ComponentCategoryId}, 
                     {component.ComponentManufacturerId}, 
                    '{component.Name}',
                    '{serializedSpecifications}'::jsonb,
                     {component.Cost},
                    '{component.WarrantyPeriod}'::interval
                ) returning *
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

    
    
    private static Component MapDynamicToComponent(dynamic obj)
    {
        var deserializedSpecifications = DeserializeSpecifications(obj.specifications);
        
        var component = new Component
        {
            Id = obj.id,
            ComponentCategoryId = obj.component_category_id,
            ComponentManufacturerId = obj.component_manufacturer_id,
            Name = obj.name,
            Specifications = deserializedSpecifications,
            Cost = obj.cost,
            WarrantyPeriod = obj.warranty_period,
        };

        return component;
    }

    private static string SerializeSpecifications(ComponentSpecifications specifications) =>
        JsonSerializer.Serialize(specifications);

    private static ComponentSpecifications DeserializeSpecifications(string specifications)
    {
        var deserializedSpecifications = JsonSerializer.Deserialize<ComponentSpecifications>(specifications);
        if (deserializedSpecifications is null)
            throw new SerializationException("cannot deserialize component specifications");

        return deserializedSpecifications;
    }
}