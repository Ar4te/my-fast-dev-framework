using System.Collections.Concurrent;

namespace Common.ObjectPool;

public class ObjectPool<T> : IDisposable where T : class
{
    private readonly ConcurrentQueue<T> _objectPool;
    private readonly SemaphoreSlim _semaphoreSlim;
    private readonly Func<T> _objectFactory;
    private readonly Action<T> _onReturn;
    private readonly object _expandLocker;
    private readonly int _expandCount;
    private readonly int _maxPoolSize;
    private int _currentPoolSize;
    private bool _disposed;

    public ObjectPool(Func<T> objectFactory, int initialSize, int maxPoolSize, int expandCount = 10, Action<T>? onReturn = null)
    {
        ArgumentNullException.ThrowIfNull(objectFactory);

        _objectPool = new ConcurrentQueue<T>();
        _objectFactory = objectFactory;
        _onReturn = onReturn ?? (_ => { });
        _maxPoolSize = maxPoolSize;
        _expandCount = expandCount;
        _currentPoolSize = initialSize;
        _semaphoreSlim = new SemaphoreSlim(initialSize, maxPoolSize);
        _expandLocker = new object();
        for (int i = 0; i < initialSize; i++)
        {
            _objectPool.Enqueue(_objectFactory());
        }
    }

    public async Task<T> RentAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(ObjectPool<T>));

        await _semaphoreSlim.WaitAsync(cancellationToken);

        if (_objectPool.TryDequeue(out var obj))
        {
            return obj;
        }

        lock (_expandLocker)
        {
            if (_currentPoolSize < _maxPoolSize)
            {
                ExpandPool();
                if (_objectPool.TryDequeue(out obj))
                {
                    return obj;
                }
            }
        }

        throw new InvalidOperationException("对象池已耗尽，无法继续租用。");
    }

    public void Return(T obj)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(ObjectPool<T>));
        ArgumentNullException.ThrowIfNull(obj);

        _onReturn(obj);
        _objectPool.Enqueue(obj);

        if (_semaphoreSlim.CurrentCount < _currentPoolSize)
        {
            _semaphoreSlim.Release();
        }
    }

    private void ExpandPool()
    {
        int expandCount = Math.Min(_expandCount, _maxPoolSize - _currentPoolSize);
        if (expandCount <= 0) return;

        for (int i = 0; i < expandCount; i++)
        {
            _objectPool.Enqueue(_objectFactory());
        }

        _currentPoolSize += expandCount;
        _semaphoreSlim.Release(expandCount);
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<T, Task<TResult>> operation, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(operation);

        var obj = await RentAsync(cancellationToken);
        try
        {
            var result = await operation(obj);
            return result;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            Return(obj);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        while (_objectPool.TryDequeue(out var obj))
        {
            (obj as IDisposable)?.Dispose();
        }

        _semaphoreSlim.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
