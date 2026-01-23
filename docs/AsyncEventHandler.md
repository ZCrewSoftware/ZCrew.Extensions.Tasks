# AsyncEventHandler

`AsyncEventHandler` and `AsyncEventHandler<TEventArgs>` are async-compatible delegate types for events, similar to the
standard `EventHandler` but supporting `async`/`await` patterns. This is accomplished without using `async void` to
prevent crashing if an exception occurs.

## Overview

```csharp
public delegate Task AsyncEventHandler(object sender, EventArgs args, CancellationToken token);
public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs args, CancellationToken token);
```

### Example

```csharp
public class MyService
{
    public event AsyncEventHandler<string>? MessageReceived;

    public async Task ProcessMessageAsync(string args, CancellationToken token)
    {
        // Invoke all handlers sequentially, collecting any exceptions
        await MessageReceived.InvokeSequentialAsync(this, args, token);
    }
}

// Subscribe to the event
var service = new MyService();
service.MessageReceived += async (sender, args, token) =>
{
    await SaveToDbAsync(args, token);
};
service.MessageReceived += async (sender, args, token) =>
{
    await NotifyUsersAsync(args, token);
};
```

## Invoke Methods

| Method                  | Execution  | Exception Behavior                                 | Cancellation Support                          |
|-------------------------|------------|----------------------------------------------------|-----------------------------------------------|
| `InvokeAsync`           | Sequential | **Only the first synchronous exception is thrown** | At the start                                  |
| `InvokeSequentialAsync` | Sequential | Collects all exceptions, invokes all handlers      | At the start and between each handler         |
| `InvokeParallelAsync`   | Parallel   | Collects all exceptions, invokes all handlers      | At the start and before starting each handler |

## Exception Handling

### InvokeAsync

Synchronous exceptions thrown by any handler immediately propagate, and the remaining handlers are **not** invoked:

```csharp
handler += async (sender, args, token) => throw new Exception();
handler += async (sender, args, token) => await Task.Delay(TimeSpan.FromSeconds(100), token); // Not invoked

// Since the first handler threw an exception synchronously, the second handler will NOT be invoked
await handler.InvokeAsync(sender, args, token);
```

However, if the exception is thrown asynchronously or an exception-task is returned synchronously using
`Task.FromException(...)`, the remaining handlers are invoked.
The exceptions thrown here by the first two handlers will not be observed:

```csharp
handler += async (sender, args, token) =>
{
    await Task.Delay(TimeSpan.FromSeconds(5), token); // This causes the handler to yield
    throw new Exception();
};
handler += (sender, args, token) => Task.FromException(new Exception()); // Return an exception-task
handler += async (sender, args, token) => await Task.Delay(TimeSpan.FromSeconds(100), token); // Invoked!

// Since the first two handlers did not throw an exception synchronously, the third handler will be invoked
await handler.InvokeAsync(sender, args, token);
```

### InvokeSequentialAsync / InvokeParallelAsync

These methods collect exceptions from all handlers and throw an `AggregateException` after all handlers have completed:

```csharp
try
{
    await handler.InvokeSequentialAsync(sender, args, token);

    // or...

    await handler.InvokeParallelAsync(sender, args, token);
}
catch (AggregateException ex)
{
    foreach (var inner in ex.Flatten().InnerExceptions)
    {
        Console.WriteLine($"Handler failed: {inner.Message}");
    }
}
```

### Cancellation

All invocation methods check `CancellationToken` before invoking handlers.
The `InvokeAsync` is the only invocation method that does not check for cancellation **between** handlers in any way.
When cancellation is requested:

- `OperationCanceledException` is thrown immediately
- Remaining handlers are **not** invoked
- Any in-flight exceptions are not wrapped (cancellation takes priority)

```csharp
try
{
    await handler.InvokeSequentialAsync(sender, args, cancellationToken);
}
catch (OperationCanceledException)
{
    // Cancellation was requested
}
```

## Best Practices

### Unsubscribe to Prevent Resource Leaks

Like standard `EventHandler`, always unsubscribe from `AsyncEventHandler` when the subscriber is no longer needed:

```csharp
// Store the handler reference for later removal
AsyncEventHandler<string> handler = async (sender, args, token) => { ... };

service.MessageReceived += handler;

// Later, when disposing or cleaning up
service.MessageReceived -= handler;
```

### Handler Ordering

When using `InvokeSequentialAsync`, handlers are invoked in registration order.
However, the exact sequence can be difficult to predict in complex applications where multiple components subscribe to
the same event at different times.
If strict ordering is required for publicly accessible event handlers, consider using a dedicated message bus or
orchestration pattern instead.

Failure to unsubscribe can prevent objects from being garbage collected, leading to memory leaks.

### When to Use Alternatives

`AsyncEventHandler` is suitable for simple pub/sub scenarios within a single application.
Consider using dedicated solutions for:

- **Complex routing or filtering** - Use an in-memory message bus (e.g., MediatR, MassTransit InMemory)
- **Cross-process communication** - Use a message broker (e.g., RabbitMQ, Azure Service Bus)
- **Event sourcing or replay** - Use an event store or event aggregator
- **Guaranteed delivery** - Use a durable messaging system
