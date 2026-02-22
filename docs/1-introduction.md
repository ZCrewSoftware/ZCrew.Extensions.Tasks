# Introduction

`ZCrew.Extensions.Tasks` is a .NET library that provides unified async wrappers for delegates and single-threaded
dispatching primitives. It allows library authors to accept both synchronous and asynchronous callbacks through common
interfaces, simplifying API design while maintaining full async/await compatibility.

## Features

### Async Event Handlers

Async-compatible event delegates that replace `EventHandler` with proper `async`/`await` support, avoiding `async void`.
Includes sequential, parallel, and fail-fast invocation strategies with cancellation support.

[Read more](2-async-event-handler.md)

### IAsyncAction

Unified async wrappers for void-returning delegates (`Action`, `Func<Task>`, `Func<ValueTask>`, and their cancellation
variants). Converts any delegate shape into a common `IAsyncAction` interface with support for up to 8 parameters.

[Read more](3-async-action.md)

### IAsyncFunc

Unified async wrappers for result-returning delegates (`Func<TResult>`, `Func<Task<TResult>>`,
`Func<ValueTask<TResult>>`, and their cancellation variants). Converts any delegate shape into a common
`IAsyncFunc<TResult>` interface with support for up to 8 parameters.

[Read more](4-async-func.md)

### SynchronizationContext Dispatcher

A dispatcher that serializes work items onto a single logical execution context using a custom
`SynchronizationContext`. Work items execute one at a time without requiring a dedicated background thread or explicit
lifecycle management.

[Read more](5-synchronization-context-dispatcher.md)

### Channel Dispatcher

Background dispatchers that use `System.Threading.Channels` to queue and process work items sequentially on a
background task. Available in unbounded and bounded variants, with the bounded variant applying backpressure when the
queue reaches capacity.

[Read more](6-channel-dispatcher.md)
