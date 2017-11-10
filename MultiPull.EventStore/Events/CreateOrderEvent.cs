using System;
using MultiPull.Contracts.Models;
using MultiPull.EventStore.StoreTypes;

namespace MultiPull.EventStore.Events
{
    public class CreateOrderEvent : IEvent
    {
        public Guid Id { get; }

        public Order Order { get; }

        public CreateOrderEvent(Guid id, Order order)
        {
            Id = id;
            Order = order;
        }
    }
}
