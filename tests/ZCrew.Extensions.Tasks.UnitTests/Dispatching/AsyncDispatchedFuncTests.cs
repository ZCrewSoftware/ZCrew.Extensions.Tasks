using ZCrew.Extensions.Tasks.Dispatching;

namespace ZCrew.Extensions.Tasks.UnitTests.Dispatching;

public class AsyncDispatchedFuncTests
{
    [Fact]
    public async Task InvokeAsync_WhenFuncCompletes_ShouldReturnResultFromWaitAsync()
    {
        // Arrange
        var func = ((Func<int>)(() => 42)).AsAsyncFunc();
        var dispatched = new AsyncDispatchedFunc<int>(func);
        var token = TestContext.Current.CancellationToken;

        // Act
        await dispatched.InvokeAsync(token);
        var result = await dispatched.WaitAsync(token);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task InvokeAsync_WhenFuncCompletes_ShouldNotBeCanceled()
    {
        // Arrange
        var func = ((Func<int>)(() => 42)).AsAsyncFunc();
        var dispatched = new AsyncDispatchedFunc<int>(func);
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
        var func = ((Func<int>)(() => 42)).AsAsyncFunc();
        var dispatched = new AsyncDispatchedFunc<int>(func);
        var expected = new InvalidOperationException("test");
        dispatched.SetException(expected);

        // Act
        var waitAsync = () => dispatched.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(waitAsync);
        Assert.Same(expected, ex);
    }

    [Fact]
    public async Task SetCanceled_WhenCalled_ShouldCancelWaitAsyncAndSetIsCanceled()
    {
        // Arrange
        var func = ((Func<int>)(() => 42)).AsAsyncFunc();
        var dispatched = new AsyncDispatchedFunc<int>(func);
        dispatched.SetCanceled();

        // Act
        var waitAsync = () => dispatched.WaitAsync(TestContext.Current.CancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(waitAsync);
        Assert.True(dispatched.IsCanceled);
    }

    [Fact]
    public async Task InvokeAsync_WhenFuncThrows_ShouldPropagateException()
    {
        // Arrange
        var expected = new InvalidOperationException("test");
        var func = ((Func<int>)(() => throw expected)).AsAsyncFunc();
        var dispatched = new AsyncDispatchedFunc<int>(func);
        var token = TestContext.Current.CancellationToken;

        // Act
        var invokeAsync = () => dispatched.InvokeAsync(token);

        // Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(invokeAsync);
        Assert.Same(expected, ex);
    }

    [Fact]
    public async Task WaitAsync_WhenTokenIsPreCanceled_ShouldThrow()
    {
        // Arrange
        var func = ((Func<int>)(() => 42)).AsAsyncFunc();
        var dispatched = new AsyncDispatchedFunc<int>(func);
        var token = new CancellationToken(canceled: true);

        // Act
        var waitAsync = () => dispatched.WaitAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(waitAsync);
    }
}
