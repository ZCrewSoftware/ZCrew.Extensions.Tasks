namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     Provides a base class for <see cref="IDispatcher"/> implementations and factory methods
///     for creating dispatchers.
/// </summary>
public abstract class Dispatcher : IDispatcher
{
    /// <summary>
    ///     Creates a default instance of <see cref="IDispatcher"/>.
    /// </summary>
    /// <returns>A <see cref="IDispatcher"/> instance.</returns>
    public static IDispatcher CreateDefault()
    {
        return CreateSynchronizationContextDispatcher();
    }

    /// <summary>
    ///     Creates a <see cref="IDispatcher"/> backed by a custom <see cref="SynchronizationContext"/>.
    /// </summary>
    /// <returns>A <see cref="IDispatcher"/> instance.</returns>
    public static IDispatcher CreateSynchronizationContextDispatcher()
    {
        return new SynchronizationContextDispatcher();
    }

    /// <summary>
    ///     Creates a <see cref="IHostedDispatcher"/> backed by an unbounded channel.
    /// </summary>
    /// <returns>A <see cref="IHostedDispatcher"/> instance.</returns>
    public static IHostedDispatcher CreateUnboundedChannelDispatcher()
    {
        return new UnboundedChannelDispatcher();
    }

    /// <summary>
    ///     Creates a <see cref="IHostedDispatcher"/> backed by a bounded channel with the specified capacity.
    /// </summary>
    /// <param name="capacity">The maximum number of operations the channel can hold.</param>
    /// <returns>A <see cref="IHostedDispatcher"/> instance.</returns>
    public static IHostedDispatcher CreateBoundedChannelDispatcher(int capacity)
    {
        return new BoundedChannelDispatcher(capacity);
    }

    /// <inheritdoc/>
    public event UnhandledExceptionEventHandler? UnhandledException;

    /// <inheritdoc/>
    public abstract bool CheckAccess();

    /// <inheritdoc/>
    public void AssertAccess()
    {
        if (!CheckAccess())
        {
            throw new InvalidOperationException(
                "The current thread is not associated with the Dispatcher. "
                    + "Use InvokeAsync() to switch execution to the Dispatcher."
            );
        }
    }

    /// <inheritdoc/>
    public abstract Task InvokeAsync(Action workItem);

    /// <inheritdoc/>
    public abstract Task InvokeAsync(Func<Task> workItem);

    /// <inheritdoc/>
    public abstract Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem);

    /// <inheritdoc/>
    public abstract Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem);

    /// <summary>
    ///     Called to notify listeners of an unhandled exception.
    /// </summary>
    /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/>.</param>
    protected void OnUnhandledException(UnhandledExceptionEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(e);

        UnhandledException?.Invoke(this, e);
    }
}
