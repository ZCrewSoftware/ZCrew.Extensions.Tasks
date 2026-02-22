using Nito.AsyncEx;
using NSubstitute;
using ZCrew.Extensions.Tasks.Dispatching;

namespace ZCrew.Extensions.Tasks.IntegrationTests.Dispatching;

public class SynchronizationContextDispatcherTests
{
    [Fact]
    public async Task InvokeAsync_Action_WhenNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync((Action)null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_FuncTask_WhenNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync(null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_FuncResult_WhenNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync((Func<int>)null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_FuncTaskResult_WhenNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync((Func<Task<int>>)null!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_Action_WhenCalled_ShouldExecuteAction()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
        var workItem = Substitute.For<Action>();

        // Act
        await dispatcher.InvokeAsync(workItem);

        // Assert
        workItem.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeAsync_FuncTask_WhenCalled_ShouldExecuteAndComplete()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
        var workItem = Substitute.For<Func<Task>>();
        workItem.Invoke().Returns(Task.CompletedTask);

        // Act
        await dispatcher.InvokeAsync(workItem);

        // Assert
        await workItem.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeAsync_FuncResult_WhenCalled_ShouldReturnResult()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
        var workItem = Substitute.For<Func<int>>();
        workItem.Invoke().Returns(42);

        // Act
        var result = await dispatcher.InvokeAsync(workItem);

        // Assert
        Assert.Equal(42, result);
        workItem.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeAsync_FuncTaskResult_WhenCalled_ShouldReturnResult()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
        var workItem = Substitute.For<Func<Task<int>>>();
        workItem.Invoke().Returns(Task.FromResult(42));

        // Act
        var result = await dispatcher.InvokeAsync(workItem);

        // Assert
        Assert.Equal(42, result);
        await workItem.Received(1).Invoke();
    }

    [Fact(Timeout = 5000)]
    public async Task InvokeAsync_WhenCalledConcurrently_ShouldSerializeExecution()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
        var running = 0;
        var maxConcurrent = 0;
        var allDone = new AsyncManualResetEvent();
        var count = 0;
        const int total = 5;

        // Act
        var tasks = Enumerable
            .Range(0, total)
            .Select(_ =>
                dispatcher.InvokeAsync(async () =>
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
    public void CheckAccess_WhenCalledFromOutside_ShouldReturnFalse()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();

        // Act
        var result = dispatcher.CheckAccess();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CheckAccess_WhenCalledFromInside_ShouldReturnTrue()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
        var result = false;

        // Act
        await dispatcher.InvokeAsync(() => result = dispatcher.CheckAccess());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task InvokeAsync_Action_WhenCalledFromInside_ShouldExecuteInline()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
        var innerAction = Substitute.For<Action>();

        // Act
        await dispatcher.InvokeAsync(() =>
        {
            _ = dispatcher.InvokeAsync(innerAction);
        });

        // Assert
        innerAction.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeAsync_FuncResult_WhenCalledFromInside_ShouldReturnResultInline()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
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
    }

    [Fact]
    public async Task InvokeAsync_FuncTask_WhenCalledFromInside_ShouldExecuteInline()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
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
    }

    [Fact]
    public async Task InvokeAsync_FuncTaskResult_WhenCalledFromInside_ShouldReturnResultInline()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
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
    }

    [Fact]
    public async Task InvokeAsync_Action_WhenWorkItemThrows_ShouldFaultReturnedTask()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
        var expected = new InvalidOperationException("test");

        // Act
        var invokeAsync = () => dispatcher.InvokeAsync((Action)(() => throw expected));

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(invokeAsync);
        Assert.Same(expected, ex);
    }

    [Fact]
    public async Task InvokeAsync_WhenWorkItemThrows_ShouldContinueProcessing()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();
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
    }

    [Fact]
    public void AssertAccess_WhenCalledFromOutside_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();

        // Act
        var assertAccess = () => dispatcher.AssertAccess();

        // Assert
        Assert.Throws<InvalidOperationException>(assertAccess);
    }

    [Fact]
    public async Task AssertAccess_WhenCalledFromInside_ShouldNotThrow()
    {
        // Arrange
        var dispatcher = new SynchronizationContextDispatcher();

        // Act
        await dispatcher.InvokeAsync(() => dispatcher.AssertAccess());

        // Assert
        Assert.True(true);
    }
}
