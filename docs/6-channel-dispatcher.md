# Channel Dispatcher

Channel dispatchers use `System.Threading.Channels` to queue and process work items sequentially on a background task.
They implement `IHostedDispatcher`, which requires explicit start/stop lifecycle management.

## Overview

```csharp
// Unbounded - no limit on queued operations
IHostedDispatcher dispatcher = Dispatcher.CreateUnboundedChannelDispatcher();

// Bounded - applies backpressure when the queue reaches capacity
IHostedDispatcher dispatcher = Dispatcher.CreateBoundedChannelDispatcher(capacity: 100);
```

### Example

```csharp
IHostedDispatcher dispatcher = Dispatcher.CreateUnboundedChannelDispatcher();

// Must be started before dispatching work
await dispatcher.StartAsync();

await dispatcher.InvokeAsync(() =>
{
    // This code runs on the dispatcher's background task
    dispatcher.AssertAccess(); // Will not throw
});

int result = await dispatcher.InvokeAsync(() => ComputeValue());

await dispatcher.InvokeAsync(async () =>
{
    await LoadDataAsync();
});

// Stop the dispatcher when done
await dispatcher.StopAsync();
```

## Lifecycle Management

Channel dispatchers implement `IHostedDispatcher` and must be explicitly started and stopped:

```csharp
// Start the background processing loop
await dispatcher.StartAsync();

// ... dispatch work ...

// Signal the dispatcher to stop processing
await dispatcher.StopAsync();
```

- `StartAsync` launches the background task that reads from the channel and executes operations
- `StopAsync` signals cancellation and waits for the background task to complete
- The `CancellationToken` passed to `StartAsync` only cancels the start operation itself; once started, `StopAsync`
  must be called to shut down the dispatcher

## Unbounded Channel Dispatcher

Created via `Dispatcher.CreateUnboundedChannelDispatcher()`.

- Allows an unlimited number of operations to be queued
- `InvokeAsync` never blocks waiting for queue capacity
- Suitable when the producer rate is expected to stay manageable or when backpressure is not a concern

## Bounded Channel Dispatcher

Created via `Dispatcher.CreateBoundedChannelDispatcher(capacity)`.

- Limits the number of queued operations to the specified capacity
- When the queue is full, `InvokeAsync` callers will asynchronously wait until space becomes available
- Provides natural backpressure to prevent unbounded memory growth from a fast producer

```csharp
// Only allow 10 operations to be queued at a time
IHostedDispatcher dispatcher = Dispatcher.CreateBoundedChannelDispatcher(capacity: 10);
await dispatcher.StartAsync();

// If 10 operations are already queued, this will wait until one completes
_ = dispatcher.InvokeAsync(() => ProcessItem());
```

## How It Works

The channel dispatcher runs a background loop that:

1. Reads the next operation from the channel
2. Executes it on the background task
3. Propagates the result (or exception) back to the caller waiting on the `InvokeAsync` task
4. Repeats until cancellation is requested via `StopAsync`

Each dispatcher instance has a unique identity tracked via `AsyncLocal<int?>`, which allows `CheckAccess()` to determine
whether the calling code is already running inside the dispatcher's processing loop.

## CheckAccess and AssertAccess

These work the same as the [SynchronizationContext Dispatcher](5-synchronization-context-dispatcher.md), but the
identity check uses an `AsyncLocal` value rather than `SynchronizationContext.Current`:

```csharp
if (dispatcher.CheckAccess())
{
    // Already on the dispatcher's background task
    UpdateState();
}
else
{
    await dispatcher.InvokeAsync(() => UpdateState());
}
```

When already inside the dispatcher, `InvokeAsync` executes the work item synchronously and returns immediately,
avoiding a round-trip through the channel.

## InvokeAsync Overloads

| Overload                                      | Description                                  |
|-----------------------------------------------|----------------------------------------------|
| `InvokeAsync(Action)`                         | Dispatches a synchronous action              |
| `InvokeAsync(Func<Task>)`                     | Dispatches an async action                   |
| `InvokeAsync<TResult>(Func<TResult>)`         | Dispatches a synchronous function            |
| `InvokeAsync<TResult>(Func<Task<TResult>>)`   | Dispatches an async function                 |

## Exception Handling

Exceptions thrown by work items are handled as follows:

- The exception is raised via the `UnhandledException` event
- The exception is propagated back to the caller through the dispatched operation's `Task`
- The dispatcher continues processing the next queued operation (exceptions do not stop the background loop)

```csharp
dispatcher.UnhandledException += (sender, e) =>
{
    Console.WriteLine($"Unhandled: {e.ExceptionObject}");
};
```

## When to Use

Channel dispatchers are a good fit when:

- You need explicit start/stop lifecycle management (e.g., integration with `IHostedService`)
- Want fire-and-forget execution
- You want backpressure support to limit queued operations (bounded variant)
- You prefer channel-based queuing semantics over `SynchronizationContext`

Consider using the [SynchronizationContext Dispatcher](5-synchronization-context-dispatcher.md) instead when you want a
simpler dispatcher that doesn't require lifecycle management.
