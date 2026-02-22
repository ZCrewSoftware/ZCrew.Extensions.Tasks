namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     Represents a result-returning operation that has been dispatched to a <see cref="IDispatcher"/> for execution.
/// </summary>
/// <typeparam name="TResult">The type of the result produced by the operation.</typeparam>
internal interface IDispatchedFunc<TResult> : IDispatchedOperation
{
    /// <summary>
    ///     Waits for the dispatched function to complete and returns its result.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the wait.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> that completes with the result when the dispatched function has finished
    ///     executing.
    /// </returns>
    Task<TResult> WaitAsync(CancellationToken token);
}
