﻿using System.Text.Json;
using Server.Models.Auxiliary;
using Server.Models.Domain;
using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class CustomersNpgsqlRepository : AbstractNpgsqlRepository<Customer, int>
{
    public CustomersNpgsqlRepository(string connectionString, string tableName = CustomersTableName)
        : base(connectionString, tableName, DynamicToObjectMapper.MapDynamicToCustomer)
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(Customer customer)
    {
        var serializedContacts = SerializeContacts(customer.Contacts);
        
        var addQueryString = 
            $"""
                select * from create_customer('{customer.Name}', {serializedContacts}, '{customer.RegistrationDate:yyyy-MM-dd}'::date);
            """;

        return addQueryString;
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnEditQueryString(Customer customer)
    {
        var serializedContacts = SerializeContacts(customer.Contacts);
        
        var updateQueryString = 
            $"""
                update {TableName}
                set name = '{customer.Name}',
                    contacts = {serializedContacts}::jsonb,
                    registration_date = '{customer.RegistrationDate:yyyy-MM-dd}'::date
                where id = {customer.Id}
            """;

        return updateQueryString;
    }



    private static string SerializeContacts(Contacts? contacts)
    {
        /*
         * The reason for explicit return of null is 'null' generated by serializer
         * will be interpreted by the database as null json-object, but we need null
         * database value 
         */
        if (contacts is null)
            return "null";

        var serializedContacts = $"'{JsonSerializer.Serialize(contacts)}'";
        return serializedContacts;
    }
}
