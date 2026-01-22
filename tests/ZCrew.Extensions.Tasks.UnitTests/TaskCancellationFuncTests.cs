using NSubstitute;

namespace ZCrew.Extensions.Tasks.UnitTests;

public class TaskCancellationFuncTests
{
    [Fact]
    public async Task TaskCancellationFunc_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<CancellationToken, Task<object>>>();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(token);

        // Assert
        Assert.Equal(result, actualResult);
        await func.Received(1).Invoke(token);
    }

    [Fact]
    public async Task TaskCancellationFunc_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<CancellationToken, Task<object>>>();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await func.DidNotReceive().Invoke(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task TaskCancellationFunc_T_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, CancellationToken, Task<object>>>();
        var arg = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg, token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg, token);

        // Assert
        Assert.Equal(result, actualResult);
        await func.Received(1).Invoke(arg, token);
    }

    [Fact]
    public async Task TaskCancellationFunc_T_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, CancellationToken, Task<object>>>();
        var arg = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await func.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task TaskCancellationFunc_T1_T2_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, CancellationToken, Task<object>>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2, token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, token);

        // Assert
        Assert.Equal(result, actualResult);
        await func.Received(1).Invoke(arg1, arg2, token);
    }

    [Fact]
    public async Task TaskCancellationFunc_T1_T2_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, CancellationToken, Task<object>>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await func.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task TaskCancellationFunc_T1_T2_T3_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, CancellationToken, Task<object>>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2, arg3, token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        Assert.Equal(result, actualResult);
        await func.Received(1).Invoke(arg1, arg2, arg3, token);
    }

    [Fact]
    public async Task TaskCancellationFunc_T1_T2_T3_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, CancellationToken, Task<object>>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await func.DidNotReceive()
            .Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task TaskCancellationFunc_T1_T2_T3_T4_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, CancellationToken, Task<object>>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2, arg3, arg4, token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, token);

        // Assert
        Assert.Equal(result, actualResult);
        await func.Received(1).Invoke(arg1, arg2, arg3, arg4, token);
    }

    [Fact]
    public async Task TaskCancellationFunc_T1_T2_T3_T4_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, CancellationToken, Task<object>>>();
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
        await func.DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<CancellationToken>()
            );
    }
}
