using System;
using System.Threading.Tasks;
using MultiPull.Contracts.Commands;
using MultiPull.Contracts.Events;
using NServiceBus;

namespace MultiPull.Clients.OrderProcessor.Handlers
{
    public class OrderHandler : IHandleMessages<ICreateOrderEvent>
    {
        public async Task Handle(ICreateOrderEvent message, IMessageHandlerContext context)
        {
            await Console.Out.WriteLineAsync($"Income new order {message.Id}");
            await Console.Out.WriteLineAsync("Order processor save new order - successful");
        }
    }
}
