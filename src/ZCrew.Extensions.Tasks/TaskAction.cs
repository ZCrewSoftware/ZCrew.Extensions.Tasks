namespace ZCrew.Extensions.Tasks;

/// <inheritdoc/>
internal sealed class TaskAction : IAsyncAction
{
    private readonly Func<Task> action;

    internal TaskAction(Func<Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action();
    }
}

/// <inheritdoc/>
internal sealed class TaskAction<T> : IAsyncAction<T>
{
    private readonly Func<T, Task> action;

    internal TaskAction(Func<T, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T arg, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg);
    }
}

/// <inheritdoc/>
internal sealed class TaskAction<T1, T2> : IAsyncAction<T1, T2>
{
    private readonly Func<T1, T2, Task> action;

    internal TaskAction(Func<T1, T2, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2);
    }
}

/// <inheritdoc/>
internal sealed class TaskAction<T1, T2, T3> : IAsyncAction<T1, T2, T3>
{
    private readonly Func<T1, T2, T3, Task> action;

    internal TaskAction(Func<T1, T2, T3, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3);
    }
}

/// <inheritdoc/>
internal sealed class TaskAction<T1, T2, T3, T4> : IAsyncAction<T1, T2, T3, T4>
{
    private readonly Func<T1, T2, T3, T4, Task> action;

    internal TaskAction(Func<T1, T2, T3, T4, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4);
    }
}

/// <inheritdoc/>
internal sealed class TaskAction<T1, T2, T3, T4, T5> : IAsyncAction<T1, T2, T3, T4, T5>
{
    private readonly Func<T1, T2, T3, T4, T5, Task> action;

    internal TaskAction(Func<T1, T2, T3, T4, T5, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4, arg5);
    }
}

/// <inheritdoc/>
internal sealed class TaskAction<T1, T2, T3, T4, T5, T6> : IAsyncAction<T1, T2, T3, T4, T5, T6>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, Task> action;

    internal TaskAction(Func<T1, T2, T3, T4, T5, T6, Task> action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.action(arg1, arg2, arg3, arg4, arg5, arg6);
    }
}

/// <inheritdoc/>
internal sealed class TaskAction<T1, T2, T3, T4, T5, T6, T7> : IAsyncAction<T1, T2, T3, T4, T5, T6, T7>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, Task> action;

    internal TaskAction(Func<T1, T2, T3, T4, T5, T6, T7, Task> action)
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

        return this.action(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }
}

/// <inheritdoc/>
internal sealed class TaskAction<T1, T2, T3, T4, T5, T6, T7, T8> : IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action;

    internal TaskAction(Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action)
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

        return this.action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }
}
