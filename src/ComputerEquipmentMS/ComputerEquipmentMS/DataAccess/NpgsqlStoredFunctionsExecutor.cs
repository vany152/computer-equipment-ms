﻿using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;
using ComputerEquipmentMS.DataAccess.Entities;
using ComputerEquipmentMS.DataAccess.Util;
using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using Dapper;
using Mapster;
using Npgsql;
using static ComputerEquipmentMS.DataAccess.DynamicToObjectMapper; 

namespace ComputerEquipmentMS.DataAccess;

public class NpgsqlStoredFunctionsExecutor : IDisposable
{
    private readonly NpgsqlConnection _connection; 
    
    public NpgsqlStoredFunctionsExecutor(IDbConnectionString connectionString)
    {
        if (string.IsNullOrEmpty(connectionString.Value))
            throw new NoNullAllowedException("connection string cannot be null or empty");

        _connection = NpgsqlUtil.CreateNpgsqlConnectionWithNodaTime(connectionString.Value);
    }
    
    /*
     * "Configuration" group
     */
    public ICollection<ComputerConfiguration> GetConfigurationsByNamePattern(Regex namePattern)
    {
        var queryString = $"select * from get_configurations_by_name_pattern('{namePattern}')";
        var configurations = ExecuteQuery<ComputerConfiguration, ComputerConfigurationEntity>(queryString, MapDynamicToComputerConfiguration);

        return configurations;
    }

    public decimal CalculateConfigurationCost(int configurationId)
    {
        var queryString = $"select * from calculate_configuration_cost({configurationId})";
        var cost = ExecuteQuery<decimal, decimal>(queryString).SingleOrDefault();

        return cost;
    }

    public ICollection<SalePositionInfo> GetSalesOfConfiguration(int configurationId)
    {
        var queryString = $"select * from get_sales_of_configuration({configurationId})";
        var sales = ExecuteQuery<SalePositionInfo, SalePositionInfoEntity>(queryString, MapDynamicToSalePositionInfo);

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
        var sales = ExecuteQuery<SalePositionInfo, SalePositionInfoEntity>(queryString, MapDynamicToSalePositionInfo);
        
        return sales;
    }

    /*
     * "Sale" group
     */
    public Sale CreateSale(Sale newSale)
    {
        var queryString = 
            $$"""
            select * from create_sale(
                 {{newSale.Customer.Id}},
                '{{newSale.SaleMoment}}'::timestamptz,
                 {{newSale.DiscountPercentage}}::smallint,
                '{{{GetConfigurationIdsAsString()}}}'::integer[],
                '{{{GetConfigurationDiscountsAsString()}}}'::smallint[]
            )
            """;
        var sale = ExecuteQuery<Sale, SaleEntity>(queryString, MapDynamicToSale).Single();

        return sale;



        string GetConfigurationIdsAsString()
        {
            var configurationIds = newSale.SalePositions.Select(sp => sp.Configuration.Id);
            var idsJson = string.Join(",", configurationIds);

            return idsJson;
        }
        
        string GetConfigurationDiscountsAsString()
        {
            var configurationDiscounts = newSale.SalePositions.Select(sp => sp.DiscountPercentage);
            var discountsJson = string.Join(",", configurationDiscounts);

            return discountsJson;
        }
    }

    public decimal CalculateStartingSaleCost(int saleId)
    {
        var queryString = $"select * from calculate_starting_sale_cost({saleId})";
        var cost = ExecuteQuery<decimal, decimal>(queryString).SingleOrDefault();

        return cost;
    }

    public decimal CalculateFinalSaleCost(int saleId)
    {
        var queryString = $"select * from calculate_final_sale_cost({saleId})";
        var cost = ExecuteQuery<decimal, decimal>(queryString).SingleOrDefault();

        return cost;
    }

    public ICollection<Sale> GetSalesForCostInterval(CostInterval interval)
    {
        var queryString = 
            $"""
                select * from get_sales_for_cost_interval(
                    {interval.MinCost}::numeric, 
                    {interval.MaxCost}::numeric)
            """;
        var sales = ExecuteQuery<Sale, SaleEntity>(queryString, MapDynamicToSale);

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
        var sales = ExecuteQuery<Sale, SaleEntity>(queryString, MapDynamicToSale);
        
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
        var sales = ExecuteQuery<Sale, SaleEntity>(queryString, MapDynamicToSale);
        
        return sales;    
    }

    public Sale? GetSale(int saleId)
    {
        var queryString = $"select * from get_sale({saleId})";
        var sale = ExecuteQuery<Sale, SaleEntity>(queryString, MapDynamicToSale).SingleOrDefault();

        return sale;
    }
    
    public ICollection<Sale> GetSales()
    {
        var queryString = "select * from get_sales()";
        var sale = ExecuteQuery<Sale, SaleEntity>(queryString, MapDynamicToSale);

        return sale;
    }    
    
    /*
     * "Customer" group
     */
    public ICollection<Customer> GetCustomersByNamePattern(Regex namePattern)
    {
        var queryString = $"select * from get_customers_by_name_pattern('{namePattern}')";
        var customers = ExecuteQuery<Customer, CustomerEntity>(queryString, MapDynamicToCustomer);

        return customers;
    }
    
    public ICollection<Customer> GetCustomersByContact(ContactType contactType, string contact)
    {
        var queryString = 
            $$"""
                select * from get_customers_by_contact('{"{{contactType}}": "{{contact}}"}'::jsonb)
            """;
        var customers = ExecuteQuery<Customer, CustomerEntity>(queryString, MapDynamicToCustomer);

        return customers;
    }
    
    public ICollection<Sale> GetCustomersPurchases(int id)
    {
        var queryString = $"select * from get_customers_purchases({id})";
        var purchases = ExecuteQuery<Sale, SaleEntity>(queryString, MapDynamicToSale);

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
        var purchases = ExecuteQuery<Sale, SaleEntity>(queryString, MapDynamicToSale);

        return purchases;    
    }
    
    /*
     * "Component" group
     */
    public ICollection<Component> GetComponentsByName(Regex namePattern)
    {
        var queryString = $"select * from get_components_by_name_pattern('{namePattern}')";
        var components = ExecuteQuery<Component, ComponentEntity>(queryString, MapDynamicToComponent);

        return components;    
    }
    
    public ICollection<Component> GetComponentsByCategory(int categoryId)
    {
        var queryString = $"select * from get_components_by_category({categoryId})";
        var components = ExecuteQuery<Component, ComponentEntity>(queryString, MapDynamicToComponent);

        return components;      
    }
    
    public ICollection<Component> GetComponentsByManufacturer(int manufacturerId)
    {
        var queryString = $"select * from get_components_by_manufacturer({manufacturerId})";
        var components = ExecuteQuery<Component, ComponentEntity>(queryString, MapDynamicToComponent);

        return components;      
    }
    
    public ICollection<Component> GetComponentsBySpecifications(ComponentSpecifications specifications)
    {
        var serializedSpecifications = JsonSerializer.Serialize(specifications);
        var queryString = $"select * from get_components_by_specifications('{serializedSpecifications}'::jsonb)";
        var components = ExecuteQuery<Component, ComponentEntity>(queryString, MapDynamicToComponent);

        return components;    
    }
    
    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
    
    

    private ICollection<TItem> ExecuteQuery<TItem, TEntity>(string queryString, Func<dynamic, TEntity>? mapFunction = null)
    {
        var entities = mapFunction is not null
            ? _connection
                .Query(queryString)
                .Select(mapFunction)
            : _connection
                .Query<TEntity>(queryString);
        
        return entities.Adapt<List<TItem>>();
    }
}
