using System.Threading.Channels;

namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     A <see cref="BackgroundDispatcher"/> that uses a <see cref="Channel{T}"/> to queue and process
///     dispatched operations sequentially on a background task.
/// </summary>
internal abstract class ChannelDispatcher : BackgroundDispatcher
{
    private static int globalDispatcherId;
    private readonly AsyncLocal<int?> asyncDispatcherId = new();
    private readonly Channel<IDispatchedOperation> operationChannel;

    private readonly int dispatcherId;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ChannelDispatcher"/> class with the specified channel.
    /// </summary>
    /// <param name="operationChannel">The channel used to queue dispatched operations.</param>
    protected ChannelDispatcher(Channel<IDispatchedOperation> operationChannel)
    {
        this.operationChannel = operationChannel;
        this.dispatcherId = Interlocked.Increment(ref globalDispatcherId);
    }

    /// <inheritdoc/>
    public override bool CheckAccess()
    {
        return this.asyncDispatcherId.Value == this.dispatcherId;
    }

    /// <inheritdoc/>
    public override Task InvokeAsync(Action workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        if (CheckAccess())
        {
            workItem();
            return Task.CompletedTask;
        }
        return InvokeActionAsync(workItem.AsAsyncAction(), CancellationToken.None);
    }

    /// <inheritdoc/>
    public override Task InvokeAsync(Func<Task> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        if (CheckAccess())
        {
            return workItem();
        }
        return InvokeActionAsync(workItem.AsAsyncAction(), CancellationToken.None);
    }

    /// <inheritdoc/>
    public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        if (CheckAccess())
        {
            return Task.FromResult(workItem());
        }
        return InvokeFuncAsync(workItem.AsAsyncFunc(), CancellationToken.None);
    }

    /// <inheritdoc/>
    public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        if (CheckAccess())
        {
            return workItem();
        }
        return InvokeFuncAsync(workItem.AsAsyncFunc(), CancellationToken.None);
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        this.asyncDispatcherId.Value = this.dispatcherId;

        while (!token.IsCancellationRequested)
        {
            var operation = await this.operationChannel.Reader.ReadAsync(token);
            try
            {
                await operation.InvokeAsync(token);
            }
            catch (OperationCanceledException) when (operation.IsCanceled)
            {
                // Operation has already been canceled and does not need to be set again
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                // TODO: currently the invocation methods don't have cancellation tokens, so we can't check that case
                // Operation was canceled by a request to cancel the dispatcher
                operation.SetCanceled();
            }
            catch (Exception ex)
            {
                OnUnhandledException(new UnhandledExceptionEventArgs(ex, false));

                // May catch OperationCanceledException thrown within the operation, so we want to preserve the stack
                // trace instead of just calling 'operation.SetCanceled()'.
                operation.SetException(ex);
            }
        }
    }

    private async Task InvokeActionAsync(IAsyncAction action, CancellationToken token)
    {
        var operation = new AsyncDispatchedAction(action);
        await this.operationChannel.Writer.WriteAsync(operation, token);
        await operation.WaitAsync(token);
    }

    private async Task<TResult> InvokeFuncAsync<TResult>(IAsyncFunc<TResult> func, CancellationToken token)
    {
        var operation = new AsyncDispatchedFunc<TResult>(func);
        await this.operationChannel.Writer.WriteAsync(operation, token);
        return await operation.WaitAsync(token);
    }
}
