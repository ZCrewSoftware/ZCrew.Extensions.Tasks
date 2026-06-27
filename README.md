# ZCrew.Extensions.Tasks

Extensions to tasks including: unified async wrappers for delegates and async event handling.

## Installation

This package is available on NuGet as `ZCrew.Extensions.Tasks` for these frameworks:

- .NET 8.0
- .NET 9.0
- .NET 10.0

```
<PackageReference Include="ZCrew.Extensions.Tasks" />
```

## Unified Async Wrappers

This library provides unified async wrappers for delegates, allowing library authors to accept both synchronous and
asynchronous callbacks through a common interface.

### Using an Async Wrapper

Use `IAsyncAction<T>` to store any of these delegate types uniformly:

```csharp
using ZCrew.Extensions.Tasks;

public class NotificationService
{
    private readonly IAsyncAction<string> onNotification;

    // Accept a synchronous callback
    public NotificationService(Action<string> onNotification)
    {
        this.onNotification = onNotification.AsAsyncAction();
    }

    // Accept an async callback
    public NotificationService(Func<string, Task> onNotification)
    {
        this.onNotification = onNotification.AsAsyncAction();
    }

    // Accept an async callback with cancellation support
    public NotificationService(Func<string, CancellationToken, Task> onNotification)
    {
        this.onNotification = onNotification.AsAsyncAction();
    }

    public async Task NotifyAsync(string message, CancellationToken token = default)
    {
        await this.onNotification.InvokeAsync(message, token);
    }
}
```

## Async Event Handlers

Traditional C# events use `EventHandler` which is synchronous. This library provides `AsyncEventHandler` for async event
patterns.

### Defining an Async Event

```csharp
using ZCrew.Extensions.Tasks;

public class FileWatcher
{
    public event AsyncEventHandler<FileChangedEventArgs>? FileChanged;

    protected async Task OnFileChangedAsync(FileChangedEventArgs e, CancellationToken token)
    {
        await FileChanged.InvokeSequentialAsync(this, e, token);
    }
}

public class FileChangedEventArgs : EventArgs
{
    public string FilePath { get; init; } = string.Empty;
}
```

### Subscribing to Async Events

```csharp
var watcher = new FileWatcher();

watcher.FileChanged += async (sender, e, token) =>
{
    await ProcessFileAsync(e.FilePath, token);
};
```

### Invocation Methods

```csharp
// Default: handlers run one after another similar to EventHandler.Invoke
await FileChanged.InvokeAsync(this, args, token);

// Sequential: handlers run one after another
await FileChanged.InvokeSequentialAsync(this, args, token);

// Parallel: handlers run concurrently
await FileChanged.InvokeParallelAsync(this, args, token);
```
