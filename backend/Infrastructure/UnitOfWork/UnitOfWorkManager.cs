using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Infrastructure.UnitOfWork;

public class UnitOfWorkManager : IUnitOfWorkManager
{
    private readonly ILogger<UnitOfWorkManager> _logger;
    private readonly ISqlSugarClient _sqlSugarClient;
    private readonly object _locker;
    private int _tranCount { get; set; }
    public readonly ConcurrentStack<string> TranStack = new();
    public UnitOfWorkManager(ILogger<UnitOfWorkManager> logger, ISqlSugarClient sqlSugarClient)
    {
        _logger = logger;
        _sqlSugarClient = sqlSugarClient;
        _locker = new object();
        _tranCount = 0;
    }

    public int TranCount => _tranCount;

    public SqlSugarScope GetSqlSugarScope()
    {
        return _sqlSugarClient as SqlSugarScope;
    }

    public UnitOfWork CreateUnitOfWork()
    {
        var uow = new UnitOfWork
        {
            Logger = _logger,
            Client = _sqlSugarClient,
            Tenant = (ITenant)_sqlSugarClient,
            IsTran = true
        };

        uow.Client.Open();
        uow.Tenant.BeginTran();
        return uow;
    }

    public void BeginTran()
    {
        lock (_locker)
        {
            _tranCount++;
            GetSqlSugarScope().BeginTran();
        }
    }

    public void CommitTran()
    {
        lock (_locker)
        {
            _tranCount--;
            if (_tranCount == 0)
            {
                try
                {
                    GetSqlSugarScope().CommitTran();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    GetSqlSugarScope().RollbackTran();
                }
            }
        }
    }

    public void RollBackTran()
    {
        lock (_locker)
        {
            _tranCount--;
            GetSqlSugarScope().RollbackTran();
        }
    }
}