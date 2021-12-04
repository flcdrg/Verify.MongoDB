using MongoDB.Driver.Core.Events;

namespace Verify.MongoDB;

public class VerifyTestsEventSubscriber : IEventSubscriber
{
    public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
    {
        throw new NotImplementedException();
    }
}