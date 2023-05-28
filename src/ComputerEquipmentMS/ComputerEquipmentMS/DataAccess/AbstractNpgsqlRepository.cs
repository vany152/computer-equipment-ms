using ComputerEquipmentMS.DataAccess.Util;
using ComputerEquipmentMS.Models;

namespace ComputerEquipmentMS.DataAccess;

public abstract class AbstractNpgsqlRepository<TItem, TId> : AbstractSqlRepository<TItem, TId>
    where TItem : IIdentifiable<TId>
    where TId : struct
{
    protected AbstractNpgsqlRepository(string connectionString, string tableName) 
        : base(NpgsqlUtil.CreateNpgsqlConnectionWithNodaTime(connectionString), tableName, null)
    {
    }

    protected AbstractNpgsqlRepository(string connectionString, string tableName, Func<dynamic, TItem>? queryResultMappingFunction)
        : base(NpgsqlUtil.CreateNpgsqlConnectionWithNodaTime(connectionString), tableName, queryResultMappingFunction )
    {
    }
}