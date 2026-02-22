namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     Represents a void-returning operation that has been dispatched to a <see cref="IDispatcher"/> for execution.
/// </summary>
internal interface IDispatchedAction : IDispatchedOperation
{
    /// <summary>
    ///     Waits for the dispatched action to complete.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the wait.</param>
    /// <returns>A <see cref="Task"/> that completes when the dispatched action has finished executing.</returns>
    Task WaitAsync(CancellationToken token);
}
