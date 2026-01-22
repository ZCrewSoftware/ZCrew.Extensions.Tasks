namespace ZCrew.Extensions.Tasks;

/// <inheritdoc/>
internal sealed class SyncCancellationAction : IAsyncAction
{
    private readonly Action<CancellationToken> action;

    internal SyncCancellationAction(Action<CancellationToken> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(token), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncCancellationAction<T> : IAsyncAction<T>
{
    private readonly Action<T, CancellationToken> action;

    internal SyncCancellationAction(Action<T, CancellationToken> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T arg, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg, token), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncCancellationAction<T1, T2> : IAsyncAction<T1, T2>
{
    private readonly Action<T1, T2, CancellationToken> action;

    internal SyncCancellationAction(Action<T1, T2, CancellationToken> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2, token), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncCancellationAction<T1, T2, T3> : IAsyncAction<T1, T2, T3>
{
    private readonly Action<T1, T2, T3, CancellationToken> action;

    internal SyncCancellationAction(Action<T1, T2, T3, CancellationToken> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2, arg3, token), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncCancellationAction<T1, T2, T3, T4> : IAsyncAction<T1, T2, T3, T4>
{
    private readonly Action<T1, T2, T3, T4, CancellationToken> action;

    internal SyncCancellationAction(Action<T1, T2, T3, T4, CancellationToken> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2, arg3, arg4, token), token);
    }
}
