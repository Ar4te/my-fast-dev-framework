using desktop.DataAccess.Repository;
using SqlSugar;
using System.Linq.Expressions;

namespace desktop.Services.Base;

public class BaseService<T> : IBaseService<T> where T : class, new()
{
    public BaseService(IBaseRepository<T> baseRepo = null)
    {
        BaseRepo = baseRepo;
    }

    public IBaseRepository<T> BaseRepo { get; set; }

    public ISqlSugarClient Client => BaseRepo.Client;

    public Task<T?> GetByIdAsync(object id)
    {
        return BaseRepo.GetByIdAsync(id);
    }

    public Task<T?> GetByIdAsync<T2>(object id, Expression<Func<T, T2>> expression) where T2 : class, new()
    {
        return BaseRepo.GetByIdAsync(id, expression);
    }

    public Task<IEnumerable<T>?> GetByIdsAsync(object[] ids)
    {
        return BaseRepo.GetByIdsAsync(ids);
    }

    public Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        return BaseRepo.GetAsync(expression);
    }

    public Task<IEnumerable<T>?> GetListAsync(Expression<Func<T, bool>>? expression = null)
    {
        return BaseRepo.GetListAsync(expression);
    }

    public Task<IEnumerable<T2>?> GetListAsync<T2>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T2>> selectExpression)
    {
        return BaseRepo.GetListAsync(whereExpression, selectExpression);
    }

    public Task<int> CreateAsync(T model)
    {
        return BaseRepo.CreateAsync(model);
    }

    public Task<int> CreateAsync(List<T> models)
    {
        return BaseRepo.CreateAsync(models);
    }

    public Task<T2?> CreateAndReturnPkAsync<T2>(T model)
    {
        return BaseRepo.CreateAndReturnPkAsync<T2>(model);
    }

    public Task<List<T2>?> CreateListAndReturnPksAsync<T2>(List<T> models)
    {
        return BaseRepo.CreateListAndReturnPksAsync<T2>(models);
    }

    public Task<int> UpdateAsync(T model)
    {
        return BaseRepo.UpdateAsync(model);
    }

    public Task<int> UpdateAsync(List<T> models)
    {
        return BaseRepo.UpdateAsync(models);
    }

    public Task<int> DeleteAsync(object pkVal)
    {
        return BaseRepo.DeleteAsync(pkVal);
    }
}