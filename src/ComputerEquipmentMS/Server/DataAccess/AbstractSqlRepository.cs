using System.Data;
using System.Data.Common;
using Dapper;
using Npgsql;
using Server.Models;

namespace Server.DataAccess;

public abstract class AbstractSqlRepository<TItem, TId> : INpgsqlRepository<TItem, TId>
    where TItem : IIdentifiable<TId>
    where TId : struct
{
    protected readonly string TableName;
    protected readonly DbConnection DbConnection;
    protected IEnumerable<TItem> QueryResultItems = new List<TItem>();

    private int _rowsAffectedByQuery;

    protected string CurrentQueryString { get; private set; } = string.Empty;

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="dbConnection">Database connection instance</param>
    /// <param name="tableName">Database table name, which is related with this repository</param>
    /// <exception cref="NoNullAllowedException">Non of the arguments can be null or empty</exception>
    protected AbstractSqlRepository(DbConnection dbConnection, string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
            throw new NoNullAllowedException("Table name cannot be null or empty");
        
        if (dbConnection is null)
            throw new NoNullAllowedException("Database connection instance cannot be null");

        DbConnection = dbConnection;
        TableName = tableName;
    }
    
    /// <inheritdoc/>
    public ICollection<TItem> GetAll()
    {
        ConstructSelectAllQueryString();
        ExecuteQueryWithResult();
        
        var itemList = QueryResultItems.ToList();
        return itemList;
    }

    /// <inheritdoc/>
    public ICollection<TItem> GetByCriteria(Func<TItem, bool> criteria) =>
        GetAll().Where(criteria).ToList();

    /// <inheritdoc/>
    public TItem? GetById(TId id)
    {
        ConstructSelectByIdQueryString(id);
        ExecuteQueryWithResult();
        
        var desiredItem = QueryResultItems.SingleOrDefault();
        return desiredItem;
    }

    /// <inheritdoc/>
    public TItem Add(TItem item)
    {
        ConstructAddQueryString(item);
        ExecuteQueryWithResult();

        var addSuccessful = QueryResultItems.Single();
        return addSuccessful;
    }

    /// <inheritdoc/>
    public bool Edit(TItem item)
    {
        ConstructEditQueryString(item);
        ExecuteQueryWithoutResult();

        var editSuccessful = _rowsAffectedByQuery > 0;
        return editSuccessful;
    }

    /// <inheritdoc/>
    public bool Remove(TId id)
    {
        ConstructRemoveQueryString(id);
        ExecuteQueryWithoutResult();

        var removeSuccessful = _rowsAffectedByQuery > 0;
        return removeSuccessful;    
    }
    
    /// <inheritdoc/>
    public void Dispose()
    {
        DbConnection.Dispose();
        GC.SuppressFinalize(this);
    }


    
    /// <summary>
    /// Executes SQL-query and stores result in QueryResultItems.<br/>
    /// Query result must be convertable to TItem
    /// </summary>
    /// <remarks>Needs to be overriden for TItem with non-trivial mapping</remarks>>
    protected virtual void ExecuteQueryWithResult() => 
        QueryResultItems = DbConnection.Query<TItem>(CurrentQueryString);
    
    /// <summary>
    /// Constructs insert SQL-query string for specified Item 
    /// </summary>
    /// <param name="item">Item to insert to database</param>
    /// <returns>Constructed SQL-query string</returns>
    /// <remarks>Any returning by query values will be ignored</remarks>
    protected abstract string ConstructAndReturnAddQueryString(TItem item);
    
    /// <summary>
    /// Constructs and returns update SQL-query string for specified Item
    /// </summary>
    /// <param name="item">Item to update in database</param>
    /// <returns>Constructed SQL-query string</returns>
    /// <remarks>Any returning by query values will be ignored</remarks>
    protected abstract string ConstructAndReturnEditQueryString(TItem item);



    /// <summary>
    /// Executes SQL-query that 
    /// </summary>
    private void ExecuteQueryWithoutResult() =>
        _rowsAffectedByQuery = DbConnection.Execute(CurrentQueryString);
    
    private void ConstructSelectAllQueryString() => 
        CurrentQueryString = $"select * from {TableName}";
    
    private void ConstructSelectByIdQueryString(TId id)
    {
        ConstructSelectAllQueryString();
        CurrentQueryString += $" where id = {id}";
    }

    private void ConstructAddQueryString(TItem item) => 
        CurrentQueryString = ConstructAndReturnAddQueryString(item);
    
    private void ConstructEditQueryString(TItem item) => 
        CurrentQueryString = ConstructAndReturnEditQueryString(item);

    private void ConstructRemoveQueryString(TId id) => 
        CurrentQueryString = @$"delete from {TableName} where id = {id}";
}