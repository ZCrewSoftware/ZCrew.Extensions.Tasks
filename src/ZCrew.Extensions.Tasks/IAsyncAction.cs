namespace ZCrew.Extensions.Tasks;

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous action with <see langword="async"/> semantics regardless of the
///     delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Action</c></item>
///         <item><c>Action&lt;CancellationToken&gt;</c></item>
///         <item><c>Func&lt;Task&gt;</c></item>
///         <item><c>Func&lt;CancellationToken, Task&gt;</c></item>
///         <item><c>Func&lt;ValueTask&gt;</c></item>
///         <item><c>Func&lt;CancellationToken, ValueTask&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service
///     {
///         private readonly IAsyncAction action;
///     <br/>
///         public Service(Action action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;Task&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;ValueTask&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public async Task Call(CancellationToken token)
///         {
///             await this.action.InvokeAsync(token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncAction
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task InvokeAsync(CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous action with a parameter and with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Action&lt;T&gt;</c></item>
///         <item><c>Action&lt;T, CancellationToken&gt;</c></item>
///         <item><c>Func&lt;T, Task&gt;</c></item>
///         <item><c>Func&lt;T, CancellationToken, Task&gt;</c></item>
///         <item><c>Func&lt;T, ValueTask&gt;</c></item>
///         <item><c>Func&lt;T, CancellationToken, ValueTask&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service&lt;T&gt;
///     {
///         private readonly IAsyncAction&lt;T&gt; action;
///     <br/>
///         public Service(Action&lt;T&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;T, Task&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;T, ValueTask&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public async Task Call(T arg, CancellationToken token)
///         {
///             await this.action.InvokeAsync(arg, token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncAction<in T>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg">The parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task InvokeAsync(T arg, CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous action with two parameters and with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
/// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Action&lt;T1, T2&gt;</c></item>
///         <item><c>Action&lt;T1, T2, CancellationToken&gt;</c></item>
///         <item><c>Func&lt;T1, T2, Task&gt;</c></item>
///         <item><c>Func&lt;T1, T2, CancellationToken, Task&gt;</c></item>
///         <item><c>Func&lt;T1, T2, ValueTask&gt;</c></item>
///         <item><c>Func&lt;T1, T2, CancellationToken, ValueTask&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service&lt;T1, T2&gt;
///     {
///         private readonly IAsyncAction&lt;T1, T2&gt; action;
///     <br/>
///         public Service(Action&lt;T1, T2&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, Task&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, ValueTask&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public async Task Call(T1 arg1, T2 arg2, CancellationToken token)
///         {
///             await this.action.InvokeAsync(arg1, arg2, token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncAction<in T1, in T2>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg1">The first parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous action with three parameters and with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
/// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
/// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Action&lt;T1, T2, T3&gt;</c></item>
///         <item><c>Action&lt;T1, T2, T3, CancellationToken&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, Task&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, CancellationToken, Task&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, ValueTask&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, CancellationToken, ValueTask&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service&lt;T1, T2, T3&gt;
///     {
///         private readonly IAsyncAction&lt;T1, T2, T3&gt; action;
///     <br/>
///         public Service(Action&lt;T1, T2, T3&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, T3, Task&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, T3, ValueTask&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public async Task Call(T1 arg1, T2 arg2, T3 arg3, CancellationToken token)
///         {
///             await this.action.InvokeAsync(arg1, arg2, arg3, token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncAction<in T1, in T2, in T3>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg1">The first parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous action with four parameters and with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
/// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
/// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
/// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Action&lt;T1, T2, T3, T4&gt;</c></item>
///         <item><c>Action&lt;T1, T2, T3, T4, CancellationToken&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, Task&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, CancellationToken, Task&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, ValueTask&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, CancellationToken, ValueTask&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service&lt;T1, T2, T3, T4&gt;
///     {
///         private readonly IAsyncAction&lt;T1, T2, T3, T4&gt; action;
///     <br/>
///         public Service(Action&lt;T1, T2, T3, T4&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, T3, T4, Task&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, T3, T4, ValueTask&gt; action)
///         {
///             this.action = action.AsAsyncAction();
///         }
///     <br/>
///         public async Task Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token)
///         {
///             await this.action.InvokeAsync(arg1, arg2, arg3, arg4, token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncAction<in T1, in T2, in T3, in T4>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg1">The first parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default);
}
