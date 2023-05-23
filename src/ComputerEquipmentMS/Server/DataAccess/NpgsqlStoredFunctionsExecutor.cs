﻿using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;
using Dapper;
using Npgsql;
using Server.Models;
using Server.Models.Auxiliary;
using Server.Models.Domain;
using static Server.DataAccess.DynamicToObjectMapper; 

namespace Server.DataAccess;

public class NpgsqlStoredFunctionsExecutor
{
    private readonly NpgsqlConnection _connection; 
    
    public NpgsqlStoredFunctionsExecutor(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new NoNullAllowedException("connection string cannot be null or empty");

        _connection = new NpgsqlConnection(connectionString);
    }
    
    /*
     * "Configuration" group
     */
    public ICollection<ComputerConfiguration> GetConfigurationsByNamePattern(Regex namePattern)
    {
        var queryString = $"select * from get_configurations_by_name_pattern('{namePattern}')";
        var configurations = ExecuteQuery(queryString, MapDynamicToComputerConfiguration);

        return configurations;
    }

    public decimal CalculateConfigurationCost(int configurationId)
    {
        var queryString = $"select * from calculate_configuration_cost({configurationId})";
        var cost = ExecuteQuery<decimal?>(queryString).SingleOrDefault();

        if (cost is null)
            throw new InvalidOperationException($"computer configuration with id = {configurationId} not found");

        return cost.Value;
    }

    public ICollection<SalePositionInfo> GetSalesOfConfiguration(int configurationId)
    {
        var queryString = $"select * from get_sales_of_configuration({configurationId})";
        var sales = ExecuteQuery(queryString, MapDynamicToSalePositionInfo);

        return sales;
    }
    
    public ICollection<SalePositionInfo> GetSalesOfConfigurationForTimeInterval(int configurationId, TimeInterval interval)
    {
        var queryString = 
            $"""
                select * from get_sales_of_configuration_for_time_interval(
                     {configurationId}, 
                    '{interval.From}'::timestamptz,
                    '{interval.To}'::timestamptz
                )  
            """;
        var sales = ExecuteQuery(queryString, MapDynamicToSalePositionInfo);
        
        return sales;
    }

    /*
     * "Sale" group
     */
    public decimal CalculateSaleCost(int saleId)
    {
        var queryString = $"select * from calculate_sale_cost({saleId})";
        var cost = ExecuteQuery<decimal?>(queryString).SingleOrDefault();

        if (cost is null)
            throw new InvalidOperationException("sale not found");

        return cost.Value;
    }

    public ICollection<Sale> GetSalesForCostInterval(CostInterval interval)
    {
        var queryString = 
            $"""
                select * from get_sales_for_cost_interval(
                    {interval.MinCost}::numeric, 
                    {interval.MaxCost}::numeric)
            """;
        var sales = ExecuteQuery(queryString, MapDynamicToSale);

        return sales;
    }

    public ICollection<Sale> GetSalesForTimeInterval(TimeInterval interval)
    {
        var queryString = 
            $"""
                select * from get_sales_for_time_interval(
                    '{interval.From}'::timestamptz, 
                    '{interval.To}'::timestamptz
                )
            """;
        var sales = ExecuteQuery(queryString, MapDynamicToSale);
        
        return sales;    
    }
    
    public ICollection<Sale> GetSalesForTimeAndCostInterval(TimeInterval timeInterval, CostInterval costInterval)
    {
        var queryString = 
            $"""
                select * from get_sales_for_time_and_cost_interval(
                    '{timeInterval.From}'::timestamptz, 
                    '{timeInterval.To}'::timestamptz,
                     {costInterval.MinCost}::numeric,
                     {costInterval.MaxCost}::numeric
                )
            """;
        var sales = ExecuteQuery(queryString, MapDynamicToSale);
        
        return sales;    
    }
    
    /*
     * "Customer" group
     */
    public ICollection<Customer> GetCustomersByNamePattern(Regex namePattern)
    {
        var queryString = $"select * from get_customers_by_name_pattern('{namePattern}')";
        var customers = 
            _connection
                .Query(queryString)
                .Select(MapDynamicToCustomer)
                .ToList();

        return customers;
    }
    
    public ICollection<Customer> GetCustomersByContact(ContactType contactType, string contact)
    {
        var queryString = 
            $$"""
                select * from get_customers_by_contact('{"{{contactType}}": "{{contact}}"}'::jsonb)
            """;
        var customers = 
            _connection
                .Query(queryString)
                .Select(MapDynamicToCustomer)
                .ToList();

        return customers;
    }
    
    public ICollection<Sale> GetCustomersPurchases(int id)
    {
        var queryString = $"select * from get_customers_purchases({id})";
        var purchases = ExecuteQuery(queryString, MapDynamicToSale);

        return purchases;
    }
    
    public ICollection<Sale> GetCustomersPurchases(int id, TimeInterval interval)
    {
        var queryString = 
            $"""
                select * from get_customers_purchases(
                     {id}, 
                    '{interval.From}'::timestamptz, 
                    '{interval.To}'::timestamptz
                )
            """;
        var purchases = ExecuteQuery(queryString, MapDynamicToSale);

        return purchases;    
    }
    
    /*
     * "Component" group
     */
    public ICollection<Component> GetComponentsByName(Regex namePattern)
    {
        var queryString = $"select * from get_components_by_name_pattern('{namePattern}')";
        var components = 
            _connection
                .Query(queryString)
                .Select(MapDynamicToComponent)
                .ToList();

        return components;    
    }
    
    public ICollection<Component> GetComponentsByCategory(int categoryId)
    {
        var queryString = $"select * from get_components_by_category({categoryId})";
        var components = ExecuteQuery(queryString, MapDynamicToComponent);

        return components;      
    }
    
    public ICollection<Component> GetComponentsByManufacturer(int manufacturerId)
    {
        var queryString = $"select * from get_components_by_manufacturer({manufacturerId})";
        var components = ExecuteQuery(queryString, MapDynamicToComponent);

        return components;      
    }
    
    public ICollection<Component> GetComponentsBySpecifications(ComponentSpecifications specifications)
    {
        var serializedSpecifications = JsonSerializer.Serialize(specifications);
        var queryString = $"select * from get_components_by_specifications('{serializedSpecifications}'::jsonb)";
        var components = ExecuteQuery(queryString, MapDynamicToComponent);

        return components;    
    }
    
    

    private ICollection<T> ExecuteQuery<T>(string queryString, Func<dynamic, T>? mapFunction = null) =>
        mapFunction is not null
            ? _connection
                .Query(queryString)
                .Select(mapFunction)
                .ToList()
            : _connection
                .Query<T>(queryString)
                .ToList();
}
