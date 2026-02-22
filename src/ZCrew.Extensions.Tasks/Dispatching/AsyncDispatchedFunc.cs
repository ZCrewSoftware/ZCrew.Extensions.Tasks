namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     An <see cref="IDispatchedFunc{TResult}"/> implementation that wraps an <see cref="IAsyncFunc{TResult}"/>
///     and uses a <see cref="TaskCompletionSource{TResult}"/> to signal completion.
/// </summary>
/// <typeparam name="TResult">The type of the result produced by the function.</typeparam>
internal class AsyncDispatchedFunc<TResult> : IDispatchedFunc<TResult>
{
    private readonly TaskCompletionSource<TResult> tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private readonly IAsyncFunc<TResult> func;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AsyncDispatchedFunc{TResult}"/> class.
    /// </summary>
    /// <param name="func">The async function to dispatch.</param>
    public AsyncDispatchedFunc(IAsyncFunc<TResult> func)
    {
        this.func = func;
    }

    /// <inheritdoc/>
    public bool IsCanceled => this.tcs.Task.IsCanceled;

    /// <inheritdoc/>
    public async Task<TResult> WaitAsync(CancellationToken token)
    {
        return await this.tcs.Task.WaitAsync(token);
    }

    /// <inheritdoc/>
    public async Task InvokeAsync(CancellationToken token)
    {
        var result = await this.func.InvokeAsync(token);
        this.tcs.SetResult(result);
    }

    /// <inheritdoc/>
    public void SetException(Exception exception)
    {
        this.tcs.TrySetException(exception);
    }

    /// <inheritdoc/>
    public void SetCanceled()
    {
        this.tcs.TrySetCanceled();
    }
}
