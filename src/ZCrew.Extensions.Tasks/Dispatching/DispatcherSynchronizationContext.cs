using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     A <see cref="SynchronizationContext"/> that serializes work items onto a single logical execution context,
///     ensuring only one operation runs at a time.
/// </summary>
/// <remarks>
///     Derived from Blazor's <c>Microsoft.AspNetCore.Components.Rendering.RendererSynchronizationContext</c>.
/// </remarks>
[DebuggerDisplay("{taskQueue.Status,nq}")]
internal sealed class DispatcherSynchronizationContext : SynchronizationContext
{
    private readonly object contextLock;

    private Task taskQueue;

    /// <summary>
    ///     Provides notifications of unhandled exceptions that occur within the synchronization context.
    /// </summary>
    public event UnhandledExceptionEventHandler? UnhandledException;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DispatcherSynchronizationContext"/> class.
    /// </summary>
    public DispatcherSynchronizationContext()
        : this(new object(), Task.CompletedTask) { }

    private DispatcherSynchronizationContext(object contextLock, Task taskQueue)
    {
        this.contextLock = contextLock;
        this.taskQueue = taskQueue;
    }

    /// <inheritdoc />
    public override SynchronizationContext CreateCopy()
    {
        lock (this.contextLock)
        {
            return new DispatcherSynchronizationContext(this.contextLock, this.taskQueue);
        }
    }

    // The following two Action/Func<TResult> overloads can be more optimized than their
    // async equivalents, as they don't need to deal with the possibility of the callback
    // posting back to this context.  As a result, they can use the Task for the InvokeAsync
    // operation as the object to use in the task queue itself if the operation is the next
    // in line.  For the async overloads, the callbacks might await and need to post back
    // to the current synchronization context, in which case their continuation could end
    // up seeing the InvokeAsync task as the antecedent, which would lead to deadlock. As
    // such, those operations must use a different task for the task queue. Note that this
    // requires these synchronous callbacks not doing sync-over-async with any work that
    // blocks waiting for this sync ctx to do work, but such cases are perilous, anyway,
    // as they invariably lead to deadlock.

    /// <summary>
    ///     Invokes the given <see cref="Action"/> within this synchronization context.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>A <see cref="Task"/> that completes when the action has finished executing.</returns>
    public Task InvokeAsync(Action action)
    {
        var completion = AsyncTaskMethodBuilder.Create();
        var t = completion.Task; // lazy initialize before passing around the struct

        lock (this.contextLock)
        {
            if (!this.taskQueue.IsCompleted)
            {
                this.taskQueue = PostAsync(this.taskQueue, Execute, (completion, action, this));
                return t;
            }

            this.taskQueue = t;
        }

        Execute((completion, action, this));
        return t;

        static void Execute(
            (AsyncTaskMethodBuilder Completion, Action Action, DispatcherSynchronizationContext Context) state
        )
        {
            var original = Current;
            SetSynchronizationContext(state.Context);
            try
            {
                state.Action();
                state.Completion.SetResult();
            }
            catch (Exception exception)
            {
                state.Completion.SetException(exception);
            }
            finally
            {
                SetSynchronizationContext(original);
            }
        }
    }

    /// <summary>
    ///     Invokes the given <see cref="Func{TResult}"/> within this synchronization context.
    /// </summary>
    /// <param name="function">The function to execute.</param>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <returns>A <see cref="Task{TResult}"/> that completes with the result when the function has finished executing.</returns>
    public Task<TResult> InvokeAsync<TResult>(Func<TResult> function)
    {
        var completion = AsyncTaskMethodBuilder<TResult>.Create();
        var t = completion.Task; // lazy initialize before passing around the struct

        lock (this.contextLock)
        {
            if (!this.taskQueue.IsCompleted)
            {
                this.taskQueue = PostAsync(this.taskQueue, Execute, (completion, function, this));
                return t;
            }

            this.taskQueue = t;
        }

        Execute((completion, function, this));
        return t;

        static void Execute(
            (
                AsyncTaskMethodBuilder<TResult> Completion,
                Func<TResult> Func,
                DispatcherSynchronizationContext Context
            ) state
        )
        {
            var original = Current;
            SetSynchronizationContext(state.Context);
            try
            {
                state.Completion.SetResult(state.Func());
            }
            catch (Exception exception)
            {
                state.Completion.SetException(exception);
            }
            finally
            {
                SetSynchronizationContext(original);
            }
        }
    }

