using Server.Models;

namespace Server.DataAccess;

public interface ISqlRepository<TItem, in TId> : IRepository<TItem, TId>, IDisposable
    where TItem : IIdentifiable<TId>
    where TId : struct
{
    
}