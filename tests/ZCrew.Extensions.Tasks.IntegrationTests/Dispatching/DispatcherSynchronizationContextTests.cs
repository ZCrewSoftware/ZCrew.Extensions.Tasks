using Nito.AsyncEx;
using NSubstitute;
using ZCrew.Extensions.Tasks.Dispatching;

namespace ZCrew.Extensions.Tasks.IntegrationTests.Dispatching;

public class DispatcherSynchronizationContextTests
{
    [Fact]
    public async Task InvokeAsync_Action_WhenCalled_ShouldExecuteAction()
    {
        // Arrange
        var context = new DispatcherSynchronizationContext();
        var workItem = Substitute.For<Action>();

        // Act
        await context.InvokeAsync(workItem);

        // Assert
        workItem.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeAsync_Action_WhenCalled_ShouldSetSynchronizationContextCurrent()
    {
        // Arrange
        var context = new DispatcherSynchronizationContext();
        SynchronizationContext? captured = null;

        // Act
        await context.InvokeAsync(() => captured = SynchronizationContext.Current);

        // Assert
        Assert.Same(context, captured);
    }

    [Fact]
    public async Task InvokeAsync_Action_WhenActionThrows_ShouldFaultReturnedTask()
    {
        // Arrange
        var context = new DispatcherSynchronizationContext();
        var expected = new InvalidOperationException("test");

        // Act
        var invokeAsync = () => context.InvokeAsync((Action)(() => throw expected));

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(invokeAsync);
        Assert.Same(expected, ex);
    }

    [Fact]
    public async Task InvokeAsync_Func_WhenCalled_ShouldReturnResult()
    {
        // Arrange
        var context = new DispatcherSynchronizationContext();
        var workItem = Substitute.For<Func<int>>();
        workItem.Invoke().Returns(42);

        // Act
        var result = await context.InvokeAsync(workItem);

        // Assert
        Assert.Equal(42, result);
        workItem.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeAsync_FuncTask_WhenCalled_ShouldComplete()
    {
        // Arrange
        var context = new DispatcherSynchronizationContext();
        var workItem = Substitute.For<Func<Task>>();
        workItem.Invoke().Returns(Task.CompletedTask);

        // Act
        await context.InvokeAsync(workItem);

        // Assert
        await workItem.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeAsync_FuncTaskResult_WhenCalled_ShouldReturnResult()
    {
        // Arrange
        var context = new DispatcherSynchronizationContext();
        var workItem = Substitute.For<Func<Task<int>>>();
        workItem.Invoke().Returns(Task.FromResult(42));

        // Act
        var result = await context.InvokeAsync(workItem);

        // Assert
        Assert.Equal(42, result);
        await workItem.Received(1).Invoke();
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_WhenMultipleConcurrentCalls_ShouldSerialize()
    {
        // Arrange
        var context = new DispatcherSynchronizationContext();
        var running = 0;
        var maxConcurrent = 0;
        var allDone = new AsyncManualResetEvent();
        var count = 0;
        const int total = 5;

        // Act
        var tasks = Enumerable
            .Range(0, total)
            .Select(_ =>
                context.InvokeAsync(async () =>
                {
                    var current = Interlocked.Increment(ref running);
                    if (current > Volatile.Read(ref maxConcurrent))
                    {
                        Interlocked.Exchange(ref maxConcurrent, current);
                    }

                    await Task.Yield();
                    Interlocked.Decrement(ref running);

                    if (Interlocked.Increment(ref count) == total)
                    {
                        allDone.Set();
                    }
                })
            )
            .ToArray();

        await Task.WhenAll(tasks);
        await allDone.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(1, maxConcurrent);
    }

    [Fact]
    public async Task Post_WhenCalled_ShouldQueueCallbackForExecution()
    {
        // Arrange
        var context = new DispatcherSynchronizationContext();
        var tcs = new TaskCompletionSource();
        object? capturedState = null;
        var stateObj = new object();

        // Act
        context.Post(
            state =>
            {
                capturedState = state;
                tcs.SetResult();
            },
            stateObj
        );
        await tcs.Task.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.Same(stateObj, capturedState);
    }

    [Fact(Timeout = 5000)]
    public async Task Post_WhenCallbackThrows_ShouldRaiseUnhandledException()
    {
        // Arrange
        var context = new DispatcherSynchronizationContext();
        var expected = new InvalidOperationException("test");
        var exceptionRaised = new AsyncManualResetEvent();
        UnhandledExceptionEventArgs? capturedArgs = null;

        context.UnhandledException += (_, e) =>
        {
            capturedArgs = e;
            exceptionRaised.Set();
        };

        // Act
        context.Post(_ => throw expected, null);
        await exceptionRaised.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(capturedArgs);
        Assert.Same(expected, capturedArgs.ExceptionObject);
    }
}
