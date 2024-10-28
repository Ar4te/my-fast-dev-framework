using SqlSugar;
using System.Linq.Expressions;

namespace desktop.Services.Base;

public interface IBaseService<T> where T : class, new()
{
    ISqlSugarClient Client { get; }
    Task<T?> GetByIdAsync(object id);
    Task<T?> GetByIdAsync<T2>(object id, Expression<Func<T, T2>> expression) where T2 : class, new();
    Task<IEnumerable<T>?> GetByIdsAsync(object[] ids);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>?> GetListAsync(Expression<Func<T, bool>>? expression = null);
    Task<IEnumerable<T2>?> GetListAsync<T2>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T2>> selectExpression);
    Task<int> CreateAsync(T model);
    Task<int> CreateAsync(List<T> models);
    Task<T2?> CreateAndReturnPkAsync<T2>(T model);
    Task<List<T2>?> CreateListAndReturnPksAsync<T2>(List<T> models);
    Task<int> UpdateAsync(T model);
    Task<int> UpdateAsync(List<T> models);
    Task<int> DeleteAsync(object pkVal);
}