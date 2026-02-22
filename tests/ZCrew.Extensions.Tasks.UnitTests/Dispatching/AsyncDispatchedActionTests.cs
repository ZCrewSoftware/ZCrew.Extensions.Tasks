using ZCrew.Extensions.Tasks.Dispatching;

namespace ZCrew.Extensions.Tasks.UnitTests.Dispatching;

public class AsyncDispatchedActionTests
{
    [Fact]
    public async Task InvokeAsync_WhenActionCompletes_ShouldCompleteWaitAsync()
    {
        // Arrange
        var action = ((Action)(() => { })).AsAsyncAction();
        var dispatched = new AsyncDispatchedAction(action);
        var token = TestContext.Current.CancellationToken;

        // Act
        await dispatched.InvokeAsync(token);

        // Assert
        await dispatched.WaitAsync(token);
    }

    [Fact]
    public async Task InvokeAsync_WhenActionCompletes_ShouldNotBeCanceled()
    {
        // Arrange
        var action = ((Action)(() => { })).AsAsyncAction();
        var dispatched = new AsyncDispatchedAction(action);
        var token = TestContext.Current.CancellationToken;

        // Act
        await dispatched.InvokeAsync(token);
        await dispatched.WaitAsync(token);

        // Assert
        Assert.False(dispatched.IsCanceled);
    }

    [Fact]
    public async Task SetException_WhenCalled_ShouldFaultWaitAsync()
    {
        // Arrange
        var action = ((Action)(() => { })).AsAsyncAction();
        var dispatched = new AsyncDispatchedAction(action);
        var expected = new InvalidOperationException("test");
        dispatched.SetException(expected);

        // Act
        var waitAsync = () => dispatched.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(waitAsync);
        Assert.Same(expected, ex);
    }

    [Fact]
    public async Task SetCanceled_WhenCalled_ShouldCancelWaitAsync()
    {
        // Arrange
        var action = ((Action)(() => { })).AsAsyncAction();
        var dispatched = new AsyncDispatchedAction(action);
        dispatched.SetCanceled();

        // Act
        var waitAsync = () => dispatched.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(waitAsync);
    }

    [Fact]
    public void SetCanceled_WhenCalled_ShouldSetIsCanceled()
    {
        // Arrange
        var action = ((Action)(() => { })).AsAsyncAction();
        var dispatched = new AsyncDispatchedAction(action);

        // Act
        dispatched.SetCanceled();

        // Assert
        Assert.True(dispatched.IsCanceled);
    }

    [Fact]
    public async Task InvokeAsync_WhenActionThrows_ShouldPropagateException()
    {
        // Arrange
        var expected = new InvalidOperationException("test");
        var action = ((Action)(() => throw expected)).AsAsyncAction();
        var dispatched = new AsyncDispatchedAction(action);
        var token = TestContext.Current.CancellationToken;

        // Act
        var invokeAsync = () => dispatched.InvokeAsync(token);

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(invokeAsync);
        Assert.Same(expected, ex);
    }

    [Fact]
    public async Task SetCanceled_WhenAlreadyCompleted_ShouldBeNoOp()
    {
        // Arrange
        var action = ((Action)(() => { })).AsAsyncAction();
        var dispatched = new AsyncDispatchedAction(action);
        var token = TestContext.Current.CancellationToken;
        await dispatched.InvokeAsync(token);

        // Act
        dispatched.SetCanceled();

        // Assert
        await dispatched.WaitAsync(token);
        Assert.False(dispatched.IsCanceled);
    }

    [Fact]
    public async Task SetException_WhenAlreadyCompleted_ShouldBeNoOp()
    {
        // Arrange
        var action = ((Action)(() => { })).AsAsyncAction();
        var dispatched = new AsyncDispatchedAction(action);
        var token = TestContext.Current.CancellationToken;
        await dispatched.InvokeAsync(token);

        // Act
        dispatched.SetException(new InvalidOperationException("test"));

        // Assert
        await dispatched.WaitAsync(token);
    }
}
