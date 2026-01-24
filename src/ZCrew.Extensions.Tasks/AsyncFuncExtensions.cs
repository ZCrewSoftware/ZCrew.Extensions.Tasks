namespace ZCrew.Extensions.Tasks;

/// <summary>
///     Extensions for creating instances of the <see cref="IAsyncFunc{TResult}"/> type and variants accepting
///     parameters.
/// </summary>
public static class AsyncFuncExtensions
{
    #region IAsyncFunc<TResult>
    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncAction{TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<TResult> AsAsyncFunc<TResult>(this Func<TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncFunc<TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncAction{TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<TResult> AsAsyncFunc<TResult>(this Func<Task<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskFunc<TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncAction{TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<TResult> AsAsyncFunc<TResult>(this Func<ValueTask<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskFunc<TResult>(func);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncAction{TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<TResult> AsAsyncFunc<TResult>(this Func<CancellationToken, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncCancellationFunc<TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncAction{TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<TResult> AsAsyncFunc<TResult>(this Func<CancellationToken, Task<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskCancellationFunc<TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncAction{TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<TResult> AsAsyncFunc<TResult>(this Func<CancellationToken, ValueTask<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskCancellationFunc<TResult>(func);
    }
    #endregion IAsyncFunc<TResult>

    #region IAsyncFunc<T, TResult>
    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T, TResult> AsAsyncFunc<T, TResult>(this Func<T, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncFunc<T, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T, TResult> AsAsyncFunc<T, TResult>(this Func<T, Task<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskFunc<T, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T, TResult> AsAsyncFunc<T, TResult>(this Func<T, ValueTask<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskFunc<T, TResult>(func);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T, TResult> AsAsyncFunc<T, TResult>(this Func<T, CancellationToken, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncCancellationFunc<T, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T, TResult> AsAsyncFunc<T, TResult>(this Func<T, CancellationToken, Task<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskCancellationFunc<T, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T, TResult> AsAsyncFunc<T, TResult>(
        this Func<T, CancellationToken, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskCancellationFunc<T, TResult>(func);
    }
    #endregion IAsyncFunc<T, TResult>

    #region IAsyncFunc<T1, T2, TResult>
    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, TResult> AsAsyncFunc<T1, T2, TResult>(this Func<T1, T2, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncFunc<T1, T2, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, TResult> AsAsyncFunc<T1, T2, TResult>(this Func<T1, T2, Task<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskFunc<T1, T2, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, TResult> AsAsyncFunc<T1, T2, TResult>(this Func<T1, T2, ValueTask<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskFunc<T1, T2, TResult>(func);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, TResult> AsAsyncFunc<T1, T2, TResult>(
        this Func<T1, T2, CancellationToken, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncCancellationFunc<T1, T2, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, TResult> AsAsyncFunc<T1, T2, TResult>(
        this Func<T1, T2, CancellationToken, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskCancellationFunc<T1, T2, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, TResult> AsAsyncFunc<T1, T2, TResult>(
        this Func<T1, T2, CancellationToken, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskCancellationFunc<T1, T2, TResult>(func);
    }
    #endregion IAsyncFunc<T1, T2, TResult>

    #region IAsyncFunc<T1, T2, T3, TResult>
    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, TResult> AsAsyncFunc<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncFunc<T1, T2, T3, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, TResult> AsAsyncFunc<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskFunc<T1, T2, T3, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, TResult> AsAsyncFunc<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskFunc<T1, T2, T3, TResult>(func);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, TResult> AsAsyncFunc<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, CancellationToken, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncCancellationFunc<T1, T2, T3, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, TResult> AsAsyncFunc<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, CancellationToken, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskCancellationFunc<T1, T2, T3, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, TResult> AsAsyncFunc<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, CancellationToken, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskCancellationFunc<T1, T2, T3, TResult>(func);
    }
    #endregion IAsyncFunc<T1, T2, T3, TResult>

    #region IAsyncFunc<T1, T2, T3, T4, TResult>
    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, TResult> AsAsyncFunc<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncFunc<T1, T2, T3, T4, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, TResult> AsAsyncFunc<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskFunc<T1, T2, T3, T4, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, TResult> AsAsyncFunc<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskFunc<T1, T2, T3, T4, TResult>(func);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, TResult> AsAsyncFunc<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, CancellationToken, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncCancellationFunc<T1, T2, T3, T4, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, TResult> AsAsyncFunc<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, CancellationToken, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskCancellationFunc<T1, T2, T3, T4, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, TResult> AsAsyncFunc<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, CancellationToken, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskCancellationFunc<T1, T2, T3, T4, TResult>(func);
    }
    #endregion IAsyncFunc<T1, T2, T3, T4, TResult>

    #region IAsyncFunc<T1, T2, T3, T4, T5, TResult>
    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, TResult>(
        this Func<T1, T2, T3, T4, T5, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncFunc<T1, T2, T3, T4, T5, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, TResult>(
        this Func<T1, T2, T3, T4, T5, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskFunc<T1, T2, T3, T4, T5, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, TResult>(
        this Func<T1, T2, T3, T4, T5, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskFunc<T1, T2, T3, T4, T5, TResult>(func);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, TResult>(
        this Func<T1, T2, T3, T4, T5, CancellationToken, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncCancellationFunc<T1, T2, T3, T4, T5, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, TResult>(
        this Func<T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskCancellationFunc<T1, T2, T3, T4, T5, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, TResult>(
        this Func<T1, T2, T3, T4, T5, CancellationToken, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskCancellationFunc<T1, T2, T3, T4, T5, TResult>(func);
    }
    #endregion IAsyncFunc<T1, T2, T3, T4, T5, TResult>

    #region IAsyncFunc<T1, T2, T3, T4, T5, T6, TResult>
    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncFunc<T1, T2, T3, T4, T5, T6, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskFunc<T1, T2, T3, T4, T5, T6, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskFunc<T1, T2, T3, T4, T5, T6, TResult>(func);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, CancellationToken, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncCancellationFunc<T1, T2, T3, T4, T5, T6, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskCancellationFunc<T1, T2, T3, T4, T5, T6, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, CancellationToken, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskCancellationFunc<T1, T2, T3, T4, T5, T6, TResult>(func);
    }
    #endregion IAsyncFunc<T1, T2, T3, T4, T5, T6, TResult>

    #region IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>
    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, T7, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(func);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, TResult> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncCancellationFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskCancellationFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult> AsAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, ValueTask<TResult>> func
    )
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskCancellationFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(func);
    }
    #endregion IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, TResult>

    #region IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> AsAsyncFunc<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        T7,
        T8,
        TResult
    >(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> AsAsyncFunc<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        T7,
        T8,
        TResult
    >(this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> AsAsyncFunc<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        T7,
        T8,
        TResult
    >(this Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(func);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> AsAsyncFunc<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        T7,
        T8,
        TResult
    >(this Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new SyncCancellationFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> AsAsyncFunc<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        T7,
        T8,
        TResult
    >(this Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new TaskCancellationFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(func);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="func"/> with cancellation support as an
    ///     <see cref="IAsyncFunc{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="func"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="func"/> is <see langword="null"/>.</exception>
    public static IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> AsAsyncFunc<
        T1,
        T2,
        T3,
        T4,
        T5,
        T6,
        T7,
        T8,
        TResult
    >(this Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, ValueTask<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return new ValueTaskCancellationFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(func);
    }
    #endregion IAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
}
