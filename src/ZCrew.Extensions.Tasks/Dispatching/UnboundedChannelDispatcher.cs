using System.Threading.Channels;

namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     A <see cref="ChannelDispatcher"/> backed by an unbounded channel, allowing an unlimited number of operations to
/// be queued.
/// </summary>
internal sealed class UnboundedChannelDispatcher : ChannelDispatcher
{
    private static readonly UnboundedChannelOptions defaultDispatcherOptions = new() { SingleReader = true };

    /// <summary>
    ///     Initializes a new instance of the <see cref="UnboundedChannelDispatcher"/> class.
    /// </summary>
    public UnboundedChannelDispatcher()
        : base(CreateChannel()) { }

    private static Channel<IDispatchedOperation> CreateChannel()
    {
        return Channel.CreateUnbounded<IDispatchedOperation>(defaultDispatcherOptions);
    }
}
