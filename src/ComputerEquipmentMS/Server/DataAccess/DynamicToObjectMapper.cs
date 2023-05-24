﻿using System.Runtime.Serialization;
using System.Text.Json;
using Server.Models.Auxiliary;
using Server.Models.Domain;

namespace Server.DataAccess;

public static class DynamicToObjectMapper
{
    public static Component MapDynamicToComponent(dynamic obj)
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
    
    private static ComponentSpecifications DeserializeSpecifications(string specifications)
    {
        var deserializedSpecifications = JsonSerializer.Deserialize<ComponentSpecifications>(specifications);
        if (deserializedSpecifications is null)
            throw new SerializationException("cannot deserialize component specifications");

        return deserializedSpecifications;
    }
    
    public static ComputerConfiguration MapDynamicToComputerConfiguration(dynamic obj) =>
        new()
        {
            Id = obj.id,
            Name = obj.name,
            WarrantyPeriod = obj.warranty_period,
            Margin = obj.margin,
            ComponentIds = obj.component_ids
        };

    public static SalePositionInfo MapDynamicToSalePositionInfo(dynamic obj) =>
        new()
        {
            SalePositionId = obj.sale_position_id,
            SaleMoment = obj.sale_moment,
            StartingCost = obj.starting_cost,
            FinalCost = obj.final_cost,
            DiscountPercentage = obj.discount_percentage,
            WarrantyPeriod = obj.warranty_period,
        };
    
    public static Sale MapDynamicToSale(dynamic obj) =>
        new ()
        {
            Id = obj.id,
            CustomerId = obj.customer_id,
            SaleMoment = obj.sale_moment,
            DiscountPercentage = obj.discount_percentage,
            SalePositionIds = obj.sale_position_ids
        };
    
    public static Customer MapDynamicToCustomer(dynamic obj)
    {
        var deserializedContacts = DeserializeContacts(obj.contacts);
        
        var customer = new Customer
        {
            Id = obj.id,
            Name = obj.name,
            RegistrationDate = obj.registration_date,
            Contacts = deserializedContacts
        };

        return customer;
    }

    private static Contacts? DeserializeContacts(string contacts) =>
        string.IsNullOrEmpty(contacts)
            ? null
            : JsonSerializer.Deserialize<Contacts>(contacts);
}
