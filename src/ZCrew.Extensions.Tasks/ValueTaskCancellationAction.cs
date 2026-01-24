namespace ZCrew.Extensions.Tasks;

/// <inheritdoc/>
internal sealed class ValueTaskCancellationAction : IAsyncAction
{
    private readonly Func<CancellationToken, ValueTask> action;

    internal ValueTaskCancellationAction(Func<CancellationToken, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(token).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskCancellationAction<T> : IAsyncAction<T>
{
    private readonly Func<T, CancellationToken, ValueTask> action;

    internal ValueTaskCancellationAction(Func<T, CancellationToken, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T arg, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg, token).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskCancellationAction<T1, T2> : IAsyncAction<T1, T2>
{
    private readonly Func<T1, T2, CancellationToken, ValueTask> action;

    internal ValueTaskCancellationAction(Func<T1, T2, CancellationToken, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, token).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskCancellationAction<T1, T2, T3> : IAsyncAction<T1, T2, T3>
{
    private readonly Func<T1, T2, T3, CancellationToken, ValueTask> action;

    internal ValueTaskCancellationAction(Func<T1, T2, T3, CancellationToken, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, token).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskCancellationAction<T1, T2, T3, T4> : IAsyncAction<T1, T2, T3, T4>
{
    private readonly Func<T1, T2, T3, T4, CancellationToken, ValueTask> action;

    internal ValueTaskCancellationAction(Func<T1, T2, T3, T4, CancellationToken, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4, token).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskCancellationAction<T1, T2, T3, T4, T5> : IAsyncAction<T1, T2, T3, T4, T5>
{
    private readonly Func<T1, T2, T3, T4, T5, CancellationToken, ValueTask> action;

    internal ValueTaskCancellationAction(Func<T1, T2, T3, T4, T5, CancellationToken, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4, arg5, token).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskCancellationAction<T1, T2, T3, T4, T5, T6> : IAsyncAction<T1, T2, T3, T4, T5, T6>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, CancellationToken, ValueTask> action;

    internal ValueTaskCancellationAction(Func<T1, T2, T3, T4, T5, T6, CancellationToken, ValueTask> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4, arg5, arg6, token).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskCancellationAction<T1, T2, T3, T4, T5, T6, T7> : IAsyncAction<T1, T2, T3, T4, T5, T6, T7>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, ValueTask> action;

    internal ValueTaskCancellationAction(Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, ValueTask> action)
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

        return this.action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, token).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskCancellationAction<T1, T2, T3, T4, T5, T6, T7, T8>
    : IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, ValueTask> action;

    internal ValueTaskCancellationAction(Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, ValueTask> action)
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

        return this.action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, token).AsTask();
    }
}
