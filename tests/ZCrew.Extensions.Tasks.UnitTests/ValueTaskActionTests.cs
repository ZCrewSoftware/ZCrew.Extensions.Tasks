using NSubstitute;

namespace ZCrew.Extensions.Tasks.UnitTests;

public class ValueTaskActionTests
{
    [Fact]
    public async Task ValueTaskAction_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<ValueTask>>();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(token);

        // Assert
        await action.Received(1).Invoke();
    }

    [Fact]
    public async Task ValueTaskAction_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<ValueTask>>();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task ValueTaskAction_T_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, ValueTask>>();
        var arg = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg, token);

        // Assert
        await action.Received(1).Invoke(arg);
    }

    [Fact]
    public async Task ValueTaskAction_T_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, ValueTask>>();
        var arg = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action.DidNotReceive().Invoke(Arg.Any<object>());
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2);
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>());
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2, arg3);
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>());
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, arg4, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2, arg3, arg4);
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, arg3, arg4, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>());
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_T5_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, arg4, arg5, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_T5_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, arg3, arg4, arg5, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action
            .DidNotReceive()
            .Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>());
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_T5_T6_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_T5_T6_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action
            .DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>()
            );
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_T5_T6_T7_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var arg7 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_T5_T6_T7_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var arg7 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action
            .DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>()
            );
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_T5_T6_T7_T8_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, object, object, object, object, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var arg5 = new object();
        var arg6 = new object();
        var arg7 = new object();
        var arg8 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    [Fact]
    public async Task ValueTaskAction_T1_T2_T3_T4_T5_T6_T7_T8_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, object, object, object, object, ValueTask>>();
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
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action
            .DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>()
            );
    }
}
