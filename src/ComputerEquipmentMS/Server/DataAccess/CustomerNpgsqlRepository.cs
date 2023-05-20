﻿using System.Text.Json;
using Server.Models;

using static Server.DataAccess.Constants.DbTableNames;

namespace Server.DataAccess;

public class CustomerNpgsqlRepository : AbstractNpgsqlRepository<Customer, int>
{
    public CustomerNpgsqlRepository(string connectionString, string tableName = CustomersTableName)
        : base(connectionString, tableName, MapDynamicToCustomer)
    {
    }

    /// <inheritdoc/>
    protected override string ConstructAndReturnAddQueryString(Customer customer)
    {
        var serializedContacts = SerializeContacts(customer.Contacts);
        
        var addQueryString = 
            $"""
                insert into {TableName} (name, contacts, registration_date)
                values ('{customer.Name}', {serializedContacts}, '{customer.RegistrationDate:u}'::date)
                returning *
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
                    registration_date = '{customer.RegistrationDate:u}'::date
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

    private static Customer MapDynamicToCustomer(dynamic obj)
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
