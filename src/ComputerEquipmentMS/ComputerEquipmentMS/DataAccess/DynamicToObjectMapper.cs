using System.Runtime.Serialization;
using System.Text.Json;
using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.DataAccess.Entities.Auxiliary;

namespace ComputerEquipmentMS.DataAccess;

public static class DynamicToObjectMapper
{
    public static ComponentEntity MapDynamicToComponent(dynamic obj)
    {
        var deserializedSpecifications = DeserializeSpecifications(obj.specifications);
        
        var component = new ComponentEntity
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
    
    private static ComponentSpecificationsEntity DeserializeSpecifications(string specifications)
    {
        var deserializedSpecifications = JsonSerializer.Deserialize<ComponentSpecificationsEntity>(specifications);
        if (deserializedSpecifications is null)
            throw new SerializationException("cannot deserialize component specifications");

        return deserializedSpecifications;
    }
    
    public static ComputerConfigurationEntity MapDynamicToComputerConfiguration(dynamic obj) =>
        new()
        {
            Id = obj.id,
            Name = obj.name,
            WarrantyPeriod = obj.warranty_period,
            Margin = obj.margin,
            ComponentIds = obj.component_ids
        };

    public static SalePositionInfoEntity MapDynamicToSalePositionInfo(dynamic obj) =>
        new()
        {
            SalePositionId = obj.sale_position_id,
            SaleId = obj.sale_id,
            SaleMoment = obj.sale_moment,
            StartingCost = obj.starting_cost,
            FinalCost = obj.final_cost,
            DiscountPercentage = obj.discount_percentage,
            WarrantyPeriod = obj.warranty_period,
        };
    
    public static SaleEntity MapDynamicToSale(dynamic obj) =>
        new ()
        {
            Id = obj.id,
            CustomerId = obj.customer_id,
            SaleMoment = obj.sale_moment,
            DiscountPercentage = obj.discount_percentage,
            SalePositionIds = obj.sale_position_ids
        };
    
    public static CustomerEntity MapDynamicToCustomer(dynamic obj)
    {
        var deserializedContacts = DeserializeContacts(obj.contacts);
        
        var customer = new CustomerEntity
        {
            Id = obj.id,
            Name = obj.name,
            RegistrationDate = obj.registration_date,
            Contacts = deserializedContacts
        };

        return customer;
    }

    public static SalePositionEntity MapDynamicToSalePosition(dynamic obj) =>
        new()
        {
            Id = obj.id,
            SaleId = obj.sale_id,
            ConfigurationId = obj.computer_configuration_id,
            Cost = obj.cost,
            DiscountPercentage = obj.discount_percentage,
            WarrantyPeriod = obj.warranty_period,
        };

    
    
    private static ContactsEntity? DeserializeContacts(string contacts) =>
        string.IsNullOrEmpty(contacts)
            ? null
            : JsonSerializer.Deserialize<ContactsEntity>(contacts);
}
