using Nito.AsyncEx;
using ZCrew.Extensions.Tasks.Dispatching;

namespace ZCrew.Extensions.Tasks.UnitTests.Dispatching;

public class BackgroundDispatcherTests
{
    [Fact(Timeout = 5000)]
    public async Task StartAsync_WhenCalled_ShouldInvokeExecuteAsync()
    {
        // Arrange
        var dispatcher = new TestBackgroundDispatcher();

        // Act
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        await dispatcher.ExecuteEntered.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.True(dispatcher.ExecuteEntered.IsSet);

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task StartAsync_WhenTokenIsPreCanceled_ShouldThrow()
    {
        // Arrange
        var dispatcher = new TestBackgroundDispatcher();
        var token = new CancellationToken(canceled: true);

        // Act
        var startAsync = () => dispatcher.StartAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(startAsync);
    }

    [Fact]
    public async Task StartAsync_WhenExecuteAsyncCompletesSynchronously_ShouldReturnCompleted()
    {
        // Arrange
        var dispatcher = new TestBackgroundDispatcher { CompleteSynchronously = true };

        // Act
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.True(dispatcher.ExecuteEntered.IsSet);
    }

    [Fact(Timeout = 5000)]
    public async Task StopAsync_WhenCalled_ShouldCancelTokenPassedToExecuteAsync()
    {
        // Arrange
        var dispatcher = new TestBackgroundDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        await dispatcher.ExecuteEntered.WaitAsync(TestContext.Current.CancellationToken);

        // Act
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.True(dispatcher.ExecuteToken.IsCancellationRequested);
    }

    [Fact(Timeout = 5000)]
    public async Task StopAsync_WhenStopTokenIsCanceled_ShouldReturnWithoutWaitingForExecuteAsync()
    {
        // Arrange
        var dispatcher = new TestBackgroundDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        await dispatcher.ExecuteEntered.WaitAsync(TestContext.Current.CancellationToken);
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act
        await dispatcher.StopAsync(cts.Token);

        // Assert
        Assert.True(dispatcher.ExecuteToken.IsCancellationRequested);
    }
}

file class TestBackgroundDispatcher : BackgroundDispatcher
{
    private readonly TaskCompletionSource keepAlive = new();

    public AsyncManualResetEvent ExecuteEntered { get; } = new();
    public CancellationToken ExecuteToken { get; private set; }
    public bool CompleteSynchronously { get; init; }

    public override bool CheckAccess()
    {
        return false;
    }

    public override Task InvokeAsync(Action workItem)
    {
        throw new NotSupportedException();
    }

    public override Task InvokeAsync(Func<Task> workItem)
    {
        throw new NotSupportedException();
    }

    public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
    {
        throw new NotSupportedException();
    }

    public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
    {
        throw new NotSupportedException();
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        ExecuteToken = token;
        ExecuteEntered.Set();

        if (CompleteSynchronously)
        {
            return;
        }

        await using var registration = token.Register(() => this.keepAlive.TrySetResult());
        await this.keepAlive.Task;
    }
}
