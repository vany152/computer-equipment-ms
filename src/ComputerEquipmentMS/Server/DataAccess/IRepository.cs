using Server.Models;

namespace Server.DataAccess;

/// <summary>
/// Represents a set of items with unique Ids
/// </summary>
/// <typeparam name="TItem">Item's type</typeparam>
/// <typeparam name="TId">Item's Id type</typeparam>
public interface IRepository<TItem, in TId>
    where TItem : IIdentifiable<TId>
    where TId : struct
{
    /// <summary>
    /// Returns all items contained by repository
    /// </summary>
    ICollection<TItem> GetAll();

    /// <summary>
    /// Returns all items satisfying specified criteria
    /// </summary>
    /// <param name="criteria">Search criteria</param>
    /// <returns>Set of found items</returns>
    ICollection<TItem> GetByCriteria(Func<TItem, bool> criteria);

    /// <summary>
    /// Returns first occurrence of item with specified Id 
    /// </summary>
    /// <param name="id">Id of searched item</param>
    /// <returns>Item if found, null otherwise</returns>
    TItem? GetById(TId id);

    /// <summary>
    /// Adds item to the repository
    /// </summary>
    /// <param name="item">Item to save</param>
    /// <returns>Saved to database item</returns>
    TItem Add(TItem item);
    
    /// <summary>
    /// Replaces item with given, having same Id as given 
    /// </summary>
    /// <param name="item">Item to replace existing</param>
    /// <returns>true if Item was replaced successfully, otherwise false</returns>
    bool Edit(TItem item);
    
    /// <summary>
    /// Removes item with specified Id
    /// </summary>
    /// <param name="id">Id of item to remove</param>
    /// <returns>true if item was removed successfully, otherwise false</returns>
    bool Remove(TId id);
}
