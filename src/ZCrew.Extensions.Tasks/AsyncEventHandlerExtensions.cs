namespace ZCrew.Extensions.Tasks;

/// <summary>
///     Extension methods for the <see cref="AsyncEventHandler" /> and <see cref="AsyncEventHandler{TEventArgs}" />.
///     The preferred way to call an <see cref="AsyncEventHandler" /> and <see cref="AsyncEventHandler{TEventArgs}" />
///     is <see cref="InvokeAsync" /> and <see cref="InvokeAsync{TEventArgs}" /> respectively, which invoke the event
///     handlers sequentially, and stop and rethrow on the first exception.
/// </summary>
public static class AsyncEventHandlerExtensions
{
    /// <summary>
    ///     Invokes the <see cref="AsyncEventHandler" /> sequentially. Each event handler will be called in the sequence
    ///     they were registered. The <see cref="CancellationToken" /> <paramref name="token" /> will be checked before
    ///     calling each event handler and remaining event handlers will not be called if cancellation has been
    ///     requested. Exceptions thrown by any event handler will be rethrown immediately and the remaining event
    ///     handlers will not be called.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <see name="EventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <remarks>
    ///     This method is based on the functionality of <see cref="EventHandler.Invoke"/> and will stop calling
    ///     handlers if an exception is thrown.
    /// </remarks>
    public static Task InvokeAsync(
        this AsyncEventHandler? asyncEventHandler,
        object sender,
        EventArgs eventArgs,
        CancellationToken token = default
    )
    {
        return InvokeSequentialAsync(asyncEventHandler, sender, eventArgs, true, token);
    }

    /// <summary>
    ///     Invokes the <see cref="AsyncEventHandler{TEventArgs}" /> sequentially. Each event handler will be called in
    ///     the sequence they were registered. The <see cref="CancellationToken" /> <paramref name="token" /> will be
    ///     checked before calling each event handler and remaining event handlers will not be called if cancellation
    ///     has been requested. Exceptions thrown by any event handler will be rethrown immediately and the remaining
    ///     event handlers will not be called.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler{TEventArgs}" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <see name="EventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <typeparam name="TEventArgs">The type of the event.</typeparam>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <remarks>
    ///     This method is based on the functionality of <see cref="EventHandler{TEventArgs}.Invoke"/> and will stop
    ///     calling handlers if an exception is thrown.
    /// </remarks>
    public static Task InvokeAsync<TEventArgs>(
        this AsyncEventHandler<TEventArgs>? asyncEventHandler,
        object sender,
        TEventArgs eventArgs,
        CancellationToken token = default
    )
    {
        return InvokeSequentialAsync(asyncEventHandler, sender, eventArgs, true, token);
    }

    /// <summary>
    ///     Invokes the <see cref="AsyncEventHandler" /> in parallel. Each event handler will be forced to yield
    ///     asynchronously. The <see cref="CancellationToken" /> <paramref name="token" /> will be checked before
    ///     calling the event handlers and remaining event handlers may not be called if cancellation has been
    ///     requested. Exceptions thrown by any event handler will be recorded in an <see cref="AggregateException" />
    ///     and remaining event handlers will be called.
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
    ///     Invokes the <see cref="AsyncEventHandler" /> sequentially. Each event handler will be called in the sequence
    ///     they were registered. The <see cref="CancellationToken" /> <paramref name="token" /> will be checked before
    ///     calling each event handler and remaining event handlers will not be called if cancellation has been
    ///     requested. Exceptions thrown by any event handler will be recorded in an <see cref="AggregateException" />
    ///     and remaining event handlers will be called.
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
    public static Task InvokeSequentialAsync(
        this AsyncEventHandler? asyncEventHandler,
        object sender,
        EventArgs eventArgs,
        CancellationToken token = default
    )
    {
        return InvokeSequentialAsync(asyncEventHandler, sender, eventArgs, false, token);
    }

