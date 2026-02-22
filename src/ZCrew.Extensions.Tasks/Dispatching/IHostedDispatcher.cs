namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     A dispatcher that requires explicit lifecycle management via start and stop operations. These operations should
///     complete rather quickly and should be awaited. The dispatcher will continue to run in the background even after
///     <see cref="StartAsync"/> has completed.
/// </summary>
public interface IHostedDispatcher : IDispatcher
{
    /// <summary>
    ///     Starts the dispatcher, allowing it to begin processing dispatched work items.
    /// </summary>
    /// <param name="token">
    ///     A <see cref="CancellationToken"/> that can be used to cancel the start operation. If this
    ///     <paramref name="token"/> is canceled after this method completes then the dispatcher will not be canceled.
    ///     The <see cref="StopAsync"/> method would need to be called to cancel the dispatcher.
    /// </param>
    /// <returns>A <see cref="Task"/> that completes when the dispatcher has started.</returns>
    Task StartAsync(CancellationToken token = default);

    /// <summary>
    ///     Stops the dispatcher, signaling it to cease processing work items.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the stop operation.</param>
    /// <returns>A <see cref="Task"/> that completes when the dispatcher has stopped.</returns>
    Task StopAsync(CancellationToken token = default);
}
