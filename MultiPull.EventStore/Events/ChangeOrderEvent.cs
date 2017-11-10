using System;
using MultiPull.Contracts.Models;
using MultiPull.EventStore.StoreTypes;

namespace MultiPull.EventStore.Events
{
    public class ChangeOrderEvent : IEvent
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }

        public ChangeOrderEvent(Guid id, Order order)
        {
            Id = id;
            Order = order;
        }
    }
}
