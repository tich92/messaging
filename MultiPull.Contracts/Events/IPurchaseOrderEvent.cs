using System;
using NServiceBus;

namespace MultiPull.Contracts.Events
{
    public interface IPurchaseOrderEvent : IEvent
    {
        Guid OrderId { get; set; }
    }
}
