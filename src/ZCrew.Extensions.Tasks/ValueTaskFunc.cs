namespace ZCrew.Extensions.Tasks;

/// <inheritdoc/>
internal sealed class ValueTaskFunc<TResult> : IAsyncFunc<TResult>
{
    private readonly Func<ValueTask<TResult>> func;

    internal ValueTaskFunc(Func<ValueTask<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func().AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskFunc<T, TResult> : IAsyncFunc<T, TResult>
{
    private readonly Func<T, ValueTask<TResult>> func;

    internal ValueTaskFunc(Func<T, ValueTask<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T arg, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskFunc<T1, T2, TResult> : IAsyncFunc<T1, T2, TResult>
{
    private readonly Func<T1, T2, ValueTask<TResult>> func;

    internal ValueTaskFunc(Func<T1, T2, ValueTask<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg1, arg2).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskFunc<T1, T2, T3, TResult> : IAsyncFunc<T1, T2, T3, TResult>
{
    private readonly Func<T1, T2, T3, ValueTask<TResult>> func;

    internal ValueTaskFunc(Func<T1, T2, T3, ValueTask<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg1, arg2, arg3).AsTask();
    }
}

/// <inheritdoc/>
internal sealed class ValueTaskFunc<T1, T2, T3, T4, TResult> : IAsyncFunc<T1, T2, T3, T4, TResult>
{
    private readonly Func<T1, T2, T3, T4, ValueTask<TResult>> func;

    internal ValueTaskFunc(Func<T1, T2, T3, T4, ValueTask<TResult>> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        return this.func(arg1, arg2, arg3, arg4).AsTask();
    }
}
