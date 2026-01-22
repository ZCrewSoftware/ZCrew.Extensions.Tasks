using System.Diagnostics.CodeAnalysis;
using Nito.AsyncEx;
using ZCrew.Extensions.Tasks.UnitTests.TestHelpers;

namespace ZCrew.Extensions.Tasks.UnitTests;

public sealed class AsyncEventHandlerTests
{
    [Fact]
    public async Task InvokeAsync_WhenThereAreManyPassingHandlers_ShouldCallAllHandlersSequentially()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId2 = new Guid("DB1B226A-94F3-4DEF-90ED-8223B870E3C6");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => invocationList.Register(eventId2));
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        await eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);
        eventWrapper.TestEvent -= eventHandler1;
        eventWrapper.TestEvent -= eventHandler2;
        eventWrapper.TestEvent -= eventHandler3;

        // Assert
        var invocations = invocationList.ToList();
        Assert.Collection(
            invocations,
            invocation => Assert.Equal(eventId1, invocation),
            invocation => Assert.Equal(eventId2, invocation),
            invocation => Assert.Equal(eventId3, invocation)
        );
    }

    [Fact]
    public async Task InvokeAsync_WhenOnlyHandlerThrowsSynchronousException_ShouldThrowSameException()
    {
        // Arrange
        var eventHandler = TestEventHandler(() => throw new ArgumentException());

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler;

        // Act
        var invokeAsync = async () =>
        {
            try
            {
                await eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler;
            }
        };

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_WhenManyHandlersThrowsSynchronousExceptions_ShouldThrowOnlyFirstException()
    {
        // Arrange
        var eventHandler1 = TestEventHandler(() => throw new ArgumentException());
        var eventHandler2 = TestEventHandler(() => throw new IOException());

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;

        // Act
        var invokeAsync = async () =>
        {
            try
            {
                await eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler1;
                eventWrapper.TestEvent -= eventHandler2;
            }
        };

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(invokeAsync);
    }

    [Fact]
    public async Task InvokeAsync_WhenFirstHandlerThrowsSynchronousException_ShouldNotCallHandlersAfterException()
    {
        // Arrange
        var eventId2 = new Guid("DB1B226A-94F3-4DEF-90ED-8223B870E3C6");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => throw new ArgumentException());
        var eventHandler2 = TestEventHandler(() => invocationList.Register(eventId2));
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);
        }
        catch (ArgumentException) { }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        Assert.Empty(invocationList);
    }

    [Fact]
    public async Task InvokeAsync_WhenMiddleHandlerThrowsSynchronousException_ShouldNotCallHandlersAfterException()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => throw new ArgumentException());
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);
        }
        catch (ArgumentException) { }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        var invocations = invocationList.ToList();
        Assert.Collection(invocations, invocation => Assert.Equal(eventId1, invocation));
    }

    [Fact]
    public async Task InvokeAsync_WhenLastHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId2 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => invocationList.Register(eventId2));
        var eventHandler3 = TestEventHandler(() => throw new ArgumentException());

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);
        }
        catch (ArgumentException) { }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        var invocations = invocationList.ToList();
        Assert.Collection(
            invocations,
            invocation => Assert.Equal(eventId1, invocation),
            invocation => Assert.Equal(eventId2, invocation)
        );
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeAsync_WhenCancellationTokenIsCanceled_ShouldIgnoreCancellation()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        using var cts = new CancellationTokenSource();

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => cts.Cancel());
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeAsync(EventArgs.Empty, cts.Token);
        }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        var invocations = invocationList.ToList();
        Assert.Collection(
            invocations,
            invocation => Assert.Equal(eventId1, invocation),
            invocation => Assert.Equal(eventId3, invocation)
        );
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeAsync_WhenTokenIsCanceledWhenHandlerIsAwaiting_ShouldCancelAndThrowOperationCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var handlerEnteredEvent = new AsyncManualResetEvent();

        var eventHandler = TestEventHandler(async () =>
        {
            var delay = Task.Delay(Timeout.Infinite, cts.Token);
            handlerEnteredEvent.Set();
            await delay;
        });

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler;

        // Act
        var invokeSequentialAsync = async () =>
        {
            try
            {
                using var waitCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var task = eventWrapper.InvokeAsync(EventArgs.Empty, cts.Token);
                await handlerEnteredEvent.WaitAsync(waitCts.Token);
                await cts.CancelAsync();
                await task;
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler;
            }
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeSequentialAsync);
        Assert.True(handlerEnteredEvent.IsSet, "because the handler should have been entered");
    }

    [Fact]
    public async Task InvokeAsync_WhenThereAreNoRegisteredHandlers_ShouldInvokeWithoutExceptions()
    {
        // Arrange
        var eventWrapper = new AsyncEventHandlerWrapper();

        // Act
        await eventWrapper.InvokeAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        Assert.True(true, "Should reach end of test without an exception");
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenThereAreManyPassingHandlers_ShouldCallAllHandlersInParallel()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId2 = new Guid("DB1B226A-94F3-4DEF-90ED-8223B870E3C6");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var eventLock1 = new AsyncManualResetEvent();
        var eventLock2 = new AsyncManualResetEvent();

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(async () =>
        {
            await eventLock1.WaitAsync();
            invocationList.Register(eventId1);
        });
        var eventHandler2 = TestEventHandler(async () =>
        {
            await eventLock2.WaitAsync();
            invocationList.Register(eventId2);
            eventLock1.Set();
        });
        var eventHandler3 = TestEventHandler(() =>
        {
            invocationList.Register(eventId3);
            eventLock2.Set();
        });

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        await eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);
        eventWrapper.TestEvent -= eventHandler1;
        eventWrapper.TestEvent -= eventHandler2;
        eventWrapper.TestEvent -= eventHandler3;

        // Assert
        var invocations = invocationList.ToList();
        Assert.Collection(
            invocations,
            invocation => Assert.Equal(eventId3, invocation),
            invocation => Assert.Equal(eventId2, invocation),
            invocation => Assert.Equal(eventId1, invocation)
        );
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenOnlyHandlerThrowsSynchronousException_ShouldThrowAggregateException()
    {
        // Arrange
        var eventHandler = TestEventHandler(() => throw new ArgumentException());

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler;

        // Act
        var invokeAsync = async () =>
        {
            try
            {
                await eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler;
            }
        };

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeAsync);
        Assert.Single(aggregateException.InnerExceptions);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is ArgumentException);
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenManyHandlersThrowsSynchronousExceptions_ShouldThrowAggregateException()
    {
        // Arrange
        var eventHandler1 = TestEventHandler(() => throw new ArgumentException());
        var eventHandler2 = TestEventHandler(() => throw new IOException());

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;

        // Act
        var invokeAsync = async () =>
        {
            try
            {
                await eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler1;
                eventWrapper.TestEvent -= eventHandler2;
            }
        };

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeAsync);
        Assert.Equal(2, aggregateException.InnerExceptions.Count);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is ArgumentException);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is IOException);
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenFirstHandlerThrowsSynchronousException_ShouldCallHandlersAfterException()
    {
        // Arrange
        var eventId2 = new Guid("DB1B226A-94F3-4DEF-90ED-8223B870E3C6");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => throw new ArgumentException());
        var eventHandler2 = TestEventHandler(() => invocationList.Register(eventId2));
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);
        }
        catch (AggregateException) { }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        var invocations = invocationList.ToList();
        Assert.Contains(invocations, invocation => eventId2 == invocation);
        Assert.Contains(invocations, invocation => eventId3 == invocation);
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenMiddleHandlerThrowsSynchronousException_ShouldCallHandlersAfterException()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => throw new ArgumentException());
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);
        }
        catch (AggregateException) { }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        var invocations = invocationList.ToList();
        Assert.Contains(invocations, invocation => eventId1 == invocation);
        Assert.Contains(invocations, invocation => eventId3 == invocation);
    }

    [Fact]
    public async Task InvokeParallelAsync_WhenLastHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId2 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => invocationList.Register(eventId2));
        var eventHandler3 = TestEventHandler(() => throw new ArgumentException());

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);
        }
        catch (AggregateException) { }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        var invocations = invocationList.ToList();
        Assert.Contains(invocations, invocation => eventId1 == invocation);
        Assert.Contains(invocations, invocation => eventId2 == invocation);
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeParallelAsync_WhenCancellationTokenIsCanceledBeforeCall_ShouldNotCallHandlers()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId2 = new Guid("DB1B226A-94F3-4DEF-90ED-8223B870E3C6");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        using var cts = new CancellationTokenSource();

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => invocationList.Register(eventId2));
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        var invokeParallelAsync = async () =>
        {
            try
            {
                await cts.CancelAsync();
                await eventWrapper.InvokeParallelAsync(EventArgs.Empty, cts.Token);
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler1;
                eventWrapper.TestEvent += eventHandler2;
                eventWrapper.TestEvent += eventHandler3;
            }
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeParallelAsync);

        var invocations = invocationList.ToList();
        Assert.Empty(invocations);
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeParallelAsync_WhenTokenIsCanceledWhenHandlerIsAwaiting_ShouldCancelAndThrowOperationCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var handlerEnteredEvent = new AsyncManualResetEvent();

        var eventHandler = TestEventHandler(async () =>
        {
            var delay = Task.Delay(Timeout.Infinite, cts.Token);
            handlerEnteredEvent.Set();
            await delay;
        });

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler;

        // Act
        var invokeParallelAsync = async () =>
        {
            try
            {
                using var waitCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var task = eventWrapper.InvokeAsync(EventArgs.Empty, cts.Token);
                await handlerEnteredEvent.WaitAsync(waitCts.Token);
                await cts.CancelAsync();
                await task;
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler;
            }
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
        await eventWrapper.InvokeParallelAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        Assert.True(true, "Should reach end of test without an exception");
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenThereAreManyPassingHandlers_ShouldCallAllHandlersSequentially()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId2 = new Guid("DB1B226A-94F3-4DEF-90ED-8223B870E3C6");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => invocationList.Register(eventId2));
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);
        eventWrapper.TestEvent -= eventHandler1;
        eventWrapper.TestEvent -= eventHandler2;
        eventWrapper.TestEvent -= eventHandler3;

        // Assert
        var invocations = invocationList.ToList();
        Assert.Collection(
            invocations,
            invocation => Assert.Equal(eventId1, invocation),
            invocation => Assert.Equal(eventId2, invocation),
            invocation => Assert.Equal(eventId3, invocation)
        );
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenOnlyHandlerThrowsSynchronousException_ShouldThrowSameException()
    {
        // Arrange
        var eventHandler = TestEventHandler(() => throw new ArgumentException());

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler;

        // Act
        var invokeAsync = async () =>
        {
            try
            {
                await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler;
            }
        };

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeAsync);
        Assert.Single(aggregateException.InnerExceptions);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is ArgumentException);
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenManyHandlersThrowsSynchronousExceptions_ShouldThrowAggregateException()
    {
        // Arrange
        var eventHandler1 = TestEventHandler(() => throw new ArgumentException());
        var eventHandler2 = TestEventHandler(() => throw new IOException());

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;

        // Act
        var invokeAsync = async () =>
        {
            try
            {
                await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler1;
                eventWrapper.TestEvent -= eventHandler2;
            }
        };

        // Assert
        var aggregateException = await Assert.ThrowsAsync<AggregateException>(invokeAsync);
        Assert.Equal(2, aggregateException.InnerExceptions.Count);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is ArgumentException);
        Assert.Contains(aggregateException.InnerExceptions, exception => exception is IOException);
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenFirstHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var eventId2 = new Guid("DB1B226A-94F3-4DEF-90ED-8223B870E3C6");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => throw new ArgumentException());
        var eventHandler2 = TestEventHandler(() => invocationList.Register(eventId2));
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);
        }
        catch (AggregateException) { }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        var invocations = invocationList.ToList();
        Assert.Collection(
            invocations,
            invocation => Assert.Equal(eventId2, invocation),
            invocation => Assert.Equal(eventId3, invocation)
        );
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenMiddleHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => throw new ArgumentException());
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);
        }
        catch (AggregateException) { }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        var invocations = invocationList.ToList();
        Assert.Collection(
            invocations,
            invocation => Assert.Equal(eventId1, invocation),
            invocation => Assert.Equal(eventId3, invocation)
        );
    }

    [Fact]
    public async Task InvokeSequentialAsync_WhenLastHandlerThrowsSynchronousException_ShouldCallAllHandlersAndThrowException()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId2 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => invocationList.Register(eventId2));
        var eventHandler3 = TestEventHandler(() => throw new ArgumentException());

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        try
        {
            await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);
        }
        catch (AggregateException) { }
        finally
        {
            eventWrapper.TestEvent -= eventHandler1;
            eventWrapper.TestEvent += eventHandler2;
            eventWrapper.TestEvent += eventHandler3;
        }

        // Assert
        var invocations = invocationList.ToList();
        Assert.Collection(
            invocations,
            invocation => Assert.Equal(eventId1, invocation),
            invocation => Assert.Equal(eventId2, invocation)
        );
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeSequentialAsync_WhenCancellationTokenIsCanceled_ShouldCancelBeforeInvokingPendingHandlers()
    {
        // Arrange
        var eventId1 = new Guid("98C9A7EF-6554-4CB2-ACA5-7022E72400B4");
        var eventId3 = new Guid("3A9586E4-E4B8-4F26-9016-07DD6B4B6417");

        using var cts = new CancellationTokenSource();

        var invocationList = new InvocationList<Guid>();
        var eventHandler1 = TestEventHandler(() => invocationList.Register(eventId1));
        var eventHandler2 = TestEventHandler(() => cts.Cancel());
        var eventHandler3 = TestEventHandler(() => invocationList.Register(eventId3));

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler1;
        eventWrapper.TestEvent += eventHandler2;
        eventWrapper.TestEvent += eventHandler3;

        // Act
        var invokeSequentialAsync = async () =>
        {
            try
            {
                await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, cts.Token);
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler1;
                eventWrapper.TestEvent += eventHandler2;
                eventWrapper.TestEvent += eventHandler3;
            }
        };

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invokeSequentialAsync);

        var invocations = invocationList.ToList();
        Assert.Collection(invocations, invocation => Assert.Equal(eventId1, invocation));
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task InvokeSequentialAsync_WhenTokenIsCanceledWhenHandlerIsAwaiting_ShouldCancelAndThrowOperationCanceledException()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var handlerEnteredEvent = new AsyncManualResetEvent();

        var eventHandler = TestEventHandler(async () =>
        {
            var delay = Task.Delay(Timeout.Infinite, cts.Token);
            handlerEnteredEvent.Set();
            await delay;
        });

        var eventWrapper = new AsyncEventHandlerWrapper();
        eventWrapper.TestEvent += eventHandler;

        // Act
        var invokeSequentialAsync = async () =>
        {
            try
            {
                using var waitCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var task = eventWrapper.InvokeAsync(EventArgs.Empty, cts.Token);
                await handlerEnteredEvent.WaitAsync(waitCts.Token);
                await cts.CancelAsync();
                await task;
            }
            finally
            {
                eventWrapper.TestEvent -= eventHandler;
            }
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
        await eventWrapper.InvokeSequentialAsync(EventArgs.Empty, CancellationToken.None);

        // Assert
        Assert.True(true, "Should reach end of test without an exception");
    }

    private static AsyncEventHandler TestEventHandler(Action action)
    {
        return Handle;

        Task Handle(object? sender, EventArgs eventArgs, CancellationToken token)
        {
            action();
            return Task.CompletedTask;
        }
    }

    private static AsyncEventHandler TestEventHandler(Func<Task> action)
    {
        return Handle;

        Task Handle(object? sender, EventArgs eventArgs, CancellationToken token)
        {
            return action();
        }
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
