using System.Data;
using System.Data.Common;
using ComputerEquipmentMS.Models;
using Dapper;
using Mapster;

namespace ComputerEquipmentMS.DataAccess;

public abstract class AbstractSqlRepository<TItem, TEntity, TId> : ISqlRepository<TItem, TId>
    where TItem : IIdentifiable<TId>
    where TId : struct
{
    private readonly DbConnection _dbConnection;
    protected readonly string TableName;
    private string _currentQueryString  = string.Empty;
    private IEnumerable<TEntity> _currentQueryResultItems = new List<TEntity>();

    /// <summary>
    /// Amount of rows affected by executing SQL-query that does not return result
    /// </summary>
    private int _amountOfRowsAffectedByQuery;


    /// <summary>
    /// Function for mapping SQL-query result to TItem type with non-trivial mapping way
    /// </summary>
    /// <remarks>If null, SQL-query result maps directly to TItem type</remarks>
    private readonly Func<dynamic, TEntity>? _queryResultMappingFunction;
    
    

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="dbConnection">Database connection instance</param>
    /// <param name="tableName">Database table name, which is related with this repository</param>
    /// <exception cref="NoNullAllowedException">Non of the arguments can be null or empty</exception>
    protected AbstractSqlRepository(DbConnection dbConnection, string tableName)
        : this(dbConnection, tableName, null)
    {
    }

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="dbConnection">Database connection instance</param>
    /// <param name="tableName">Database table name, which is related with this repository</param>
    /// <param name="queryResultMappingFunction">Function that maps dynamic query result to TItem</param>
    /// <exception cref="NoNullAllowedException">Non of the arguments can be null or empty</exception>
    protected AbstractSqlRepository(DbConnection dbConnection, string tableName, Func<dynamic, TEntity>? queryResultMappingFunction)
    {
        if (string.IsNullOrEmpty(tableName))
            throw new NoNullAllowedException("Table name cannot be null or empty");
        
        if (dbConnection is null)
            throw new NoNullAllowedException("Database connection instance cannot be null");

        _dbConnection = dbConnection;
        TableName = tableName;
        _queryResultMappingFunction = queryResultMappingFunction;
    }

    /// <inheritdoc/>
    public ICollection<TItem> GetAll()
    {
        ConstructGetAllQueryString();
        ExecuteQueryWithResult();
        
        var entityList = _currentQueryResultItems.ToList();
        return entityList.Adapt<List<TItem>>();
    }

    /// <inheritdoc/>
    public ICollection<TItem> GetByCriteria(Func<TItem, bool> criteria) =>
        GetAll().Where(criteria).ToList();

    /// <inheritdoc/>
    public TItem? GetById(TId id)
    {
        ConstructGetByIdQueryString(id);
        ExecuteQueryWithResult();
        
        var desiredEntity = _currentQueryResultItems.SingleOrDefault();
        return desiredEntity is not null
            ? desiredEntity.Adapt<TItem>()
            : default;
    }

    /// <inheritdoc/>
    public TItem Add(TItem item)
    {
        var entity = item.Adapt<TEntity>();
        ConstructAddQueryString(entity);
        ExecuteQueryWithResult();

        var newEntity = _currentQueryResultItems.Single();
        if (newEntity is null)
            throw new NoNullAllowedException("cannot have null value after add to database");
        
        return newEntity.Adapt<TItem>();
    }

    /// <inheritdoc/>
    public bool Edit(TItem item)
    {
        var entity = item.Adapt<TEntity>();
        ConstructEditQueryString(entity);
        ExecuteQueryWithoutResult();

        var editSuccessful = _amountOfRowsAffectedByQuery > 0;
        return editSuccessful;
    }

    /// <inheritdoc/>
    public bool Remove(TId id)
    {
        ConstructRemoveQueryString(id);
        ExecuteQueryWithoutResult();

        var removeSuccessful = _amountOfRowsAffectedByQuery > 0;
        return removeSuccessful;    
    }
    
    /// <inheritdoc/>
    public void Dispose()
    {
        _dbConnection.Dispose();
        GC.SuppressFinalize(this);
    }



    /// <summary>
    /// Executes SQL-query and stores result in QueryResultItems.<br/>
    /// </summary>
    /// <remarks>
    /// The QueryResultMappingFunction used if specified,
    /// otherwise result is mapped directly to TItem
    /// </remarks>
    private void ExecuteQueryWithResult() =>
        _currentQueryResultItems = _queryResultMappingFunction is not null
            ? _dbConnection.Query(_currentQueryString).Select(_queryResultMappingFunction)
            : _dbConnection.Query<TEntity>(_currentQueryString);
    
    /// <summary>
    /// Constructs select SQL-query string for retrieving all values from table 
    /// </summary>
    /// <returns>Constructed SQL-query string</returns>
    protected virtual string ConstructAndReturnGetAllQueryString() =>
        $"select * from {TableName}";
    
    /// <summary>
    /// Constructs select SQL-query string for retrieving values with specified Id from table
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Constructed SQL-query string</returns>
    protected virtual string ConstructAndReturnGetByIdQueryString(TId id) =>
        $"{ConstructAndReturnGetAllQueryString()} where id = {id}";
    
    /// <summary>
    /// Constructs insert SQL-query string for specified Item 
    /// </summary>
    /// <param name="item">Item to insert to database</param>
    /// <returns>Constructed SQL-query string</returns>
    /// <remarks>Any returning by query values will be ignored</remarks>
    protected abstract string ConstructAndReturnAddQueryString(TEntity item);
    
    /// <summary>
    /// Constructs and returns update SQL-query string for specified Item
    /// </summary>
    /// <param name="item">Item to update in database</param>
    /// <returns>Constructed SQL-query string</returns>
    /// <remarks>Any returning by query values will be ignored</remarks>
    protected abstract string ConstructAndReturnEditQueryString(TEntity item);



    /// <summary>
    /// Executes SQL-query that 
    /// </summary>
    private void ExecuteQueryWithoutResult() =>
        _amountOfRowsAffectedByQuery = _dbConnection.Execute(_currentQueryString);
    
    private void ConstructGetAllQueryString() => 
        _currentQueryString = ConstructAndReturnGetAllQueryString();
    
    private void ConstructGetByIdQueryString(TId id) => 
        _currentQueryString = ConstructAndReturnGetByIdQueryString(id);

    private void ConstructAddQueryString(TEntity item) => 
        _currentQueryString = ConstructAndReturnAddQueryString(item);
    
    private void ConstructEditQueryString(TEntity item) => 
        _currentQueryString = ConstructAndReturnEditQueryString(item);

    private void ConstructRemoveQueryString(TId id) => 
        _currentQueryString = @$"delete from {TableName} where id = {id}";
}