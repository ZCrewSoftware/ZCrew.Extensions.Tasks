namespace ZCrew.Extensions.Tasks;

/// <summary>
///     Extension methods for the <see cref="AsyncEventHandler" /> and <see cref="AsyncEventHandler{TEventArgs}" />.
///     The preferred way to call an <see cref="AsyncEventHandler" /> and <see cref="AsyncEventHandler{TEventArgs}" />
///     is <see cref="InvokeSequentialAsync" /> and <see cref="InvokeSequentialAsync{TEventArgs}" /> respectively.
/// </summary>
public static class AsyncEventHandlerExtensions
{
    /// <summary>
    ///     Invokes the <see cref="AsyncEventHandler" /> in a sequential way based on the system implementation of
    ///     <see cref="AsyncEventHandler.Invoke" />. This will not check if the <see cref="CancellationToken" />
    ///     <paramref name="token" /> has been canceled between calls. Synchronous exceptions thrown by any event
    ///     handler will immediately rethrow and the remaining event handlers will not be called. Once every handler has
    ///     been called and no exceptions have been thrown: no exceptions thrown asynchronously will be caught.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <see cref="EventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <remarks>
    ///     This method is based on the functionality of <see cref="EventHandler.Invoke"/> and will stop calling
    ///     handlers if a synchronous exception is thrown. Asynchronous exceptions will not be rethrown and will not be
    ///     observed.
    /// </remarks>
    public static Task InvokeAsync(
        this AsyncEventHandler? asyncEventHandler,
        object sender,
        EventArgs eventArgs,
        CancellationToken token = default
    )
    {
        token.ThrowIfCancellationRequested();
        return asyncEventHandler?.Invoke(sender, eventArgs, token) ?? Task.CompletedTask;
    }

    /// <summary>
    ///     Invokes the <see cref="AsyncEventHandler{TEventArgs}" /> in a sequential way based on the system
    ///     implementation of <see cref="AsyncEventHandler{TEventArgs}.Invoke" />. This will not check if the
    ///     <see cref="CancellationToken" /> <paramref name="token" /> has been canceled between calls. Synchronous
    ///     exceptions thrown by any event handler will immediately rethrow and the remaining event handlers will not be
    ///     called. Once every handler has been called and no exceptions have been thrown: no exceptions thrown
    ///     asynchronously will be caught.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler{TEventArgs}" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <typeparamref name="TEventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <remarks>
    ///     This method is based on the functionality of <see cref="EventHandler{TEventArgs}.Invoke"/> and will stop
    ///     calling handlers if a synchronous exception is thrown. Asynchronous exceptions will not be rethrown and will
    ///     not be observed.
    /// </remarks>
    public static Task InvokeAsync<TEventArgs>(
        this AsyncEventHandler<TEventArgs>? asyncEventHandler,
        object sender,
        TEventArgs eventArgs,
        CancellationToken token = default
    )
    {
        token.ThrowIfCancellationRequested();
        return asyncEventHandler?.Invoke(sender, eventArgs, token) ?? Task.CompletedTask;
    }

    /// <summary>
    ///     Invokes the <see cref="AsyncEventHandler" /> in a parallel way. Each event handler will be forced to yield
    ///     asynchronously. The <see cref="CancellationToken" /> <paramref name="token" /> will be checked before
    ///     calling the event handlers and pending event handlers may not be invoked if cancellation has been requested.
    ///     Exceptions thrown by any event handler will be recorded in a <see cref="AggregateException" /> and pending
    ///     event handlers will be invoked.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <see cref="EventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <exception cref="AggregateException">If one or more exceptions were thrown by the handlers.</exception>
    /// <remarks>
    ///     This method deviates from the behavior of <see cref="EventHandler.Invoke"/> and will continue calling
    ///     handlers if an exception is thrown. Use <see cref="InvokeAsync"/> if this is undesirable.
    /// </remarks>
    public static async Task InvokeParallelAsync(
        this AsyncEventHandler? asyncEventHandler,
        object sender,
        EventArgs eventArgs,
        CancellationToken token = default
    )
    {
        token.ThrowIfCancellationRequested();
        if (asyncEventHandler == null)
        {
            return;
        }

        var invocations = asyncEventHandler.GetInvocationList();
        var tasks = default(List<Task>);
        foreach (var invocation in invocations)
        {
            token.ThrowIfCancellationRequested();

            // Filter out invocations of the wrong type
            if (invocation is not AsyncEventHandler asyncEventHandlerInvocation)
            {
                continue;
            }

            // Initialize the task list and add the async event handler's invocation after yielding. This will prevent
            // tasks from blocking synchronously or throwing exceptions before all tasks have the opportunity to start.
            tasks ??= [];
            tasks.Add(Invoke(asyncEventHandlerInvocation));
        }

        if (tasks is null)
        {
            return;
        }

        // Ensure an aggregate exception is thrown by not relying on the exception thrown by awaiting all the tasks.
        var whenAll = Task.WhenAll(tasks);
        try
        {
            await whenAll;
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            throw;
        }
        catch when (whenAll.Exception is not null)
        {
            throw whenAll.Exception;
        }
        return;

        async Task Invoke(AsyncEventHandler handler)
        {
            await Task.Yield();
            await handler.Invoke(sender, eventArgs, token);
        }
    }

