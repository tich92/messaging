using System;
using MultiPull.Contracts.Events;
using NServiceBus;

namespace MultiPull.Contracts.Commands
{
    public class CreateOrderCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        public string OrderHeader { get; set; }

        public double TotalPrice { get; set; }

        public int TotalCount { get; set; }
    }
}
