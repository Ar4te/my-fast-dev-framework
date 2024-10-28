using System.Linq.Expressions;
using SqlSugar;

namespace Domain.IRepository;

public interface IBaseRepository<T> where T : class
{
    ISqlSugarClient Client { get; }
    Task<T?> GetByIdAsync(object id);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T2">子类</typeparam>
    /// <param name="id"></param>
    /// <param name="expression">导航查询</param>
    /// <returns></returns>
    Task<T?> GetByIdAsync<T2>(object id, Expression<Func<T, T2>> expression)
        where T2 : class, new();

    Task<T?> GetByIdAsync<T1, T2, T3>(T1 id, Expression<Func<T, T2>> includeExpression, Expression<Func<T2, T3>> includeOrderExpression)
        where T2 : class, new()
        where T3 : class;

    Task<T?> GetAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>?> GetByIdsAsync(object[] ids);
    Task<IEnumerable<T>?> GetListAsync(Expression<Func<T, bool>>? expression = null);
    Task<IEnumerable<T2>?> GetListAsync<T2>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T2>> selectExpression);
    Task<int> CreateAsync(T model);
    Task<T2?> CreateAndReturnPkAsync<T2>(T model);
    Task<List<T2>?> CreateListAndReturnPksAsync<T2>(List<T> models);
    Task<int> UpdateAsync(T model);
    Task<int> UpdateAsync(List<T> models);
    Task<int> DeleteAsync(object pkVal);
    Task<int> CreateAsync(List<T> models);
}