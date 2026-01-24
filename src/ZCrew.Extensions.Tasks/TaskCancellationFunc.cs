namespace ZCrew.Extensions.Tasks;

/// <inheritdoc/>
internal sealed class TaskCancellationFunc<TResult> : IAsyncFunc<TResult>
{
    private readonly Func<CancellationToken, Task<TResult>> func;

    internal TaskCancellationFunc(Func<CancellationToken, Task<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationFunc<T, TResult> : IAsyncFunc<T, TResult>
{
    private readonly Func<T, CancellationToken, Task<TResult>> func;

    internal TaskCancellationFunc(Func<T, CancellationToken, Task<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T arg, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationFunc<T1, T2, TResult> : IAsyncFunc<T1, T2, TResult>
{
    private readonly Func<T1, T2, CancellationToken, Task<TResult>> func;

    internal TaskCancellationFunc(Func<T1, T2, CancellationToken, Task<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg1, arg2, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationFunc<T1, T2, T3, TResult> : IAsyncFunc<T1, T2, T3, TResult>
{
    private readonly Func<T1, T2, T3, CancellationToken, Task<TResult>> func;

    internal TaskCancellationFunc(Func<T1, T2, T3, CancellationToken, Task<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg1, arg2, arg3, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationFunc<T1, T2, T3, T4, TResult> : IAsyncFunc<T1, T2, T3, T4, TResult>
{
    private readonly Func<T1, T2, T3, T4, CancellationToken, Task<TResult>> func;

    internal TaskCancellationFunc(Func<T1, T2, T3, T4, CancellationToken, Task<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg1, arg2, arg3, arg4, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationFunc<T1, T2, T3, T4, T5, TResult> : IAsyncFunc<T1, T2, T3, T4, T5, TResult>
{
    private readonly Func<T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> func;

    internal TaskCancellationFunc(Func<T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg1, arg2, arg3, arg4, arg5, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationFunc<T1, T2, T3, T4, T5, T6, TResult>
    : IAsyncFunc<T1, T2, T3, T4, T5, T6, TResult>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task<TResult>> func;

    internal TaskCancellationFunc(Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(
        T1 arg1,
        T2 arg2,
        T3 arg3,
        T4 arg4,
        T5 arg5,
        T6 arg6,
        CancellationToken token = default
    )
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg1, arg2, arg3, arg4, arg5, arg6, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationFunc<T1, T2, T3, T4, T5, T6, T7, TResult>
    : IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task<TResult>> func;

    internal TaskCancellationFunc(Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(
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

        return this.func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, token);
    }
}

/// <inheritdoc/>
internal sealed class TaskCancellationFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    : IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
{
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task<TResult>> func;

    internal TaskCancellationFunc(Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(
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

        return this.func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, token);
    }
}
