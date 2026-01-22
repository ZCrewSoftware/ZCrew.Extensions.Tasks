namespace ZCrew.Extensions.Tasks;

/// <inheritdoc/>
internal sealed class SyncCancellationFunc<TResult> : IAsyncFunc<TResult>
{
    private readonly Func<CancellationToken, TResult> func;

    internal SyncCancellationFunc(Func<CancellationToken, TResult> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var funcRef = this.func;
        return Task.Run(() => funcRef(token), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncCancellationFunc<T, TResult> : IAsyncFunc<T, TResult>
{
    private readonly Func<T, CancellationToken, TResult> func;

    internal SyncCancellationFunc(Func<T, CancellationToken, TResult> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T arg, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var funcRef = this.func;
        return Task.Run(() => funcRef(arg, token), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncCancellationFunc<T1, T2, TResult> : IAsyncFunc<T1, T2, TResult>
{
    private readonly Func<T1, T2, CancellationToken, TResult> func;

    internal SyncCancellationFunc(Func<T1, T2, CancellationToken, TResult> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var funcRef = this.func;
        return Task.Run(() => funcRef(arg1, arg2, token), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncCancellationFunc<T1, T2, T3, TResult> : IAsyncFunc<T1, T2, T3, TResult>
{
    private readonly Func<T1, T2, T3, CancellationToken, TResult> func;

    internal SyncCancellationFunc(Func<T1, T2, T3, CancellationToken, TResult> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var funcRef = this.func;
        return Task.Run(() => funcRef(arg1, arg2, arg3, token), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncCancellationFunc<T1, T2, T3, T4, TResult> : IAsyncFunc<T1, T2, T3, T4, TResult>
{
    private readonly Func<T1, T2, T3, T4, CancellationToken, TResult> func;

    internal SyncCancellationFunc(Func<T1, T2, T3, T4, CancellationToken, TResult> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var funcRef = this.func;
        return Task.Run(() => funcRef(arg1, arg2, arg3, arg4, token), token);
    }
}
