namespace ZCrew.Extensions.Tasks;

/// <inheritdoc/>
internal sealed class TaskCancellationAction : IAsyncAction
{
    private readonly Func<CancellationToken, Task> action;

    internal TaskCancellationAction(Func<CancellationToken, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationAction<T> : IAsyncAction<T>
{
    private readonly Func<T, CancellationToken, Task> action;

    internal TaskCancellationAction(Func<T, CancellationToken, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T arg, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationAction<T1, T2> : IAsyncAction<T1, T2>
{
    private readonly Func<T1, T2, CancellationToken, Task> action;

    internal TaskCancellationAction(Func<T1, T2, CancellationToken, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationAction<T1, T2, T3> : IAsyncAction<T1, T2, T3>
{
    private readonly Func<T1, T2, T3, CancellationToken, Task> action;

    internal TaskCancellationAction(Func<T1, T2, T3, CancellationToken, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationAction<T1, T2, T3, T4> : IAsyncAction<T1, T2, T3, T4>
{
    private readonly Func<T1, T2, T3, T4, CancellationToken, Task> action;

    internal TaskCancellationAction(Func<T1, T2, T3, T4, CancellationToken, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationAction<T1, T2, T3, T4, T5> : IAsyncAction<T1, T2, T3, T4, T5>
{
    private readonly Func<T1, T2, T3, T4, T5, CancellationToken, Task> action;

    internal TaskCancellationAction(Func<T1, T2, T3, T4, T5, CancellationToken, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4, arg5, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationAction<T1, T2, T3, T4, T5, T6> : IAsyncAction<T1, T2, T3, T4, T5, T6>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task> action;

    internal TaskCancellationAction(Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4, arg5, arg6, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationAction<T1, T2, T3, T4, T5, T6, T7> : IAsyncAction<T1, T2, T3, T4, T5, T6, T7>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task> action;

    internal TaskCancellationAction(Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task> action)
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

        return this.action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationAction<T1, T2, T3, T4, T5, T6, T7, T8>
    : IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task> action;

    internal TaskCancellationAction(Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task> action)
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

        return this.action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, token);
    }
}
