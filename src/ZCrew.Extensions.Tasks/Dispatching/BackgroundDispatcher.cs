namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     A <see cref="Dispatcher"/> base class that runs on a background task with start/stop lifecycle management.
/// </summary>
internal abstract class BackgroundDispatcher : Dispatcher, IHostedDispatcher
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private Task executeAsync = Task.CompletedTask;

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var task = ExecuteAsync(this.cancellationTokenSource.Token);
        if (task.IsCompleted)
        {
            await task;
            return;
        }

        this.executeAsync = task;
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken token = default)
    {
        try
        {
            await this.cancellationTokenSource.CancelAsync();
        }
        finally
        {
            await Task.WhenAny(this.executeAsync, Task.Delay(Timeout.Infinite, token));
        }
    }

    /// <summary>
    ///     Executes the dispatcher's background processing loop.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> that signals when the dispatcher should stop.</param>
    /// <returns>A <see cref="Task"/> that represents the lifetime of the background processing.</returns>
    protected abstract Task ExecuteAsync(CancellationToken token);
}
