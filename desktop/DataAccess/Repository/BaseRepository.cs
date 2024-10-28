using System.Linq.Expressions;
using System.Reflection;
using desktop.DataAccess.UnitOfWork;
using desktop.Exntesions;
using SqlSugar;

namespace desktop.DataAccess.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
{
    private readonly SqlSugarScope _sqlSugarScope;

    private ISqlSugarClient SqlSugarClient
    {
        get
        {
            ISqlSugarClient db = _sqlSugarScope;

            if (typeof(T).GetCustomAttribute<DatabaseConfigIdAttribute>() is { } dc && !string.IsNullOrEmpty(dc.ConfigId) && !dc.ConfigId.Equals(db.CurrentConnectionConfig.ConfigId.ToString(), StringComparison.Ordinal))
            {
                db = _sqlSugarScope.GetConnectionScope(dc.ConfigId);
            }

            return db;
        }
    }

    public ISqlSugarClient Client => SqlSugarClient;

    public BaseRepository(IUnitOfWorkManager unitOfWorkManager)
    {
        _sqlSugarScope = unitOfWorkManager.GetSqlSugarScope();
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await SqlSugarClient.Queryable<T>().In(id).SingleAsync();
    }

    public async Task<T?> GetByIdAsync<T2>(object id, Expression<Func<T, T2>> expression) where T2 : class, new()
    {
        return await SqlSugarClient.Queryable<T>()
            .In(id)
            .Includes(expression)
            .SingleAsync();
    }

    public async Task<T?> GetByIdAsync<T1, T2, T3>(T1 id, Expression<Func<T, T2>> includeExpression, Expression<Func<T2, T3>> includeOrderExpression)
        where T2 : class, new()
        where T3 : class
    {
        var t = await SqlSugarClient.Queryable<T>()
            .In(id)
            .Includes(includeExpression, includeOrderExpression)
            .SingleAsync();
        return t;
    }

    public async Task<IEnumerable<T>?> GetByIdsAsync(object[] ids)
    {
        return await SqlSugarClient.Queryable<T>().In(ids).ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        return await SqlSugarClient.Queryable<T>().FirstAsync(expression);
    }

    public async Task<IEnumerable<T>?> GetListAsync(Expression<Func<T, bool>>? expression = null)
    {
        return await SqlSugarClient.Queryable<T>().WhereIF(expression is not null, expression).ToListAsync();
    }

    public async Task<IEnumerable<T2>?> GetListAsync<T2>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T2>> selectExpression)
    {
        return await SqlSugarClient.Queryable<T>()
            .WhereIF(whereExpression is not null, whereExpression)
            .Select(selectExpression)
            .ToListAsync();
    }

    public async Task<int> CreateAsync(T model)
    {
        return await SqlSugarClient.Insertable(model).ExecuteCommandAsync();
    }

    public Task<int> CreateAsync(List<T> models)
    {
        return SqlSugarClient.Insertable(models).ExecuteCommandAsync();
    }

    public async Task<T2?> CreateAndReturnPkAsync<T2>(T model)
    {
        var pks = await SqlSugarClient.Insertable(model).ExecuteReturnPkListAsync<T2>();
        return pks.Count == 0 ? default : pks[0];
    }

    public Task<List<T2>?> CreateListAndReturnPksAsync<T2>(List<T> models)
    {
        return SqlSugarClient.Insertable(models).ExecuteReturnPkListAsync<T2>();
    }

    public Task<int> UpdateAsync(T model)
    {
        return SqlSugarClient.Updateable(model).ExecuteCommandAsync();
    }

    public Task<int> UpdateAsync(List<T> models)
    {
        return SqlSugarClient.Updateable(models).ExecuteCommandAsync();
    }

    public Task<int> DeleteAsync(object pkVal)
    {
        return SqlSugarClient.Deleteable<T>(pkVal).ExecuteCommandAsync();
    }
}
