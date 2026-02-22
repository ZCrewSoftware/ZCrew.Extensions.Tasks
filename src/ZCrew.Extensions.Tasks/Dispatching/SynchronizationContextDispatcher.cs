namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     A <see cref="Dispatcher"/> implementation that serializes work items using a
///     <see cref="DispatcherSynchronizationContext"/>.
/// </summary>
internal sealed class SynchronizationContextDispatcher : Dispatcher
{
    private readonly DispatcherSynchronizationContext context;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SynchronizationContextDispatcher"/> class.
    /// </summary>
    public SynchronizationContextDispatcher()
    {
        this.context = new DispatcherSynchronizationContext();
        this.context.UnhandledException += (_, e) =>
        {
            OnUnhandledException(e);
        };
    }

    /// <inheritdoc/>
    public override bool CheckAccess()
    {
        return SynchronizationContext.Current == this.context;
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

        return this.context.InvokeAsync(workItem);
    }

    /// <inheritdoc/>
    public override Task InvokeAsync(Func<Task> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        if (CheckAccess())
        {
            return workItem();
        }

        return this.context.InvokeAsync(workItem);
    }

    /// <inheritdoc/>
    public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        if (CheckAccess())
        {
            return Task.FromResult(workItem());
        }

        return this.context.InvokeAsync(workItem);
    }

    /// <inheritdoc/>
    public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);
        if (CheckAccess())
        {
            return workItem();
        }

        return this.context.InvokeAsync(workItem);
    }
}
