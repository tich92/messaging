using System;
using NServiceBus;

namespace MultiPull.Contracts.Events
{
    public interface ICreateOrderEvent : IEvent
    {
        Guid Id { get; set; }
        Guid CustomerId { get; set; }

        string OrderHeader { get; set; }

        double TotalPrice { get; set; }

        int TotalCount { get; set; }
    }
}