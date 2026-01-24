namespace ZCrew.Extensions.Tasks;

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous function with <see langword="async"/> semantics regardless of
///     the delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Func&lt;TResult&gt;</c></item>
///         <item><c>Func&lt;CancellationToken, TResult&gt;</c></item>
///         <item><c>Func&lt;Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;CancellationToken, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;ValueTask&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;CancellationToken, ValueTask&lt;TResult&gt;&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service&lt;TResult&gt;
///     {
///         private readonly IAsyncFunc&lt;TResult&gt; func;
///     <br/>
///         public Service(Func&lt;TResult&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;Task&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;ValueTask&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public async Task&lt;TResult&gt; Call(CancellationToken token)
///         {
///             await this.func.InvokeAsync(token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncFunc<TResult>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task<TResult> InvokeAsync(CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous function with a parameter with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Func&lt;T, TResult&gt;</c></item>
///         <item><c>Func&lt;T, CancellationToken, TResult&gt;</c></item>
///         <item><c>Func&lt;T, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T, CancellationToken, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T, ValueTask&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T, CancellationToken, ValueTask&lt;TResult&gt;&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service&lt;T, TResult&gt;
///     {
///         private readonly IAsyncFunc&lt;T, TResult&gt; func;
///     <br/>
///         public Service(Func&lt;T, TResult&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;T, Task&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;T, ValueTask&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public async Task&lt;TResult&gt;&gt; Call(T arg, CancellationToken token)
///         {
///             await this.func.InvokeAsync(arg, token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncFunc<in T, TResult>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg">The parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task<TResult> InvokeAsync(T arg, CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous function with two parameters with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Func&lt;T1, T2, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, CancellationToken, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, CancellationToken, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, ValueTask&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, CancellationToken, ValueTask&lt;TResult&gt;&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service&lt;T1, T2, TResult&gt;
///     {
///         private readonly IAsyncFunc&lt;T1, T2, TResult&gt; func;
///     <br/>
///         public Service(Func&lt;T1, T2, TResult&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, Task&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, ValueTask&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public async Task&lt;TResult&gt;&gt; Call(T1 arg1, T2 arg2, CancellationToken token)
///         {
///             await this.func.InvokeAsync(arg1, arg2, token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncFunc<in T1, in T2, TResult>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg1">The first parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task<TResult> InvokeAsync(T1 arg1, T2 arg2, CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous function with three parameters with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Func&lt;T1, T2, T3, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, CancellationToken, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, CancellationToken, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, ValueTask&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, CancellationToken, ValueTask&lt;TResult&gt;&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service&lt;T1, T2, T3, TResult&gt;
///     {
///         private readonly IAsyncFunc&lt;T1, T2, T3, TResult&gt; func;
///     <br/>
///         public Service(Func&lt;T1, T2, T3, TResult&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, T3, Task&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, T3, ValueTask&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public async Task&lt;TResult&gt;&gt; Call(T1 arg1, T2 arg2, T3 arg3, CancellationToken token)
///         {
///             await this.func.InvokeAsync(arg1, arg2, arg3, token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncFunc<in T1, in T2, in T3, TResult>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg1">The first parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous function with four parameters with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Func&lt;T1, T2, T3, T4, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, CancellationToken, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, CancellationToken, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, ValueTask&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, CancellationToken, ValueTask&lt;TResult&gt;&gt;</c></item>
///     </list>
/// </remarks>
/// <example>
/// <code>
///     class Service&lt;T1, T2, T3, T4, TResult&gt;
///     {
///         private readonly IAsyncFunc&lt;T1, T2, T3, T4, TResult&gt; func;
///     <br/>
///         public Service(Func&lt;T1, T2, T3, T4, TResult&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, T3, T4, Task&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public Service(Func&lt;T1, T2, T3, T4, ValueTask&lt;TResult&gt;&gt; func)
///         {
///             this.func = func.AsAsyncFunc();
///         }
///     <br/>
///         public async Task&lt;TResult&gt;&gt; Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token)
///         {
///             await this.func.InvokeAsync(arg1, arg2, arg3, arg4, token);
///         }
///     }
/// </code>
/// </example>
public interface IAsyncFunc<in T1, in T2, in T3, in T4, TResult>
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
    Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous function with five parameters with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Func&lt;T1, T2, T3, T4, T5, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, CancellationToken, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, CancellationToken, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, ValueTask&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, CancellationToken, ValueTask&lt;TResult&gt;&gt;</c></item>
///     </list>
/// </remarks>
public interface IAsyncFunc<in T1, in T2, in T3, in T4, in T5, TResult>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg1">The first parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous function with six parameters with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, CancellationToken, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, CancellationToken, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, ValueTask&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, CancellationToken, ValueTask&lt;TResult&gt;&gt;</c></item>
///     </list>
/// </remarks>
public interface IAsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, TResult>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg1">The first parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task<TResult> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, CancellationToken token = default);
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous function with seven parameters with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, CancellationToken, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, ValueTask&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, CancellationToken, ValueTask&lt;TResult&gt;&gt;</c></item>
///     </list>
/// </remarks>
public interface IAsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, TResult>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg1">The first parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task<TResult> InvokeAsync(
        T1 arg1,
        T2 arg2,
        T3 arg3,
        T4 arg4,
        T5 arg5,
        T6 arg6,
        T7 arg7,
        CancellationToken token = default
    );
}

/// <summary>
///     Wrapper to invoke a synchronous or asynchronous function with eight parameters with <see langword="async"/>
///     semantics regardless of the delegate type.
/// </summary>
/// <remarks>
///     This may be useful when writing library code that can accept multiple delegates from:
///     <list type="bullet">
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, TResult&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, ValueTask&lt;TResult&gt;&gt;</c></item>
///         <item><c>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, ValueTask&lt;TResult&gt;&gt;</c></item>
///     </list>
/// </remarks>
public interface IAsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, TResult>
{
    /// <summary>
    ///     Invoke the inner delegate with <see langword="async"/> semantics.
    /// </summary>
    /// <param name="arg1">The first parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this wrapper encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this wrapper encapsulates.</param>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <exception cref="OperationCanceledException">If <paramref name="token"/> has been canceled.</exception>
    Task<TResult> InvokeAsync(
        T1 arg1,
        T2 arg2,
        T3 arg3,
        T4 arg4,
        T5 arg5,
        T6 arg6,
        T7 arg7,
        T8 arg8,
        CancellationToken token = default
    );
}