    /// <summary>
    ///     Invokes the given asynchronous action within this synchronization context.
    /// </summary>
    /// <param name="asyncAction">The asynchronous action to execute.</param>
    /// <returns>A <see cref="Task"/> that completes when the action has finished executing.</returns>
    public Task InvokeAsync(Func<Task> asyncAction)
    {
        var completion = AsyncTaskMethodBuilder.Create();
        var t = completion.Task; // lazy initialize before passing around the struct

        SendIfQuiescedOrElsePost(
            static async void (state) =>
            {
                try
                {
                    await state.asyncAction().ConfigureAwait(false);
                    state.completion.SetResult();
                }
                catch (Exception exception)
                {
                    state.completion.SetException(exception);
                }
            },
            (completion, asyncAction)
        );

        return t;
    }

    /// <summary>
    ///     Invokes the given asynchronous function within this synchronization context.
    /// </summary>
    /// <param name="asyncFunction">The asynchronous function to execute.</param>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <returns>A <see cref="Task{TResult}"/> that completes with the result when the function has finished executing.</returns>
    public Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> asyncFunction)
    {
        var completion = AsyncTaskMethodBuilder<TResult>.Create();
        var t = completion.Task; // lazy initialize before passing around the struct

        SendIfQuiescedOrElsePost(
            static async void (state) =>
            {
                try
                {
                    state.completion.SetResult(await state.asyncFunction().ConfigureAwait(false));
                }
                catch (Exception exception)
                {
                    state.completion.SetException(exception);
                }
            },
            (completion, asyncFunction)
        );

        return t;
    }

    /// <inheritdoc/>
    public override void Post(SendOrPostCallback d, object? state)
    {
        lock (this.contextLock)
        {
            this.taskQueue = PostAsync(this.taskQueue, static s => s.d(s.state), (d, state));
        }
    }

    /// <inheritdoc/>
    public override void Send(SendOrPostCallback d, object? state)
    {
        Task antecedent;
        var completion = AsyncTaskMethodBuilder.Create();

        lock (this.contextLock)
        {
            antecedent = this.taskQueue;
            this.taskQueue = completion.Task;
        }

        // We have to block. That's the contract of Send - we don't expect this to be used
        // in many scenarios in Components.
        antecedent.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing).GetAwaiter().GetResult();

        InvokeWithThisAsCurrentSyncCtxThenSetResult(completion, d.Invoke, state); // Allocates, but using this method should be rare
    }

    /// <summary>
    ///     Queues a work item that invokes the <paramref name="callback"/> with this instance as the current
    ///     synchronization context. The work item will only run once <paramref name="antecedent"/> has completed.
    /// </summary>
    private async Task PostAsync<TState>(Task antecedent, Action<TState> callback, TState state)
    {
        await antecedent.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing | ConfigureAwaitOptions.ForceYielding);
        try
        {
            SetSynchronizationContext(this); // this will be undone automatically by the thread pool, so we don't need to here
            callback(state);
        }
        catch (Exception ex)
        {
            DispatchException(ex);
        }
    }

    /// <summary>
    ///     Workhorse for the InvokeAsync methods.
    /// </summary>
    /// <remarks>
    ///     Similar to Post, but it can run the work item synchronously if the context is not busy. This is the main
    ///     code path used by components, we want to be able to run async work but only dispatch if necessary.
    /// </remarks>
    private void SendIfQuiescedOrElsePost<TState>(Action<TState> callback, TState state)
    {
        AsyncTaskMethodBuilder completion;
        lock (this.contextLock)
        {
            if (!this.taskQueue.IsCompleted)
            {
                this.taskQueue = PostAsync(this.taskQueue, callback, state);
                return;
            }

            // We can execute this synchronously because nothing is currently running or queued.
            completion = AsyncTaskMethodBuilder.Create();
            this.taskQueue = completion.Task;
        }

        InvokeWithThisAsCurrentSyncCtxThenSetResult(completion, callback, state);
    }

    /// <summary>
    ///     Sets the current synchronization context to this instance, invokes the <paramref name="callback"/>, resets
    ///     the synchronization context, and sets marks the builder as completed.
    /// </summary>
    private void InvokeWithThisAsCurrentSyncCtxThenSetResult<TState>(
        AsyncTaskMethodBuilder completion,
        Action<TState> callback,
        TState state
    )
    {
        var original = Current;
        try
        {
            SetSynchronizationContext(this);
            callback(state);
        }
        finally
        {
            SetSynchronizationContext(original);
            completion.SetResult();
        }
    }

    /// <summary>
    ///     Invokes <see cref="UnhandledException"/> with the supplied exception instance.
    /// </summary>
    private void DispatchException(Exception ex)
    {
        UnhandledException?.Invoke(this, new UnhandledExceptionEventArgs(ex, isTerminating: false));
    }
}