    /// <summary>
    ///     Invokes the <see cref="AsyncEventHandler" /> in a sequential way. Each event handler will be invoked in the
    ///     sequence they were registered. The <see cref="CancellationToken" /> <paramref name="token" /> will be
    ///     checked before calling each event handlers and pending event handlers will not be invoked if cancellation
    ///     has been requested. Exceptions thrown by any event handler will be recorded in a
    ///     <see cref="AggregateException" /> and pending event handlers will be invoked.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <see name="EventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <exception cref="AggregateException">If one or more exceptions were thrown by the handlers.</exception>
    /// <remarks>
    ///     This method deviates from the behavior of <see cref="EventHandler.Invoke"/> and will continue calling
    ///     handlers if an exception is thrown. Use <see cref="InvokeAsync"/> if this is undesirable.
    /// </remarks>
    public static async Task InvokeSequentialAsync(
        this AsyncEventHandler? asyncEventHandler,
        object sender,
        EventArgs eventArgs,
        CancellationToken token = default
    )
    {
        token.ThrowIfCancellationRequested();
        if (asyncEventHandler == null)
        {
            return;
        }

        var invocations = asyncEventHandler.GetInvocationList();
        var exceptions = default(List<Exception>);
        foreach (var invocation in invocations)
        {
            token.ThrowIfCancellationRequested();

            // Filter out invocations of the wrong type
            if (invocation is not AsyncEventHandler asyncEventHandlerInvocation)
            {
                continue;
            }

            // Invoke the async event handler and await its completion, collecting any exceptions thrown
            try
            {
                await asyncEventHandlerInvocation.Invoke(sender, eventArgs, token);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                exceptions ??= [];
                exceptions.Add(ex);
            }
        }

        if (exceptions is not null)
        {
            throw new AggregateException(exceptions);
        }
    }

    /// <summary>
    ///     Invokes the <see cref="AsyncEventHandler{TEventArgs}" /> in a parallel way. Each event handler will be
    ///     forced to yield asynchronously. The <see cref="CancellationToken" /> <paramref name="token" /> will be
    ///     checked before calling the event handlers and pending event handlers may not be invoked if cancellation has
    ///     been requested. Exceptions thrown by any event handler will be recorded in a
    ///     <see cref="AggregateException" /> and pending event handlers will be invoked.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler{TEventArgs}" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <typeparamref name="TEventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <exception cref="AggregateException">If one or more exceptions were thrown by the handlers.</exception>
    /// <remarks>
    ///     This method deviates from the behavior of <see cref="EventHandler{TEventArgs}.Invoke"/> and will continue
    ///     calling handlers if an exception is thrown. Use <see cref="InvokeAsync{TEventArgs}"/> if this is
    ///     undesirable.
    /// </remarks>
    public static async Task InvokeParallelAsync<TEventArgs>(
        this AsyncEventHandler<TEventArgs>? asyncEventHandler,
        object sender,
        TEventArgs eventArgs,
        CancellationToken token = default
    )
    {
        token.ThrowIfCancellationRequested();
        if (asyncEventHandler == null)
        {
            return;
        }

        var invocations = asyncEventHandler.GetInvocationList();
        var tasks = default(List<Task>);
        foreach (var invocation in invocations)
        {
            // Filter out invocations of the wrong type
            if (invocation is not AsyncEventHandler<TEventArgs> asyncEventHandlerInvocation)
            {
                continue;
            }

            // Initialize the task list and add the async event handler's invocation after yielding. This will prevent
            // tasks from blocking synchronously or throwing exceptions before all tasks have the opportunity to start.
            tasks ??= [];
            tasks.Add(Invoke(asyncEventHandlerInvocation));
        }

        if (tasks is null)
        {
            return;
        }

        // Ensure an aggregate exception is thrown by not relying on the exception thrown by awaiting all the tasks.
        var whenAll = Task.WhenAll(tasks);
        try
        {
            await whenAll;
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            throw;
        }
        catch when (whenAll.Exception is not null)
        {
            throw whenAll.Exception;
        }
        return;

        async Task Invoke(AsyncEventHandler<TEventArgs> handler)
        {
            await Task.Yield();
            await handler.Invoke(sender, eventArgs, token);
        }
    }

    /// <summary>
    ///     Invokes the <see cref="AsyncEventHandler{TEventArgs}" /> in a sequential way. Each event handler will be
    ///     invoked in the sequence they were registered. The <see cref="CancellationToken" /> <paramref name="token" />
    ///     will be checked before calling each event handlers and pending event handlers will not be invoked if
    ///     cancellation has been requested. Exceptions thrown by any event handler will be recorded in a
    ///     <see cref="AggregateException" /> and pending event handlers will be invoked.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler{TEventArgs}" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <typeparamref name="TEventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <exception cref="AggregateException">If one or more exceptions were thrown by the handlers.</exception>
    /// <remarks>
    ///     This method deviates from the behavior of <see cref="EventHandler{TEventArgs}.Invoke"/> and will continue
    ///     calling handlers if an exception is thrown. Use <see cref="InvokeAsync{TEventArgs}"/> if this is
    ///     undesirable.
    /// </remarks>
    public static async Task InvokeSequentialAsync<TEventArgs>(
        this AsyncEventHandler<TEventArgs>? asyncEventHandler,
        object sender,
        TEventArgs eventArgs,
        CancellationToken token = default
    )
    {
        token.ThrowIfCancellationRequested();
        if (asyncEventHandler == null)
        {
            return;
        }

        var invocations = asyncEventHandler.GetInvocationList();
        var exceptions = default(List<Exception>);
        foreach (var invocation in invocations)
        {
            token.ThrowIfCancellationRequested();

            // Filter out invocations of the wrong type
            if (invocation is not AsyncEventHandler<TEventArgs> asyncEventHandlerInvocation)
            {
                continue;
            }

            // Invoke the async event handler and await its completion, collecting any exceptions thrown
            try
            {
                await asyncEventHandlerInvocation.Invoke(sender, eventArgs, token);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                exceptions ??= [];
                exceptions.Add(ex);
            }
        }

        if (exceptions is not null)
        {
            throw new AggregateException(exceptions);
        }
    }
}
