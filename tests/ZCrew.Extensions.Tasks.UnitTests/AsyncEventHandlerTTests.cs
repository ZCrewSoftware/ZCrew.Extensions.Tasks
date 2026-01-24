using System.Diagnostics.CodeAnalysis;
using Nito.AsyncEx;
using NSubstitute;

namespace ZCrew.Extensions.Tasks.UnitTests;

public class AsyncEventHandlerTTests
{
    [Fact]
    public async Task InvokeAsync_WhenThereAreManyPassingHandlers_ShouldCallAllHandlersSequentially()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        await eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        Received.InOrder(() =>
        {
            action1();
            action2();
            action3();
        });
    }

    [Fact]
    public async Task InvokeAsync_WhenOnlyHandlerThrowsSynchronousException_ShouldThrowSameException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();

        // Act
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_WhenManyHandlersThrowsSynchronousExceptions_ShouldThrowOnlyFirstException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) => throw new IOException();

        // Act
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_WhenFirstHandlerThrowsSynchronousException_ShouldNotCallHandlersAfterException()
    {
        // Arrange
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
        action2.DidNotReceive().Invoke();
        action3.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task InvokeAsync_WhenMiddleHandlerThrowsSynchronousException_ShouldNotCallHandlersAfterException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
        action1.Received(1).Invoke();
        action3.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task InvokeAsync_WhenLastHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        var invokeAsync = () => eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);

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

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
        eventWrapper.TestEvent += async (_, _, _) =>
        {
            var delay = Task.Delay(Timeout.Infinite, cts.Token);
            handlerEnteredEvent.Set();
            await delay;
        };

        // Act
        var invokeAsync = async () =>
        {
            using var waitCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var task = eventWrapper.InvokeAsync(EventArgs.Empty, cts.Token);
            await handlerEnteredEvent.WaitAsync(waitCts.Token);
            await cts.CancelAsync();
            await task;
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeAsync);
        Assert.True(handlerEnteredEvent.IsSet, "because the handler should have been entered");
    }

    [Fact]
    public async Task InvokeAsync_WhenCalled_ShouldPassSameEventArgs()
    {
        // Arrange
        var eventArgs = new object();
        var calledEventArgs = default(object?);

        var eventWrapper = new AsyncEventHandlerWrapper<object>();
        eventWrapper.TestEvent += (_, e, _) =>
        {
            calledEventArgs = e;
            return Task.CompletedTask;
        };

        // Act
        await eventWrapper.InvokeAsync(eventArgs, CancellationToken.None);

        // Assert
        Assert.Same(eventArgs, calledEventArgs);
    }

    [Fact]
    public async Task InvokeAsync_WhenThereAreNoRegisteredHandlers_ShouldInvokeWithoutExceptions()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();

        // Act
        await eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);

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
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        await eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        Received.InOrder(() =>
        {
            action3();
            action2();
            action1();
        });
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenOnlyHandlerThrowsSynchronousException_ShouldThrowAggregateException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();

        // Act
        var invokeParallelAsync = () => eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeParallelAsync);
        var exception = Assert.Single(aggregateException.InnerExceptions);
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenManyHandlersThrowsSynchronousExceptions_ShouldThrowAggregateException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) => throw new IOException();

        // Act
        var invokeParallelAsync = () => eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeParallelAsync);
        Assert.Equal(2, aggregateException.InnerExceptions.Count);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is ArgumentException);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is IOException);
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenFirstHandlerThrowsSynchronousException_ShouldCallHandlersAfterException()
    {
        // Arrange
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        var invokeParallelAsync = () => eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeParallelAsync);
        action2.Received(1).Invoke();
        action3.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenMiddleHandlerThrowsSynchronousException_ShouldCallHandlersAfterException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        var invokeParallelAsync = () => eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeParallelAsync);
        action1.Received(1).Invoke();
        action3.Received(1).Invoke();
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenLastHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        var invokeParallelAsync = () => eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);

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

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
        eventWrapper.TestEvent += async (_, _, _) =>
        {
            var delay = Task.Delay(Timeout.Infinite, cts.Token);
            handlerEnteredEvent.Set();
            await delay;
        };

        // Act
        var invokeParallelAsync = async () =>
        {
            using var waitCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var task = eventWrapper.InvokeParallelAsync(EventArgs.Empty, cts.Token);
            await handlerEnteredEvent.WaitAsync(waitCts.Token);
            await cts.CancelAsync();
            await task;
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeParallelAsync);
        Assert.True(handlerEnteredEvent.IsSet, "because the handler should have been entered");
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenCalled_ShouldPassSameEventArgs()
    {
        // Arrange
        var eventArgs = new object();
        var calledEventArgs = default(object?);

        var eventWrapper = new AsyncEventHandlerWrapper<object>();
        eventWrapper.TestEvent += (_, e, _) =>
        {
            calledEventArgs = e;
            return Task.CompletedTask;
        };

        // Act
        await eventWrapper.InvokeParallelAsync(eventArgs, CancellationToken.None);

        // Assert
        Assert.Same(eventArgs, calledEventArgs);
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenThereAreNoRegisteredHandlers_ShouldInvokeWithoutExceptions()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();

        // Act
        await eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);

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

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        Received.InOrder(() =>
        {
            action1();
            action2();
            action3();
        });
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenOnlyHandlerThrowsSynchronousException_ShouldThrowSameException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();

        // Act
        var invokeSequentialAsync = () => eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeSequentialAsync);
        var exception = Assert.Single(aggregateException.InnerExceptions);
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenManyHandlersThrowsSynchronousExceptions_ShouldThrowAggregateException()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
        eventWrapper.TestEvent += (_, _, _) => throw new ArgumentException();
        eventWrapper.TestEvent += (_, _, _) => throw new IOException();

        // Act
        var invokeSequentialAsync = () => eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeSequentialAsync);
        Assert.Collection(
            aggregateException.InnerExceptions,
            exception => Assert.IsType<ArgumentException>(exception),
            exception => Assert.IsType<IOException>(exception)
        );
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenFirstHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action2 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        var invokeSequentialAsync = () => eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeSequentialAsync);
        Received.InOrder(() =>
        {
            action2();
            action3();
        });
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenMiddleHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action3 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        var invokeSequentialAsync = () => eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<AggregateException>(invokeSequentialAsync);
        Received.InOrder(() =>
        {
            action1();
            action3();
        });
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenLastHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var action1 = Substitute.For<Action>();
        var action2 = Substitute.For<Action>();

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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
        var invokeSequentialAsync = () => eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);

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

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
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

        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();
        eventWrapper.TestEvent += async (_, _, _) =>
        {
            var delay = Task.Delay(Timeout.Infinite, cts.Token);
            handlerEnteredEvent.Set();
            await delay;
        };

        // Act
        var invokeSequentialAsync = async () =>
        {
            using var waitCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var task = eventWrapper.InvokeSequentialAsync(EventArgs.Empty, cts.Token);
            await handlerEnteredEvent.WaitAsync(waitCts.Token);
            await cts.CancelAsync();
            await task;
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeSequentialAsync);
        Assert.True(handlerEnteredEvent.IsSet, "because the handler should have been entered");
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenCalled_ShouldPassSameEventArgs()
    {
        // Arrange
        var eventArgs = new object();
        var calledEventArgs = default(object?);

        var eventWrapper = new AsyncEventHandlerWrapper<object>();
        eventWrapper.TestEvent += (_, e, _) =>
        {
            calledEventArgs = e;
            return Task.CompletedTask;
        };

        // Act
        await eventWrapper.InvokeSequentialAsync(eventArgs, CancellationToken.None);

        // Assert
        Assert.Same(eventArgs, calledEventArgs);
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenThereAreNoRegisteredHandlers_ShouldInvokeWithoutExceptions()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper<EventArgs>();

        // Act
        await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        Assert.True(true, "Should reach end of test without an exception");
    }
}

file class AsyncEventHandlerWrapper<TEventArgs>
{
    public event AsyncEventHandler<TEventArgs>? TestEvent;

    public Task InvokeAsync(TEventArgs e, CancellationToken token = default)
    {
        return TestEvent.InvokeAsync(this, e, token);
    }

    public Task InvokeParallelAsync(TEventArgs e, CancellationToken token = default)
    {
        return TestEvent.InvokeParallelAsync(this, e, token);
    }

    public Task InvokeSequentialAsync(TEventArgs e, CancellationToken token = default)
    {
        return TestEvent.InvokeSequentialAsync(this, e, token);
    }
}
