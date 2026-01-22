using NSubstitute;

namespace ZCrew.Extensions.Tasks.UnitTests;

public class SyncFuncTests
{
    [Fact]
    public async Task SyncFunc_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object>>();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke().Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke();
    }

    [Fact]
    public async Task SyncFunc_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object>>();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task SyncFunc_T_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object>>();
        var arg = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg, token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(arg);
    }

    [Fact]
    public async Task SyncFunc_T_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object>>();
        var arg = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive().Invoke(Arg.Any<object>());
    }

    [Fact]
    public async Task SyncFunc_T1_T2_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(arg1, arg2);
    }

    [Fact]
    public async Task SyncFunc_T1_T2_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>());
    }

    [Fact]
    public async Task SyncFunc_T1_T2_T3_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2, arg3).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(arg1, arg2, arg3);
    }

    [Fact]
    public async Task SyncFunc_T1_T2_T3_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>());
    }

    [Fact]
    public async Task SyncFunc_T1_T2_T3_T4_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2, arg3, arg4).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(arg1, arg2, arg3, arg4);
    }

    [Fact]
    public async Task SyncFunc_T1_T2_T3_T4_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>());
    }
}
