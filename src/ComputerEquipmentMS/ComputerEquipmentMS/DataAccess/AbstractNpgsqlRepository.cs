using ComputerEquipmentMS.DataAccess.Util;
using ComputerEquipmentMS.Models;

namespace ComputerEquipmentMS.DataAccess;

public abstract class AbstractNpgsqlRepository<TItem, TEntity, TId> : AbstractSqlRepository<TItem, TEntity, TId>
    where TItem : IIdentifiable<TId>
    where TId : struct
{
    protected AbstractNpgsqlRepository(IDbConnectionString connectionString, string tableName) 
        : base(NpgsqlUtil.CreateNpgsqlConnectionWithNodaTime(connectionString.Value), tableName, null)
    {
    }

    protected AbstractNpgsqlRepository(IDbConnectionString connectionString, string tableName, Func<dynamic, TEntity>? queryResultMappingFunction)
        : base(NpgsqlUtil.CreateNpgsqlConnectionWithNodaTime(connectionString.Value), tableName, queryResultMappingFunction )
    {
    }
}