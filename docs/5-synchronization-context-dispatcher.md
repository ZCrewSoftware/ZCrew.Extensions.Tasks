# SynchronizationContext Dispatcher

The `SynchronizationContext` dispatcher serializes work items onto a single logical execution context, ensuring only one
operation runs at a time. It is the default dispatcher returned by `Dispatcher.CreateDefault()`.

## Overview

```csharp
IDispatcher dispatcher = Dispatcher.CreateDefault();
// or
IDispatcher dispatcher = Dispatcher.CreateSynchronizationContextDispatcher();
```

This dispatcher is backed by a custom `SynchronizationContext` derived from Blazor's
`RendererSynchronizationContext`. It does not require a dedicated background thread or explicit start/stop lifecycle
management, making it suitable for lightweight single-threaded coordination.

### Example

```csharp
IDispatcher dispatcher = Dispatcher.CreateDefault();

// Dispatch work from any thread
await dispatcher.InvokeAsync(() =>
{
    // This code runs inside the dispatcher's context
    dispatcher.AssertAccess(); // Will not throw
});

// Return values from dispatched work
int result = await dispatcher.InvokeAsync(() => ComputeValue());

// Dispatch async work
await dispatcher.InvokeAsync(async () =>
{
    await LoadDataAsync();
    // Continuations also run inside the dispatcher's context
    dispatcher.AssertAccess(); // Will not throw
});
```

## How It Works

The dispatcher maintains an internal task queue. When `InvokeAsync` is called:

1. If the queue is idle, the work item executes synchronously on the calling thread with the dispatcher's
   `SynchronizationContext` set as the current context
2. If the queue is busy, the work item is posted to the queue and executes after all preceding items complete

This means `await` continuations inside dispatched async work items are automatically marshaled back to the dispatcher's
context, similar to how `await` works on a UI thread.

## CheckAccess and AssertAccess

Use `CheckAccess()` to determine if the calling code is already running inside the dispatcher, and `AssertAccess()` to
throw if it is not:

```csharp
if (dispatcher.CheckAccess())
{
    // Already on the dispatcher - safe to access dispatcher-bound state directly
    UpdateState();
}
else
{
    // Not on the dispatcher - must dispatch
    await dispatcher.InvokeAsync(() => UpdateState());
}
```

When already inside the dispatcher, calling `InvokeAsync` executes the work item synchronously and returns a completed
task, avoiding unnecessary queuing overhead. So the check in the above example is not necessary.

## InvokeAsync Overloads

| Overload                                      | Description                                  |
|-----------------------------------------------|----------------------------------------------|
| `InvokeAsync(Action)`                         | Dispatches a synchronous action              |
| `InvokeAsync(Func<Task>)`                     | Dispatches an async action                   |
| `InvokeAsync<TResult>(Func<TResult>)`         | Dispatches a synchronous function            |
| `InvokeAsync<TResult>(Func<Task<TResult>>)`   | Dispatches an async function                 |

## Exception Handling

Exceptions thrown by work items are propagated back to the caller through the returned `Task`. If an exception occurs
in a work item that was posted to the queue (rather than executed synchronously), the exception is surfaced through the
`UnhandledException` event:

```csharp
dispatcher.UnhandledException += (sender, e) =>
{
    Console.WriteLine($"Unhandled: {e.ExceptionObject}");
};
```

## When to Use

The `SynchronizationContext` dispatcher is a good fit when:

- You need single-threaded coordination without a dedicated background thread
- You want `await` continuations to automatically stay on the dispatcher's context
- You don't need explicit start/stop lifecycle management
- You want the simplest dispatcher option

Consider using a [Channel Dispatcher](6-channel-dispatcher.md) instead when you need explicit lifecycle management via
`IHostedDispatcher`, or when you want backpressure support with a bounded queue.
