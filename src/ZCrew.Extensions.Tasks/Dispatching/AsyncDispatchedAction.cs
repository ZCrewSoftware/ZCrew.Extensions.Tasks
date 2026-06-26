namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     An <see cref="IDispatchedAction"/> implementation that wraps an <see cref="IAsyncAction"/>
///     and uses a <see cref="TaskCompletionSource"/> to signal completion.
/// </summary>
internal class AsyncDispatchedAction : IDispatchedAction
{
    private readonly TaskCompletionSource tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private readonly IAsyncAction action;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AsyncDispatchedAction"/> class.
    /// </summary>
    /// <param name="action">The async action to dispatch.</param>
    public AsyncDispatchedAction(IAsyncAction action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public bool IsCanceled => this.tcs.Task.IsCanceled;

    /// <inheritdoc/>
    public Task WaitAsync(CancellationToken token)
    {
        return this.tcs.Task.WaitAsync(token);
    }

    /// <inheritdoc/>
    public async Task InvokeAsync(CancellationToken token)
    {
        await this.action.InvokeAsync(token).ConfigureAwait(false);
        this.tcs.SetResult();
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
