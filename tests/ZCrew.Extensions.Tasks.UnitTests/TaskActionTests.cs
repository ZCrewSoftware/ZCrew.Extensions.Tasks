using NSubstitute;

namespace ZCrew.Extensions.Tasks.UnitTests;

public class TaskActionTests
{
    [Fact]
    public async Task TaskAction_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<Task>>();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(token);

        // Assert
        await action.Received(1).Invoke();
    }

    [Fact]
    public async Task TaskAction_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<Task>>();
        var token = new CancellationToken(canceled: true);

        // Act
        var syncAction = action.AsAsyncAction();
        var invoke = () => syncAction.InvokeAsync(token);

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(invoke);
        await action.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task TaskAction_T_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, Task>>();
        var arg = new object();
        var token = TestContext.Current.CancellationToken;

        // Act
        var syncAction = action.AsAsyncAction();
        await syncAction.InvokeAsync(arg, token);

        // Assert
        await action.Received(1).Invoke(arg);
    }

    [Fact]
    public async Task TaskAction_T_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, Task>>();
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
    public async Task TaskAction_T1_T2_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, Task>>();
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
    public async Task TaskAction_T1_T2_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, Task>>();
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
    public async Task TaskAction_T1_T2_T3_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, Task>>();
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
    public async Task TaskAction_T1_T2_T3_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, Task>>();
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
    public async Task TaskAction_T1_T2_T3_T4_WhenInvoked_ShouldCallActionOnce()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, Task>>();
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
    public async Task TaskAction_T1_T2_T3_T4_WhenTokenIsCanceled_ShouldThrowExceptionAndNotCallAction()
    {
        // Arrange
        var action = Substitute.For<Func<object, object, object, object, Task>>();
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
}
