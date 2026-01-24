using NSubstitute;

namespace ZCrew.Extensions.Tasks.UnitTests;

public class SyncCancellationFuncTests
{
    [Fact]
    public async Task SyncCancellationFunc_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<CancellationToken, object>>();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(token);
    }

    [Fact]
    public async Task SyncCancellationFunc_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<CancellationToken, object>>();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive().Invoke(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncCancellationFunc_T_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, CancellationToken, object>>();
        var arg = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg, token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg, token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(arg, token);
    }

    [Fact]
    public async Task SyncCancellationFunc_T_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, CancellationToken, object>>();
        var arg = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, CancellationToken, object>>();
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
        func.Received(1).Invoke(arg1, arg2, token);
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, CancellationToken, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, CancellationToken, object>>();
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
        func.Received(1).Invoke(arg1, arg2, arg3, token);
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, CancellationToken, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive()
            .Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, CancellationToken, object>>();
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
        func.Received(1).Invoke(arg1, arg2, arg3, arg4, token);
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, CancellationToken, object>>();
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
        func.DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_T5_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, object, CancellationToken, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2, arg3, arg4, arg5, token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, arg5, token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(arg1, arg2, arg3, arg4, arg5, token);
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_T5_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, object, CancellationToken, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, arg5, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_T5_T6_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, object, object, CancellationToken, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, token);
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_T5_T6_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<Func<object, object, object, object, object, object, CancellationToken, object>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_T5_T6_T7_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<
            Func<object, object, object, object, object, object, object, CancellationToken, object>
        >();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var arg7 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, token);
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_T5_T6_T7_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<
            Func<object, object, object, object, object, object, object, CancellationToken, object>
        >();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var arg7 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_T5_T6_T7_T8_WhenInvoked_ShouldCallFuncOnce()
    {
        // Arrange
        var func = Substitute.For<
            Func<object, object, object, object, object, object, object, object, CancellationToken, object>
        >();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var arg7 = new object();
        var arg8 = new object();
        var token = TestContext.Current.CancellationToken;
        var result = new object();

        func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, token).Returns(result);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var actualResult = await syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, token);

        // Assert
        Assert.Equal(result, actualResult);
        func.Received(1).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, token);
    }

    [Fact]
    public async Task SyncCancellationFunc_T1_T2_T3_T4_T5_T6_T7_T8_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallFunc()
    {
        // Arrange
        var func = Substitute.For<
            Func<object, object, object, object, object, object, object, object, CancellationToken, object>
        >();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var arg7 = new object();
        var arg8 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncFunc = func.AsAsyncFunc();
        var invoke = () => syncFunc.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        func.DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<CancellationToken>()
            );
    }
}
