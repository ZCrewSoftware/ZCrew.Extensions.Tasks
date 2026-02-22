# IAsyncAction

`IAsyncAction` and its generic variants (`IAsyncAction<T>`, `IAsyncAction<T1, T2>`, etc.) provide unified async wrappers
for void-returning delegates.
This allows library authors to accept both synchronous and asynchronous callbacks through a common interface,
simplifying API design while maintaining async/await compatibility.

## Overview

```csharp
public interface IAsyncAction
{
    Task InvokeAsync(CancellationToken token = default);
}

public interface IAsyncAction<in T>
{
    Task InvokeAsync(T arg, CancellationToken token = default);
}

// Additional variants: IAsyncAction<T1, T2>, IAsyncAction<T1, T2, T3>, ... up to 8 parameters
```

### Supported Delegate Types

For each interface variant, the following delegate types can be converted using `AsAsyncAction()`:

| Description                                | No Parameters                        | One Parameter                           | Many Parameters                                   |
|--------------------------------------------|--------------------------------------|-----------------------------------------|---------------------------------------------------|
| Synchronous `Action`                       | `Action`                             | `Action<T>`                             | `Action<T1, T2, ...>`                             |
| Synchronous `Action` with cancellation     | `Action<CancellationToken>`          | `Action<T, CancellationToken>`          | `Action<T1, T2, ..., CancellationToken>`          |
| Asynchronous `Task`                        | `Func<Task>`                         | `Func<T, Task>`                         | `Func<T1, T2, ..., Task>`                         |
| Asynchronous `Task` with cancellation      | `Func<CancellationToken, Task>`      | `Func<T, CancellationToken, Task>`      | `Func<T1, T2, ..., CancellationToken, Task>`      |
| Asynchronous `ValueTask`                   | `Func<ValueTask>`                    | `Func<T, ValueTask>`                    | `Func<T1, T2, ..., ValueTask>`                    |
| Asynchronous `ValueTask` with cancellation | `Func<CancellationToken, ValueTask>` | `Func<T, CancellationToken, ValueTask>` | `Func<T1, T2, ..., CancellationToken, ValueTask>` |
| Resulting `IAsyncAction` type              | `IAsyncAction`                       | `IAsyncAction<T>`                       | `IAsyncAction<T1, T2, ...>`                       |

### Example

```csharp
public class NotificationService
{
    private readonly List<IAsyncAction<string>> onNotifyHandlers;

    // Accept synchronous callback
    public void Register(Action<string> onNotify)
    {
        this.onNotifyHandlers.Add(onNotify.AsAsyncAction());
    }

    // Accept async callback
    public void Register(Func<string, Task> onNotify)
    {
        this.onNotifyHandlers.Add(onNotify.AsAsyncAction());
    }

    // Accept async callback with cancellation
    public void Register(Func<string, CancellationToken, Task> onNotify)
    {
        this.onNotifyHandlers.Add(onNotify.AsAsyncAction());
    }

    // Accept ValueTask-based async callback
    public void Register(Func<string, CancellationToken, ValueTask> onNotify)
    {
        this.onNotifyHandlers.Add(onNotify.AsAsyncAction());
    }

    public async Task SendNotificationAsync(string message, CancellationToken token)
    {
        foreach (var onNotifyHandler in onNotifyHandlers)
        {
            // All delegate types are invoked uniformly
            await onNotifyHandler.InvokeAsync(message, token);
        }
    }
}

var service = new NotificationService(msg => Console.WriteLine(msg));

// Usage with different delegate types
service.Register(msg => Console.WriteLine(msg));
service.Register(async (msg, ct) => await SendEmailAsync(msg, ct));

// Call all handlers, regardless of the underlying delegate type
await service.SendNotificationAsync("Hello", CancellationToken.None);
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

Action action = () => Console.WriteLine("test");
IAsyncAction asyncAction = action.AsAsyncAction();

try
{
    // Throws immediately because token is already cancelled
    await asyncAction.InvokeAsync(cts.Token);
}
catch (OperationCanceledException)
{
    // Cancellation was requested before execution
}
```

## Synchronous Execution Behavior

Synchronous delegates (`Action`, `Action<T>`, etc.) are executed via `Task.Run()`, which schedules execution on the
thread pool.

```csharp
Action action = () =>
{
    // This code runs on a thread pool thread, not the calling thread
    Thread.Sleep(1000);
};
IAsyncAction asyncAction = action.AsAsyncAction();

// This will return immediately
var task = asyncAction.InvokeAsync();

// Then this will wait for the Thread.Sleep to finish
await task;
```

### Implications

- **Thread context switch**: The delegate executes on a different thread than the caller
- **No synchronization context**: The delegate does not capture or use `SynchronizationContext`
- **Thread pool scheduling overhead**: There is overhead associated with scheduling work on the thread pool

For delegates that are already async (`Func<Task>`, `Func<ValueTask>`, etc.), no additional thread pool scheduling
occurs - they execute directly.

## Best Practices

### Recommended Delegate Types

When writing library code it is recommended to provide support for these three delegate types:

- `Action<T1, ...>`
- `Func<T1, ..., CancellationToken, Task>`
- `Func<T1, ..., CancellationToken, ValueTask>`

Other delegate types may be necessary for specific situations but generally aren't used as often.
Typically synchronous delegates do not need a `CancellationToken`, and asynchronous delegates always need a
`CancellationToken`.

### Ignoring Parameters with Wrapper Delegates

When you need to adapt a simpler delegate to an interface that expects parameters, wrap it:

```csharp
private readonly List<IAsyncAction<string>> handlers = [];

// Method receives an Action but upcasts it to an Action<string>
public void Register(Action action)
{
    Action<string> upcastedAction = _ => action;
    handlers.Add(upcastedAction.AsAsyncAction());
}

// Typical registration
public void Register(Action<string> action)
{
    handlers.Add(action.AsAsyncAction());
}

public async Task HandleAsync(string parameter)
{
    foreach (var handler in this.handlers)
    {
        // Some handlers will drop the parameter but that is ok
        await handler.InvokeAsync(parameter);
    }
}
```

### Performance Considerations

- **Async overhead**: The async wrapper adds overhead that may not be suitable for hot-path code executed thousands of
  times per second
- **`Task.Run()` for sync delegates**: Synchronous delegates incur thread pool scheduling overhead
- **`ValueTask` for non-allocating operations**: Typically `ValueTask` is used for hot-path asynchronous code for
  their lower allocation overhead. This performance is dropped as the `ValueTask` is converted to a `Task`

