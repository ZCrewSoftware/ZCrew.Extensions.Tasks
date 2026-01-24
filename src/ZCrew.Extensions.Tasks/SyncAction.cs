namespace ZCrew.Extensions.Tasks;

/// <inheritdoc/>
internal sealed class SyncAction : IAsyncAction
{
    private readonly Action action;

    internal SyncAction(Action action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return Task.Run(this.action, token);
    }
}

/// <inheritdoc/>
internal sealed class SyncAction<T> : IAsyncAction<T>
{
    private readonly Action<T> action;

    internal SyncAction(Action<T> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T arg, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncAction<T1, T2> : IAsyncAction<T1, T2>
{
    private readonly Action<T1, T2> action;

    internal SyncAction(Action<T1, T2> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncAction<T1, T2, T3> : IAsyncAction<T1, T2, T3>
{
    private readonly Action<T1, T2, T3> action;

    internal SyncAction(Action<T1, T2, T3> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2, arg3), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncAction<T1, T2, T3, T4> : IAsyncAction<T1, T2, T3, T4>
{
    private readonly Action<T1, T2, T3, T4> action;

    internal SyncAction(Action<T1, T2, T3, T4> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2, arg3, arg4), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncAction<T1, T2, T3, T4, T5> : IAsyncAction<T1, T2, T3, T4, T5>
{
    private readonly Action<T1, T2, T3, T4, T5> action;

    internal SyncAction(Action<T1, T2, T3, T4, T5> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2, arg3, arg4, arg5), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncAction<T1, T2, T3, T4, T5, T6> : IAsyncAction<T1, T2, T3, T4, T5, T6>
{
    private readonly Action<T1, T2, T3, T4, T5, T6> action;

    internal SyncAction(Action<T1, T2, T3, T4, T5, T6> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2, arg3, arg4, arg5, arg6), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncAction<T1, T2, T3, T4, T5, T6, T7> : IAsyncAction<T1, T2, T3, T4, T5, T6, T7>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7> action;

    internal SyncAction(Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(
        T1 arg1,
        T2 arg2,
        T3 arg3,
        T4 arg4,
        T5 arg5,
        T6 arg6,
        T7 arg7,
        CancellationToken token = default
    )
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2, arg3, arg4, arg5, arg6, arg7), token);
    }
}

/// <inheritdoc/>
internal sealed class SyncAction<T1, T2, T3, T4, T5, T6, T7, T8> : IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>
{
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8> action;

    internal SyncAction(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(
        T1 arg1,
        T2 arg2,
        T3 arg3,
        T4 arg4,
        T5 arg5,
        T6 arg6,
        T7 arg7,
        T8 arg8,
        CancellationToken token = default
    )
    {
        token.ThrowIfCancellationRequested();

        var actionRef = this.action;
        return Task.Run(() => actionRef(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), token);
    }
}
