# IAsyncFunc

`IAsyncFunc<TResult>` and its generic variants (`IAsyncFunc<T, TResult>`, `IAsyncFunc<T1, T2, TResult>`, etc.) provide
unified async wrappers for result-returning delegates.
This allows library authors to accept both synchronous and asynchronous callbacks through a common interface,
simplifying API design while maintaining async/await compatibility.

## Overview

```csharp
public interface IAsyncFunc<TResult>
{
    Task<TResult> InvokeAsync(CancellationToken token = default);
}

public interface IAsyncFunc<in T, TResult>
{
    Task<TResult> InvokeAsync(T arg, CancellationToken token = default);
}

// Additional variants: IAsyncFunc<T1, T2, TResult>, ... up to 8 parameters
```

### Supported Delegate Types

For each interface variant, the following delegate types can be converted using `AsAsyncFunc()`:

| Description                                         | No Parameters                                 | One Parameter                                    | Many Parameters                                            |
|-----------------------------------------------------|-----------------------------------------------|--------------------------------------------------|------------------------------------------------------------|
| Synchronous `Func`                                  | `Func<TResult>`                               | `Func<T, TResult>`                               | `Func<T1, T2, ..., TResult>`                               |
| Synchronous `Func` with cancellation                | `Func<CancellationToken, TResult>`            | `Func<T, CancellationToken, TResult>`            | `Func<T1, T2, ..., CancellationToken, TResult>`            |
| Asynchronous `Task<TResult>`                        | `Func<Task<TResult>>`                         | `Func<T, Task<TResult>>`                         | `Func<T1, T2, ..., Task<TResult>>`                         |
| Asynchronous `Task<TResult>` with cancellation      | `Func<CancellationToken, Task<TResult>>`      | `Func<T, CancellationToken, Task<TResult>>`      | `Func<T1, T2, ..., CancellationToken, Task<TResult>>`      |
| Asynchronous `ValueTask<TResult>`                   | `Func<ValueTask<TResult>>`                    | `Func<T, ValueTask<TResult>>`                    | `Func<T1, T2, ..., ValueTask<TResult>>`                    |
| Asynchronous `ValueTask<TResult>` with cancellation | `Func<CancellationToken, ValueTask<TResult>>` | `Func<T, CancellationToken, ValueTask<TResult>>` | `Func<T1, T2, ..., CancellationToken, ValueTask<TResult>>` |
| Resulting `IAsyncFunc` type                         | `IAsyncFunc<TResult>`                         | `IAsyncFunc<T, TResult>`                         | `IAsyncFunc<T1, T2, ..., TResult>`                         |

### Example

```csharp
public class AsyncLazy<T>
{
    private readonly IAsyncFunc<T> factory;
    private T value;
    private bool isValueSet;

    // Accept synchronous factory
    public AsyncLazy(Func<T> factory)
    {
        this.factory = factory.AsAsyncFunc();
    }

    // Accept async factory
    public AsyncLazy(Func<CancellationToken, Task<T>> factory)
    {
        this.factory = factory.AsAsyncFunc();
    }

    // Accept ValueTask-based async factory
    public AsyncLazy(Func<CancellationToken, ValueTask<T>> factory)
    {
        this.factory = factory.AsAsyncFunc();
    }

    public async Task<T> GetValueAsync(CancellationToken token)
    {
        if (this.isValueSet)
        {
            return this.value;
        }

        // All delegate types are invoked uniformly
        this.value = await this.factory.InvokeAsync(token);
        this.isValueSet = true;
    }
}

// Usage with different delegate types
var configLazy = new AsyncLazy<Config>(() => LoadConfigFromDisk());

// Both are called the same way, regardless of the underlying delegate type
Config config = await configLazy.GetValueAsync(CancellationToken.None);
```

## Cancellation Token Support

The `CancellationToken` passed to `InvokeAsync` is handled differently depending on the wrapped delegate type.
`CancellationToken` _should_ be the last parameter. If not, then the token would be treated as a regular parameter which
may be confusing.

- **For both delegates**: The token is checked before execution; if already canceled, `OperationCanceledException` is thrown immediately
- **Delegates with `CancellationToken` parameter**: The token is passed directly to the delegate
- **Delegates without `CancellationToken` parameter**: The token is **not** passed to the delegate

```csharp
var cts = new CancellationTokenSource();
cts.Cancel();

Func<int> func = () => 42;
IAsyncFunc<int> asyncFunc = func.AsAsyncFunc();

try
{
    // Throws immediately because token is already cancelled
    int result = await asyncFunc.InvokeAsync(cts.Token);
}
catch (OperationCanceledException)
{
    // Cancellation was requested before execution
}
```

## Synchronous Execution Behavior

Synchronous delegates (`Func<TResult>`, `Func<T, TResult>`, etc.) are executed via `Task.Run()`, which schedules
execution on the thread pool.

```csharp
Func<int> func = () =>
{
    // This code runs on a thread pool thread, not the calling thread
    Thread.Sleep(1000);
    return 42;
};
IAsyncFunc<int> asyncFunc = func.AsAsyncFunc();

// This will return immediately
var task = asyncFunc.InvokeAsync();

// Then this will wait for the Thread.Sleep to finish
int result = await task;
```

### Implications

- **Thread context switch**: The delegate executes on a different thread than the caller
- **No synchronization context**: The delegate does not capture or use `SynchronizationContext`
- **Thread pool scheduling overhead**: There is overhead associated with scheduling work on the thread pool

For delegates that are already async (`Func<Task<TResult>>`, `Func<ValueTask<TResult>>`, etc.), no additional thread
pool scheduling occurs - they execute directly.

## Best Practices

### Recommended Delegate Types

When writing library code it is recommended to provide support for these three delegate types:

- `Func<T1, ..., TResult>`
- `Func<T1, ..., CancellationToken, Task<TResult>>`
- `Func<T1, ..., CancellationToken, ValueTask<TResult>>`

Other delegate types may be necessary for specific situations but generally aren't used as often.
Typically synchronous delegates do not need a `CancellationToken`, and asynchronous delegates always need a
`CancellationToken`.

### Ignoring Parameters with Wrapper Delegates

When you need to adapt a simpler delegate to an interface that expects parameters, wrap it:

```csharp
public class Validator<T>
{
    private readonly IAsyncFunc<T, ValidationOptions, bool> validate;

    // Simple validator that doesn't need options
    public Validator(Func<T, bool> validate)
    {
        Func<T, ValidationOptions, bool> upcastedFunc = (value, _) => validate(value);
        this.validate = upcastedFunc.AsAsyncFunc();
    }

    // Full validator with options support
    public Validator(Func<T, ValidationOptions, bool> validate)
    {
        this.validate = validate.AsAsyncFunc();
    }

    public async Task<bool> ValidateAsync(T value, ValidationOptions options, CancellationToken token)
    {
        // The simple validator will ignore options, but that's ok
        return await this.validate.InvokeAsync(value, options, token);
    }
}
```

### Performance Considerations

- **Async overhead**: The async wrapper adds overhead that may not be suitable for hot-path code executed thousands of
  times per second
- **`Task.Run()` for sync delegates**: Synchronous delegates incur thread pool scheduling overhead
- **`ValueTask<TResult>` for non-allocating operations**: Typically `ValueTask<TResult>` is used for hot-path asynchronous code for
  their lower allocation overhead. This performance is dropped as the `ValueTask<TResult>` is converted to a `Task<TResult>`

