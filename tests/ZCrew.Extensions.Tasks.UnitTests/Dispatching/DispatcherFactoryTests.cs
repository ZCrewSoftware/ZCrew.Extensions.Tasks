using ZCrew.Extensions.Tasks.Dispatching;

namespace ZCrew.Extensions.Tasks.UnitTests.Dispatching;

public class DispatcherFactoryTests
{
    [Fact]
    public void CreateDefault_WhenCalled_ShouldReturnSynchronizationContextDispatcher()
    {
        // Act
        var dispatcher = Dispatcher.CreateDefault();

        // Assert
        Assert.IsType<SynchronizationContextDispatcher>(dispatcher);
    }

    [Fact]
    public void CreateSynchronizationContextDispatcher_WhenCalled_ShouldReturnCorrectType()
    {
        // Act
        var dispatcher = Dispatcher.CreateSynchronizationContextDispatcher();

        // Assert
        Assert.IsType<SynchronizationContextDispatcher>(dispatcher);
    }

    [Fact]
    public void CreateUnboundedChannelDispatcher_WhenCalled_ShouldReturnHostedDispatcherOfCorrectType()
    {
        // Act
        var dispatcher = Dispatcher.CreateUnboundedChannelDispatcher();

        // Assert
        Assert.IsAssignableFrom<IHostedDispatcher>(dispatcher);
        Assert.IsType<UnboundedChannelDispatcher>(dispatcher);
    }

    [Fact]
    public void CreateBoundedChannelDispatcher_WhenCalled_ShouldReturnHostedDispatcherOfCorrectType()
    {
        // Act
        var dispatcher = Dispatcher.CreateBoundedChannelDispatcher(10);

        // Assert
        Assert.IsAssignableFrom<IHostedDispatcher>(dispatcher);
        Assert.IsType<BoundedChannelDispatcher>(dispatcher);
    }
}
