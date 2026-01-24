using System.Diagnostics.CodeAnalysis;
using Nito.AsyncEx;
using NSubstitute;
using ZCrew.Extensions.Tasks.UnitTests.Extensions;

namespace ZCrew.Extensions.Tasks.UnitTests;

public sealed class AsyncEventHandlerTests
{
    [Fact]
    public async Task InvokeAsync_WhenThereAreManyPassingHandlers_ShouldCallAllHandlersSequentially()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action2();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        await eventWrapper.InvokeAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        Received.InOrder(() =>
        {
            action1();
            action2();
            action3();
        });
    }

    [Fact]
    public async Task InvokeAsync_WhenOnlyHandlerThrowsException_ShouldThrowSameException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();

        // Act
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_WhenManyHandlersThrowsExceptions_ShouldThrowOnlyFirstException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) => throw new IOException();

        // Act
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_WhenFirstHandlerThrowsException_ShouldNotCallHandlersAfterException()
    {
        // Arrange
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action2();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
        action2.DidNotReceive().Invoke();
        action3.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task InvokeAsync_WhenMiddleHandlerThrowsException_ShouldNotCallHandlersAfterException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
        action1.Received(1).Invoke();
        action3.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task InvokeAsync_WhenLastHandlerThrowsException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action2();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();

        // Act
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
        Received.InOrder(() =>
        {
            action1();
            action2();
        });
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeAsync_WhenCancellationTokenIsCanceled_ShouldCancelBeforeInvokingPendingHandlers()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var action1 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            cts.Cancel();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, cts.Token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeAsync);
        action1.Received(1).Invoke();
        action3.DidNotReceive().Invoke();
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeAsync_WhenTokenIsCanceledWhenHandlerIsAwaiting_ShouldCancelAndThrowOperationCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var handlerEnteredEvent = new AsyncManualResetEvent();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += async (_, _, _) =>
        {
            var delay = Task.Delay(Timeout.Infinite, cts.Token);
            handlerEnteredEvent.Set();
            await delay;
        };

        // Act
        var invokeAsync = async () =>
        {
            var task = eventWrapper.InvokeAsync(EventArgs.Empty, cts.Token);
            await handlerEnteredEvent.WaitAsync(TestContext.Current.CancellationToken).Timeout();
            await cts.CancelAsync();
            await task;
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeAsync);
        Assert.True(handlerEnteredEvent.IsSet, "because the handler should have been entered");
    }

    [Fact]
    public async Task InvokeAsync_WhenThereAreNoRegisteredHandlers_ShouldInvokeWithoutExceptions()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();

        // Act
        await eventWrapper.InvokeAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(true, "Should reach end of test without an exception");
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenThereAreManyPassingHandlers_ShouldCallAllHandlersInParallel()
    {
        // Arrange
        var eventLock1 = new AsyncManualResetEvent();
        var eventLock2 = new AsyncManualResetEvent();

        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();
        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += async (_, _, token) =>
        {
            await eventLock1.WaitAsync(token);
            action1();
        };
        eventWrapper.TestEvent += async (_, _, token) =>
        {
            await eventLock2.WaitAsync(token);
            action2();
            eventLock1.Set();
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            eventLock2.Set();
            return Task.CompletedTask;
        };

        // Act
        await eventWrapper.InvokeParallelAsync(EventArgs.Empty, TestContext.Current.CancellationToken).Timeout();

        // Assert
        Received.InOrder(() =>
        {
            action3();
            action2();
            action1();
        });
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenOnlyHandlerThrowsException_ShouldThrowAggregateException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();

        // Act
        var invokeParallelAsync = () =>
            eventWrapper.InvokeParallelAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeParallelAsync);
        var exception = Assert.Single(aggregateException.InnerExceptions);
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenManyHandlersThrowsExceptions_ShouldThrowAggregateException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) => throw new IOException();

        // Act
        var invokeParallelAsync = () =>
            eventWrapper.InvokeParallelAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeParallelAsync);
        Assert.Equal(2, aggregateException.InnerExceptions.Count);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is ArgumentException);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is IOException);
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenFirstHandlerThrowsException_ShouldCallHandlersAfterException()
    {
        // Arrange
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action2();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        var invokeParallelAsync = () =>
            eventWrapper.InvokeParallelAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeParallelAsync);
        action2.Received(1).Invoke();
        action3.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenMiddleHandlerThrowsException_ShouldCallHandlersAfterException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        var invokeParallelAsync = () =>
            eventWrapper.InvokeParallelAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeParallelAsync);
        action1.Received(1).Invoke();
        action3.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenLastHandlerThrowsException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action2();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();

        // Act
        var invokeParallelAsync = () =>
            eventWrapper.InvokeParallelAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeParallelAsync);
        action1.Received(1).Invoke();
        action2.Received(1).Invoke();
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeParallelAsync_WhenCancellationTokenIsCanceledBeforeCall_ShouldNotCallHandlers()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action2();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        var invokeParallelAsync = async () =>
        {
            await cts.CancelAsync();
            await eventWrapper.InvokeParallelAsync(EventArgs.Empty, cts.Token);
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeParallelAsync);
        action1.DidNotReceive().Invoke();
        action2.DidNotReceive().Invoke();
        action3.DidNotReceive().Invoke();
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeParallelAsync_WhenTokenIsCanceledWhenHandlerIsAwaiting_ShouldCancelAndThrowOperationCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var handlerEnteredEvent = new AsyncManualResetEvent();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += async (_, _, _) =>
        {
            var delay = Task.Delay(Timeout.Infinite, cts.Token);
            handlerEnteredEvent.Set();
            await delay;
        };

        // Act
        var invokeParallelAsync = async () =>
        {
            var task = eventWrapper.InvokeParallelAsync(EventArgs.Empty, cts.Token);
            await handlerEnteredEvent.WaitAsync(TestContext.Current.CancellationToken).Timeout();
            await cts.CancelAsync();
            await task;
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeParallelAsync);
        Assert.True(handlerEnteredEvent.IsSet, "because the handler should have been entered");
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenThereAreNoRegisteredHandlers_ShouldInvokeWithoutExceptions()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();

        // Act
        await eventWrapper.InvokeParallelAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(true, "Should reach end of test without an exception");
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenThereAreManyPassingHandlers_ShouldCallAllHandlersSequentially()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action2();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        Received.InOrder(() =>
        {
            action1();
            action2();
            action3();
        });
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenOnlyHandlerThrowsException_ShouldThrowSameException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();

        // Act
        var invokeSequentialAsync = () =>
            eventWrapper.InvokeSequentialAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeSequentialAsync);
        var exception = Assert.Single(aggregateException.InnerExceptions);
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenManyHandlersThrowsExceptions_ShouldThrowAggregateException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) => throw new IOException();

        // Act
        var invokeSequentialAsync = () =>
            eventWrapper.InvokeSequentialAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeSequentialAsync);
        Assert.Collection(
            aggregateException.InnerExceptions,
            exception => Assert.IsType<ArgumentException>(exception),
            exception => Assert.IsType<IOException>(exception)
        );
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenFirstHandlerThrowsException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action2();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        var invokeSequentialAsync = () =>
            eventWrapper.InvokeSequentialAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeSequentialAsync);
        Received.InOrder(() =>
        {
            action2();
            action3();
        });
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenMiddleHandlerThrowsException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        var invokeSequentialAsync = () =>
            eventWrapper.InvokeSequentialAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeSequentialAsync);
        Received.InOrder(() =>
        {
            action1();
            action3();
        });
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenLastHandlerThrowsException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action2();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();

        // Act
        var invokeSequentialAsync = () =>
            eventWrapper.InvokeSequentialAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeSequentialAsync);
        Received.InOrder(() =>
        {
            action1();
            action2();
        });
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeSequentialAsync_WhenCancellationTokenIsCanceled_ShouldCancelBeforeInvokingPendingHandlers()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        var action1 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action1();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            cts.Cancel();
            return Task.CompletedTask;
        };
        eventWrapper.TestEvent += (_, _, _) =>
        {
            action3();
            return Task.CompletedTask;
        };

        // Act
        var invokeSequentialAsync = () => eventWrapper.InvokeSequentialAsync(EventArgs.Empty, cts.Token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeSequentialAsync);
        action1.Received(1).Invoke();
        action3.DidNotReceive().Invoke();
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeSequentialAsync_WhenTokenIsCanceledWhenHandlerIsAwaiting_ShouldCancelAndThrowOperationCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var handlerEnteredEvent = new AsyncManualResetEvent();

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += async (_, _, _) =>
        {
            var delay = Task.Delay(Timeout.Infinite, cts.Token);
            handlerEnteredEvent.Set();
            await delay;
        };

        // Act
        var invokeSequentialAsync = async () =>
        {
            var task = eventWrapper.InvokeSequentialAsync(EventArgs.Empty, cts.Token);
            await handlerEnteredEvent.WaitAsync(TestContext.Current.CancellationToken).Timeout();
            await cts.CancelAsync();
            await task;
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeSequentialAsync);
        Assert.True(handlerEnteredEvent.IsSet, "because the handler should have been entered");
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenThereAreNoRegisteredHandlers_ShouldInvokeWithoutExceptions()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();

        // Act
        await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(true, "Should reach end of test without an exception");
    }
}

file class AsyncEventHandlerWrapper
{
    public event AsyncEventHandler? TestEvent;

    public Task InvokeAsync(EventArgs e, CancellationToken token = default)
    {
        return TestEvent.InvokeAsync(this, e, token);
    }

    public Task InvokeParallelAsync(EventArgs e, CancellationToken token = default)
    {
        return TestEvent.InvokeParallelAsync(this, e, token);
    }

    public Task InvokeSequentialAsync(EventArgs e, CancellationToken token = default)
    {
        return TestEvent.InvokeSequentialAsync(this, e, token);
    }
}
