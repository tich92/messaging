using System;

namespace MultiPull.EventStore.StoreTypes
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}