namespace ZCrew.Extensions.Tasks;

/// <inheritdoc/>
internal sealed class ValueTaskAction : IAsyncAction
{
    private readonly Func<ValueTask> action;

    internal ValueTaskAction(Func<ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action().AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskAction<T> : IAsyncAction<T>
{
    private readonly Func<T, ValueTask> action;

    internal ValueTaskAction(Func<T, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T arg, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskAction<T1, T2> : IAsyncAction<T1, T2>
{
    private readonly Func<T1, T2, ValueTask> action;

    internal ValueTaskAction(Func<T1, T2, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskAction<T1, T2, T3> : IAsyncAction<T1, T2, T3>
{
    private readonly Func<T1, T2, T3, ValueTask> action;

    internal ValueTaskAction(Func<T1, T2, T3, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskAction<T1, T2, T3, T4> : IAsyncAction<T1, T2, T3, T4>
{
    private readonly Func<T1, T2, T3, T4, ValueTask> action;

    internal ValueTaskAction(Func<T1, T2, T3, T4, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4).AsTask();
    }
}
