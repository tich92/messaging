using System;
using System.Threading.Tasks;
using MultiPull.Contracts.Commands;
using MultiPull.Contracts.Events;
using NServiceBus;
using NServiceBus.Logging;

namespace MultiPull.Clients.ClientProcessor.Handlers
{
    public class OrderHandler : IHandleMessages<ICreateOrderEvent>
    {
        private ILog logging = LogManager.GetLogger<OrderHandler>();

        public async Task Handle(ICreateOrderEvent message, IMessageHandlerContext context)
        {
            await Console.Out.WriteAsync($"Income new order! Customer Id: {message.CustomerId}");
            await Console.Out.WriteLineAsync($"Order ({message.Id}) for customer {message.CustomerId} - successful!");
        }
    }
}
