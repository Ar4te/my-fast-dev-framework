using Serilog;
using SqlSugar;

namespace desktop.DataAccess.UnitOfWork;

public class UnitOfWork : IDisposable
{
    private bool _disposedValue;
    public ISqlSugarClient Client { get; set; }
    public ITenant Tenant { get; set; }
    public bool IsTran { get; set; }
    public bool IsCommit { get; set; }
    public bool IsClose { get; set; }

    public bool Commit()
    {
        if (IsTran && !IsCommit)
        {
            Tenant.CommitTran();
            IsCommit = true;
        }

        if (Client.Ado.Transaction == null && !IsClose)
        {
            Client.Dispose();
            IsClose = true;
        }
        return IsCommit;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
                if (IsTran && !IsCommit)
                {
                    Log.Debug("UnitOfWork RollbackTran");
                    Tenant.RollbackTran();
                }

                if (Client.Ado.Transaction != null && IsClose)
                {
                    return;
                }

                Client.Dispose();
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            _disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~UnitOfWork()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}