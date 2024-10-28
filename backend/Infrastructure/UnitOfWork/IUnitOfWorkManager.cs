using SqlSugar;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWorkManager
{
    SqlSugarScope GetSqlSugarScope();
    int TranCount { get; }
    UnitOfWork CreateUnitOfWork();
    void BeginTran();
    void CommitTran();
    void RollBackTran();
}