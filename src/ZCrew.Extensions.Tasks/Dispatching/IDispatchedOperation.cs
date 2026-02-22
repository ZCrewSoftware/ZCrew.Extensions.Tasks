namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     Represents an operation that has been dispatched to a <see cref="IDispatcher"/> for execution.
/// </summary>
internal interface IDispatchedOperation
{
    /// <summary>
    ///     Gets a value indicating whether the operation has been canceled.
    /// </summary>
    bool IsCanceled { get; }

    /// <summary>
    ///     Invokes the dispatched operation.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that completes when the operation has finished executing.</returns>
    Task InvokeAsync(CancellationToken token);

    /// <summary>
    ///     Transitions the operation into a faulted state with the specified exception.
    /// </summary>
    /// <param name="exception">The exception that caused the operation to fault.</param>
    void SetException(Exception exception);

    /// <summary>
    ///     Transitions the operation into a canceled state.
    /// </summary>
    void SetCanceled();
}
