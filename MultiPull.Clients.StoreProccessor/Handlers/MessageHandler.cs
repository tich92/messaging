using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using MultiPull.Contracts.Commands;

namespace MultiPull.Clients.StoreProccessor.Handlers
{
    public class MessageHandler : IHandleMessages<CreateOrderCommand>
    {
        public async Task Handle(CreateOrderCommand message, IMessageHandlerContext context)
        {
            await Console.Out.WriteLineAsync($"Order with Id {message.Id} was send on Kafka transport");
            await Console.Out.WriteLineAsync("Order processed successful");
        }
    }
}
