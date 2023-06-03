using System.Text.Json;
using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.DataAccess.Entities.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using static ComputerEquipmentMS.DataAccess.Constants.DbTableNames;

namespace ComputerEquipmentMS.DataAccess;

public class ComponentsNpgsqlRepository : AbstractNpgsqlRepository<Component, ComponentEntity, int>
{
    public ComponentsNpgsqlRepository(IDbConnectionString connectionString, string tableName = ComponentsTableName) 
        : base(connectionString, tableName, obj => DynamicToObjectMapper.MapDynamicToComponent(obj))
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(ComponentEntity component)
    {
        var serializedSpecifications = SerializeSpecifications(component.Specifications);

        var addQueryString =  
            $"""
                select * from create_component(
                     {component.ComponentCategoryId}, 
                     {component.ComponentManufacturerId}, 
                    '{component.Name}',
                    '{serializedSpecifications}'::jsonb,
                     {component.Cost}::numeric,
                    '{component.WarrantyPeriod}'::interval
                )
            """;
        
        return addQueryString;
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(ComponentEntity component)
    {
        var serializedSpecifications = SerializeSpecifications(component.Specifications);
        
        var editQueryString =  
            $"""
                update {TableName}
                set component_category_id = {component.ComponentCategoryId},
                    component_manufacturer_id = {component.ComponentManufacturerId},
                    name = '{component.Name}',
                    specifications = '{serializedSpecifications}'::jsonb,
                    cost = {component.Cost}::numeric,
                    warranty_period = '{component.WarrantyPeriod}'::interval
                where id = {component.Id}
            """;
        
        return editQueryString;
    }

    

    private static string SerializeSpecifications(ComponentSpecificationsEntity specifications) =>
        JsonSerializer.Serialize(specifications);
}