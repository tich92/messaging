using System;
using MultiPull.Contracts.Models;
using MultiPull.EventStore.StoreTypes;

namespace MultiPull.EventStore.Events
{
    public class AgreeOrderEvent : IEvent
    {
        public Guid Id { get; }
        public Order Order { get; set; }

        public AgreeOrderEvent(Guid id, Order order)
        {
            Id = id;
            Order = order;
        }
    }
}