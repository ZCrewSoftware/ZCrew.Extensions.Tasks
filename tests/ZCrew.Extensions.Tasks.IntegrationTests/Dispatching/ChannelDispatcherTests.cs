using Nito.AsyncEx;
using NSubstitute;
using ZCrew.Extensions.Tasks.Dispatching;

namespace ZCrew.Extensions.Tasks.IntegrationTests.Dispatching;

public class ChannelDispatcherTests
{
    [Fact]
    public async Task InvokeAsync_Action_WhenNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync((Action)null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_FuncTask_WhenNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync(null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_FuncResult_WhenNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync((Func<int>)null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_FuncTaskResult_WhenNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync((Func<Task<int>>)null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(invokeAsync);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_Action_WhenCalled_ShouldExecuteAction()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var workItem = Substitute.For<Action>();

        // Act
        await dispatcher.InvokeAsync(workItem);

        // Assert
        workItem.Received(1).Invoke();

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_FuncTask_WhenCalled_ShouldExecuteAndComplete()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var workItem = Substitute.For<Func<Task>>();
        workItem.Invoke().Returns(Task.CompletedTask);

        // Act
        await dispatcher.InvokeAsync(workItem);

        // Assert
        await workItem.Received(1).Invoke();

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_FuncResult_WhenCalled_ShouldReturnResult()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var workItem = Substitute.For<Func<int>>();
        workItem.Invoke().Returns(42);

        // Act
        var result = await dispatcher.InvokeAsync(workItem);

        // Assert
        Assert.Equal(42, result);
        workItem.Received(1).Invoke();

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_FuncTaskResult_WhenCalled_ShouldReturnResult()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var workItem = Substitute.For<Func<Task<int>>>();
        workItem.Invoke().Returns(Task.FromResult(42));

        // Act
        var result = await dispatcher.InvokeAsync(workItem);

        // Assert
        Assert.Equal(42, result);
        await workItem.Received(1).Invoke();

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_WhenMultipleItemsQueued_ShouldExecuteInFifoOrder()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();
        var gate = new AsyncManualResetEvent();

        // Block the dispatcher with a first item so the rest queue up
        var blockingTask = dispatcher.InvokeAsync(async () =>
        {
            await gate.WaitAsync(TestContext.Current.CancellationToken);
        });

        var task1 = dispatcher.InvokeAsync(action1);
        var task2 = dispatcher.InvokeAsync(action2);
        var task3 = dispatcher.InvokeAsync(action3);

        // Act
        gate.Set();
        await Task.WhenAll(blockingTask, task1, task2, task3);

        // Assert
        Received.InOrder(() =>
        {
            action1();
            action2();
            action3();
        });

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task CheckAccess_WhenCalledFromOutside_ShouldReturnFalse()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);

        // Act
        var result = dispatcher.CheckAccess();

        // Assert
        Assert.False(result);

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task CheckAccess_WhenCalledFromInside_ShouldReturnTrue()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var result = false;

        // Act
        await dispatcher.InvokeAsync(() => result = dispatcher.CheckAccess());

        // Assert
        Assert.True(result);

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_Action_WhenCalledFromInside_ShouldExecuteInline()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var innerAction = Substitute.For<Action>();

        // Act
        await dispatcher.InvokeAsync(() =>
        {
            _ = dispatcher.InvokeAsync(innerAction);
        });

        // Assert
        innerAction.Received(1).Invoke();

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_FuncResult_WhenCalledFromInside_ShouldReturnResultInline()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var result = 0;

        // Act
        await dispatcher.InvokeAsync(() =>
        {
            var task = dispatcher.InvokeAsync(() => 42);
            Assert.True(task.IsCompleted);
            result = task.Result;
        });

        // Assert
        Assert.Equal(42, result);

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_FuncTask_WhenCalledFromInside_ShouldExecuteInline()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var innerWorkItem = Substitute.For<Func<Task>>();
        innerWorkItem.Invoke().Returns(Task.CompletedTask);

        // Act
        await dispatcher.InvokeAsync(() =>
        {
            var task = dispatcher.InvokeAsync(innerWorkItem);
            Assert.True(task.IsCompleted);
        });

        // Assert
        await innerWorkItem.Received(1).Invoke();

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_FuncTaskResult_WhenCalledFromInside_ShouldReturnResultInline()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var innerWorkItem = Substitute.For<Func<Task<int>>>();
        innerWorkItem.Invoke().Returns(Task.FromResult(42));
        var result = 0;

        // Act
        await dispatcher.InvokeAsync(() =>
        {
            var task = dispatcher.InvokeAsync(innerWorkItem);
            Assert.True(task.IsCompleted);
            result = task.Result;
        });

        // Assert
        Assert.Equal(42, result);
        await innerWorkItem.Received(1).Invoke();

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_WhenWorkItemThrows_ShouldRaiseUnhandledExceptionEvent()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var expected = new InvalidOperationException("test");
        var exceptionRaised = new AsyncManualResetEvent();
        UnhandledExceptionEventArgs? capturedArgs = null;

        dispatcher.UnhandledException += (_, e) =>
        {
            capturedArgs = e;
            exceptionRaised.Set();
        };

        // Act
        try
        {
            await dispatcher.InvokeAsync((Action)(() => throw expected));
        }
        catch
        {
            // Expected
        }

        await exceptionRaised.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(capturedArgs);
        Assert.Same(expected, capturedArgs.ExceptionObject);

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_WhenWorkItemThrows_ShouldFaultReturnedTask()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var expected = new InvalidOperationException("test");

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync((Action)(() => throw expected));

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(invokeAsync);
        Assert.Same(expected, ex);

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_WhenWorkItemThrows_ShouldContinueProcessing()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        try
        {
            await dispatcher.InvokeAsync((Action)(() => throw new InvalidOperationException()));
        }
        catch
        {
            // Expected
        }

        var workItem = Substitute.For<Func<int>>();
        workItem.Invoke().Returns(42);

        // Act
        var result = await dispatcher.InvokeAsync(workItem);

        // Assert
        Assert.Equal(42, result);
        workItem.Received(1).Invoke();

        // Cleanup
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task StartAsync_WhenTokenIsPreCanceled_ShouldThrow()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        var token = new CancellationToken(canceled: true);

        // Act
        var startAsync = () => dispatcher.StartAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(startAsync);
    }

    [Fact(Timeout = 5000)]
    public async Task StopAsync_WhenCalled_ShouldStopProcessing()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var workItem = Substitute.For<Func<int>>();
        workItem.Invoke().Returns(42);
        var result = await dispatcher.InvokeAsync(workItem);
        Assert.Equal(42, result);

        // Act
        await dispatcher.StopAsync(TestContext.Current.CancellationToken);

        // Assert
        workItem.Received(1).Invoke();
    }

    [Fact(Timeout = 5000)]
    public async Task StopAsync_WhenOperationIsRunning_ShouldCancelOperationAndFaultCaller()
    {
        // Arrange
        var dispatcher = new UnboundedChannelDispatcher();
        await dispatcher.StartAsync(TestContext.Current.CancellationToken);
        var operationStarted = new TaskCompletionSource();
        var operationGate = new TaskCompletionSource();

        var invokeTask = dispatcher.InvokeAsync(async () =>
        {
            operationStarted.SetResult();
            await operationGate.Task;
        });

        await operationStarted.Task;

        // Act
        var stopTask = dispatcher.StopAsync(TestContext.Current.CancellationToken);
        operationGate.SetCanceled(CancellationToken.None);
        await stopTask;

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => invokeTask);
    }
}
