using NSubstitute;

namespace ZCrew.Extensions.Tasks.UnitTests;

public class SyncCancellationActionTests
{
    [Fact]
    public async Task SyncCancellationAction_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Action<CancellationToken>>();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(token);

        // Assert
        action.Received(1).Invoke(token);
    }

    [Fact]
    public async Task SyncCancellationAction_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Action<CancellationToken>>();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        action.DidNotReceive().Invoke(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncCancellationAction_T_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Action<object, CancellationToken>>();
        var arg = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg, token);

        // Assert
        action.Received(1).Invoke(arg, token);
    }

    [Fact]
    public async Task SyncCancellationAction_T_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Action<object, CancellationToken>>();
        var arg = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        action.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncCancellationAction_T1_T2_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Action<object, object, CancellationToken>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, token);

        // Assert
        action.Received(1).Invoke(arg1, arg2, token);
    }

    [Fact]
    public async Task SyncCancellationAction_T1_T2_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Action<object, object, CancellationToken>>();
        var arg1 = new object();
        var arg2 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        action.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncCancellationAction_T1_T2_T3_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Action<object, object, object, CancellationToken>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        action.Received(1).Invoke(arg1, arg2, arg3, token);
    }

    [Fact]
    public async Task SyncCancellationAction_T1_T2_T3_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Action<object, object, object, CancellationToken>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(arg1, arg2, arg3, token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        action
            .DidNotReceive()
            .Invoke(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SyncCancellationAction_T1_T2_T3_T4_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Action<object, object, object, object, CancellationToken>>();
        var arg1 = new object();
        var arg2 = new object();
        var arg3 = new object();
        var arg4 = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg1, arg2, arg3, arg4, token);

        // Assert
        action.Received(1).Invoke(arg1, arg2, arg3, arg4, token);
    }

    [Fact]
    public async Task SyncCancellationAction_T1_T2_T3_T4_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Action<object, object, object, object, CancellationToken>>();
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
        action
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
