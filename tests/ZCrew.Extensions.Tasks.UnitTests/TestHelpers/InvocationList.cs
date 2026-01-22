using System.Collections;
using System.Collections.Concurrent;

namespace ZCrew.Extensions.Tasks.UnitTests.TestHelpers;

internal class InvocationList<TToken> : IEnumerable<TToken>
    where TToken : notnull
{
    private readonly ConcurrentDictionary<TToken, DateTime> invocations = new();

    public IEnumerator<TToken> GetEnumerator()
    {
        return this
            .invocations.ToArray()
            .OrderBy(invocation => invocation.Value)
            .Select(invocation => invocation.Key)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Register(TToken token)
    {
        this.invocations[token] = DateTime.UtcNow;
    }
}
