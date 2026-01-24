namespace ZCrew.Extensions.Tasks;

/// <summary>
///     Extensions for creating instances of the <see cref="IAsyncAction"/> and variants accepting parameters.
/// </summary>
public static class AsyncActionExtensions
{
    #region IAsyncAction
    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> as an <see cref="IAsyncAction"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction AsAsyncAction(this Action action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncAction(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction AsAsyncAction(this Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskAction(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction"/>..
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction AsAsyncAction(this Func<ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskAction(action);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> with cancellation support as an <see cref="IAsyncAction"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction AsAsyncAction(this Action<CancellationToken> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncCancellationAction(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an <see cref="IAsyncAction"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction AsAsyncAction(this Func<CancellationToken, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskCancellationAction(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an <see cref="IAsyncAction"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction AsAsyncAction(this Func<CancellationToken, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskCancellationAction(action);
    }
    #endregion IAsyncAction

    #region IAsyncAction<T>
    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> as an <see cref="IAsyncAction{T}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T> AsAsyncAction<T>(this Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncAction<T>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T> AsAsyncAction<T>(this Func<T, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskAction<T>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T> AsAsyncAction<T>(this Func<T, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskAction<T>(action);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> with cancellation support as an <see cref="IAsyncAction{T}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T> AsAsyncAction<T>(this Action<T, CancellationToken> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncCancellationAction<T>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T> AsAsyncAction<T>(this Func<T, CancellationToken, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskCancellationAction<T>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T">The type of the parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T> AsAsyncAction<T>(this Func<T, CancellationToken, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskCancellationAction<T>(action);
    }
    #endregion IAsyncAction<T>

    #region IAsyncAction<T1, T2>
    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2> AsAsyncAction<T1, T2>(this Action<T1, T2> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncAction<T1, T2>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2> AsAsyncAction<T1, T2>(this Func<T1, T2, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskAction<T1, T2>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2> AsAsyncAction<T1, T2>(this Func<T1, T2, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskAction<T1, T2>(action);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2> AsAsyncAction<T1, T2>(this Action<T1, T2, CancellationToken> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncCancellationAction<T1, T2>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2> AsAsyncAction<T1, T2>(this Func<T1, T2, CancellationToken, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskCancellationAction<T1, T2>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2> AsAsyncAction<T1, T2>(this Func<T1, T2, CancellationToken, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskCancellationAction<T1, T2>(action);
    }
    #endregion IAsyncAction<T1, T2>

    #region IAsyncAction<T1, T2, T3>
    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3> AsAsyncAction<T1, T2, T3>(this Action<T1, T2, T3> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncAction<T1, T2, T3>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3> AsAsyncAction<T1, T2, T3>(this Func<T1, T2, T3, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskAction<T1, T2, T3>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3> AsAsyncAction<T1, T2, T3>(this Func<T1, T2, T3, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskAction<T1, T2, T3>(action);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3> AsAsyncAction<T1, T2, T3>(this Action<T1, T2, T3, CancellationToken> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncCancellationAction<T1, T2, T3>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3> AsAsyncAction<T1, T2, T3>(
        this Func<T1, T2, T3, CancellationToken, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskCancellationAction<T1, T2, T3>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3> AsAsyncAction<T1, T2, T3>(
        this Func<T1, T2, T3, CancellationToken, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskCancellationAction<T1, T2, T3>(action);
    }
    #endregion IAsyncAction<T1, T2, T3>

    #region IAsyncAction<T1, T2, T3, T4>
    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4> AsAsyncAction<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncAction<T1, T2, T3, T4>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4> AsAsyncAction<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskAction<T1, T2, T3, T4>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4> AsAsyncAction<T1, T2, T3, T4>(
        this Func<T1, T2, T3, T4, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskAction<T1, T2, T3, T4>(action);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4> AsAsyncAction<T1, T2, T3, T4>(
        this Action<T1, T2, T3, T4, CancellationToken> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncCancellationAction<T1, T2, T3, T4>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4> AsAsyncAction<T1, T2, T3, T4>(
        this Func<T1, T2, T3, T4, CancellationToken, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskCancellationAction<T1, T2, T3, T4>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4> AsAsyncAction<T1, T2, T3, T4>(
        this Func<T1, T2, T3, T4, CancellationToken, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskCancellationAction<T1, T2, T3, T4>(action);
    }
    #endregion IAsyncAction<T1, T2, T3, T4>

    #region IAsyncAction<T1, T2, T3, T4, T5>
    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5> AsAsyncAction<T1, T2, T3, T4, T5>(
        this Action<T1, T2, T3, T4, T5> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncAction<T1, T2, T3, T4, T5>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5> AsAsyncAction<T1, T2, T3, T4, T5>(
        this Func<T1, T2, T3, T4, T5, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskAction<T1, T2, T3, T4, T5>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5> AsAsyncAction<T1, T2, T3, T4, T5>(
        this Func<T1, T2, T3, T4, T5, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskAction<T1, T2, T3, T4, T5>(action);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5> AsAsyncAction<T1, T2, T3, T4, T5>(
        this Action<T1, T2, T3, T4, T5, CancellationToken> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncCancellationAction<T1, T2, T3, T4, T5>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5> AsAsyncAction<T1, T2, T3, T4, T5>(
        this Func<T1, T2, T3, T4, T5, CancellationToken, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskCancellationAction<T1, T2, T3, T4, T5>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5> AsAsyncAction<T1, T2, T3, T4, T5>(
        this Func<T1, T2, T3, T4, T5, CancellationToken, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskCancellationAction<T1, T2, T3, T4, T5>(action);
    }
    #endregion IAsyncAction<T1, T2, T3, T4, T5>

    #region IAsyncAction<T1, T2, T3, T4, T5, T6>
    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6> AsAsyncAction<T1, T2, T3, T4, T5, T6>(
        this Action<T1, T2, T3, T4, T5, T6> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncAction<T1, T2, T3, T4, T5, T6>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6> AsAsyncAction<T1, T2, T3, T4, T5, T6>(
        this Func<T1, T2, T3, T4, T5, T6, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskAction<T1, T2, T3, T4, T5, T6>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6> AsAsyncAction<T1, T2, T3, T4, T5, T6>(
        this Func<T1, T2, T3, T4, T5, T6, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskAction<T1, T2, T3, T4, T5, T6>(action);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6> AsAsyncAction<T1, T2, T3, T4, T5, T6>(
        this Action<T1, T2, T3, T4, T5, T6, CancellationToken> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncCancellationAction<T1, T2, T3, T4, T5, T6>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6> AsAsyncAction<T1, T2, T3, T4, T5, T6>(
        this Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskCancellationAction<T1, T2, T3, T4, T5, T6>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6> AsAsyncAction<T1, T2, T3, T4, T5, T6>(
        this Func<T1, T2, T3, T4, T5, T6, CancellationToken, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskCancellationAction<T1, T2, T3, T4, T5, T6>(action);
    }
    #endregion IAsyncAction<T1, T2, T3, T4, T5, T6>

    #region IAsyncAction<T1, T2, T3, T4, T5, T6, T7>
    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7>(
        this Action<T1, T2, T3, T4, T5, T6, T7> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncAction<T1, T2, T3, T4, T5, T6, T7>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7>(
        this Func<T1, T2, T3, T4, T5, T6, T7, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskAction<T1, T2, T3, T4, T5, T6, T7>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7>(
        this Func<T1, T2, T3, T4, T5, T6, T7, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskAction<T1, T2, T3, T4, T5, T6, T7>(action);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7>(
        this Action<T1, T2, T3, T4, T5, T6, T7, CancellationToken> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncCancellationAction<T1, T2, T3, T4, T5, T6, T7>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7>(
        this Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskCancellationAction<T1, T2, T3, T4, T5, T6, T7>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7>(
        this Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskCancellationAction<T1, T2, T3, T4, T5, T6, T7>(action);
    }
    #endregion IAsyncAction<T1, T2, T3, T4, T5, T6, T7>

    #region IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>
    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7, T8}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7, T8}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskAction<T1, T2, T3, T4, T5, T6, T7, T8>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> as an <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7, T8}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskAction<T1, T2, T3, T4, T5, T6, T7, T8>(action);
    }

    /// <summary>
    ///     Wraps a synchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7, T8}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new SyncCancellationAction<T1, T2, T3, T4, T5, T6, T7, T8>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7, T8}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new TaskCancellationAction<T1, T2, T3, T4, T5, T6, T7, T8>(action);
    }

    /// <summary>
    ///     Wraps an asynchronous <paramref name="action"/> with cancellation support as an
    ///     <see cref="IAsyncAction{T1, T2, T3, T4, T5, T6, T7, T8}"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <typeparam name="T1">The type of the first parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this wrapper encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this wrapper encapsulates.</typeparam>
    /// <returns>Wrapper to execute the <paramref name="action"/> with <see langword="async"/> semantics.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
    public static IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8> AsAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, ValueTask> action
    )
    {
        ArgumentNullException.ThrowIfNull(action);
        return new ValueTaskCancellationAction<T1, T2, T3, T4, T5, T6, T7, T8>(action);
    }
    #endregion IAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>
}
