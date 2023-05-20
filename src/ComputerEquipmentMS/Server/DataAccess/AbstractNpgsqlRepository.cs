using System.Data.Common;
using Npgsql;
using Server.Models;

namespace Server.DataAccess;

public abstract class AbstractNpgsqlRepository<TItem, TId> : AbstractSqlRepository<TItem, TId>
    where TItem : IIdentifiable<TId>
    where TId : struct
{
    protected AbstractNpgsqlRepository(string connectionString, string tableName) 
        : base(new NpgsqlConnection(connectionString), tableName, null)
    {
    }

    protected AbstractNpgsqlRepository(string connectionString, string tableName, Func<dynamic, TItem>? queryResultMappingFunction)
        : base(new NpgsqlConnection(connectionString), tableName, queryResultMappingFunction )
    {
    }
}