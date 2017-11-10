using System;
using MultiPull.Contracts.Events;

namespace MultiPull.Contracts.Commands
{
    //urn:message:MultiPull.Contracts.Commands:CreateCustomerOrderCommand
    public class CreateCustomerOrderCommand : ICreateOrderEvent
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string OrderHeader { get; set; }
        public double TotalPrice { get; set; }
        public int TotalCount { get; set; }
    }
}
