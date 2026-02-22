using System.Threading.Channels;

namespace ZCrew.Extensions.Tasks.Dispatching;

/// <summary>
///     A <see cref="ChannelDispatcher"/> backed by a bounded channel, which applies backpressure
///     when the operation queue reaches the specified capacity.
/// </summary>
internal sealed class BoundedChannelDispatcher : ChannelDispatcher
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BoundedChannelDispatcher"/> class.
    /// </summary>
    /// <param name="capacity">The maximum number of operations the channel can hold before applying backpressure.</param>
    public BoundedChannelDispatcher(int capacity)
        : base(CreateChannel(capacity)) { }

    private static Channel<IDispatchedOperation> CreateChannel(int capacity)
    {
        var options = new BoundedChannelOptions(capacity) { SingleReader = true };
        return Channel.CreateBounded<IDispatchedOperation>(options);
    }
}