    /// <summary>
    ///     <para>
    ///     Invokes the <see cref="AsyncEventHandler" /> sequentially. Each event handler will be called in the sequence
    ///     they were registered. The <see cref="CancellationToken" /> <paramref name="token" /> will be checked before
    ///     calling each event handler and remaining event handlers will not be called if cancellation has been
    ///     requested.
    ///     </para>
    ///     <para>
    ///     If <paramref name="throwOnFirstException"/> is true: exceptions thrown by any event handler will be rethrown
    ///     immediately and the remaining event handlers will not be called.
    ///     If <paramref name="throwOnFirstException"/> is false: exceptions thrown by any event handler will be
    ///     recorded in an <see cref="AggregateException" /> and the remaining event handlers will be called.
    ///     </para>
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <see name="EventArgs" />.</param>
    /// <param name="throwOnFirstException">
    ///     If <see langword="false"/> then exceptions bubble-up as an <see cref="AggregateException"/>. Otherwise, the
    ///     first exception is thrown.
    /// </param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <exception cref="AggregateException">
    ///     If one or more exceptions were thrown by the handlers and when <paramref name="throwOnFirstException"/> is
    ///     <see langword="false"/>.
    /// </exception>
    private static async Task InvokeSequentialAsync(
        this AsyncEventHandler? asyncEventHandler,
        object sender,
        EventArgs eventArgs,
        bool throwOnFirstException,
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
            catch (Exception) when (throwOnFirstException)
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
    ///     Invokes the <see cref="AsyncEventHandler{TEventArgs}" /> in parallel. Each event handler will be forced to
    ///     yield asynchronously. The <see cref="CancellationToken" /> <paramref name="token" /> will be checked before
    ///     calling the event handlers and remaining event handlers may not be called if cancellation has been
    ///     requested. Exceptions thrown by any event handler will be recorded in an <see cref="AggregateException" />
    ///     and remaining event handlers will be called.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler{TEventArgs}" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <typeparamref name="TEventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <typeparam name="TEventArgs">The type of the event.</typeparam>
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
            token.ThrowIfCancellationRequested();

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
    ///     Invokes the <see cref="AsyncEventHandler{TEventArgs}" /> sequentially. Each event handler will be called in
    ///     the sequence they were registered. The <see cref="CancellationToken" /> <paramref name="token" /> will be
    ///     checked before calling each event handler and remaining event handlers will not be called if cancellation
    ///     has been requested. Exceptions thrown by any event handler will be recorded in an
    ///     <see cref="AggregateException" /> and remaining event handlers will be called.
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler{TEventArgs}" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <see name="EventArgs" />.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <typeparam name="TEventArgs">The type of the event.</typeparam>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <exception cref="AggregateException">If one or more exceptions were thrown by the handlers.</exception>
    /// <remarks>
    ///     This method deviates from the behavior of <see cref="EventHandler{TEventArgs}.Invoke"/> and will continue
    ///     calling handlers if an exception is thrown. Use <see cref="InvokeAsync{TEventArgs}"/> if this is
    ///     undesirable.
    /// </remarks>
    public static Task InvokeSequentialAsync<TEventArgs>(
        this AsyncEventHandler<TEventArgs>? asyncEventHandler,
        object sender,
        TEventArgs eventArgs,
        CancellationToken token = default
    )
    {
        return InvokeSequentialAsync(asyncEventHandler, sender, eventArgs, false, token);
    }

    /// <summary>
    ///     <para>
    ///     Invokes the <see cref="AsyncEventHandler{TEventArgs}" /> sequentially. Each event handler will be called in
    ///     the sequence they were registered. The <see cref="CancellationToken" /> <paramref name="token" /> will be
    ///     checked before calling each event handler and remaining event handlers will not be called if cancellation
    ///     has been requested.
    ///     </para>
    ///     <para>
    ///     If <paramref name="throwOnFirstException"/> is true: exceptions thrown by any event handler will be rethrown
    ///     immediately and the remaining event handlers will not be called.
    ///     If <paramref name="throwOnFirstException"/> is false: exceptions thrown by any event handler will be
    ///     recorded in an <see cref="AggregateException" /> and the remaining event handlers will be called.
    ///     </para>
    /// </summary>
    /// <param name="asyncEventHandler">The <see cref="AsyncEventHandler{TEventArgs}" />.</param>
    /// <param name="sender">The reference sending the request.</param>
    /// <param name="eventArgs">The <see name="EventArgs" />.</param>
    /// <param name="throwOnFirstException">
    ///     If <see langword="false"/> then exceptions bubble-up as an <see cref="AggregateException"/>. Otherwise, the
    ///     first exception is thrown.
    /// </param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <typeparam name="TEventArgs">The type of the event.</typeparam>
    /// <exception cref="OperationCanceledException">
    ///     If a request was made to cancel the <paramref name="token" />.
    /// </exception>
    /// <exception cref="AggregateException">
    ///     If one or more exceptions were thrown by the handlers and when <paramref name="throwOnFirstException"/> is
    ///     <see langword="false"/>.
    /// </exception>
    private static async Task InvokeSequentialAsync<TEventArgs>(
        this AsyncEventHandler<TEventArgs>? asyncEventHandler,
        object sender,
        TEventArgs eventArgs,
        bool throwOnFirstException,
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
            catch (Exception) when (throwOnFirstException)
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
