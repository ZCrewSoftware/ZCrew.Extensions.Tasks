using NSubstitute;

namespace ZCrew.Extensions.Tasks.UnitTests;

public class ValueTaskCancellationActionTests
{
    [Fact]
    public async Task ValueTaskCancellationAction_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<CancellationToken, ValueTask>>();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(token);

        // Assert
        await action.Received(1).Invoke(token);
    }

    [Fact]
    public async Task ValueTaskCancellationAction_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<CancellationToken, ValueTask>>();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action.DidNotReceive().Invoke(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValueTaskCancellationAction_T_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, CancellationToken, ValueTask>>();
        var arg = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg, token);

        // Assert
        await action.Received(1).Invoke(arg, token);
    }

    [Fact]
    public async Task ValueTaskCancellationAction_T_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, CancellationToken, ValueTask>>();
        var arg = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValueTaskCancellationAction_T1_T2_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, CancellationToken, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2, token);
    }

    [Fact]
    public async Task ValueTaskCancellationAction_T1_T2_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, CancellationToken, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValueTaskCancellationAction_T1_T2_T3_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, CancellationToken, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2, arg3, token);
    }

    [Fact]
    public async Task ValueTaskCancellationAction_T1_T2_T3_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, CancellationToken, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action
            .DidNotReceive()
            .Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValueTaskCancellationAction_T1_T2_T3_T4_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, CancellationToken, ValueTask>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, arg4, token);

        // Assert
        await action.Received(1).Invoke(arg1, arg2, arg3, arg4, token);
    }

    [Fact]
    public async Task ValueTaskCancellationAction_T1_T2_T3_T4_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, CancellationToken, ValueTask>>();
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
        await action
            .DidNotReceive()
            .Invoke(
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<object>(),
                Arg.Any<CancellationToken>()
            );
    }
}
