using Server.Models;

namespace Server.DataAccess;

public interface INpgsqlRepository<TItem, in TId> : IRepository<TItem, TId>, IDisposable
    where TItem : IIdentifiable<TId>
    where TId : struct
{
    
}