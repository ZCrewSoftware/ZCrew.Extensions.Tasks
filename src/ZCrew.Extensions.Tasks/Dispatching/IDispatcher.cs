namespace ZCrew.Extensions.Tasks.Dispatching;

public interface IDispatcher
{
    /// <summary>
    ///     Provides notifications of unhandled exceptions that occur within the dispatcher.
    /// </summary>
    event UnhandledExceptionEventHandler? UnhandledException;

    /// <summary>
    ///     Validates that the currently executing code is running inside the dispatcher.
    /// </summary>
    void AssertAccess();

    /// <summary>
    ///     Returns a value that determines whether using the dispatcher to invoke a work item is required from the
    ///     current context.
    /// </summary>
    /// <returns><c>false</c> if calling an <c>InvokeAsync</c> method is required, otherwise <c>true</c>.</returns>
    bool CheckAccess();

    /// <summary>
    ///     Invokes the given <see cref="Action"/> within a single async context.
    /// </summary>
    /// <param name="workItem">The action to execute.</param>
    /// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
    Task InvokeAsync(Action workItem);

    /// <summary>
    ///     Invokes the given <see cref="Func{TResult}"/> within a single async context.
    /// </summary>
    /// <param name="workItem">The asynchronous action to execute.</param>
    /// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
    Task InvokeAsync(Func<Task> workItem);

    /// <summary>
    ///     Invokes the given <see cref="Func{TResult}"/> within a single async context.
    /// </summary>
    /// <param name="workItem">The function to execute.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> that will be completed when the function has finished executing.
    /// </returns>
    Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem);

    /// <summary>
    ///     Invokes the given <see cref="Func{TResult}"/> within a single async context.
    /// </summary>
    /// <param name="workItem">The asynchronous function to execute.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> that will be completed when the function has finished executing.
    /// </returns>
    Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem);
}
