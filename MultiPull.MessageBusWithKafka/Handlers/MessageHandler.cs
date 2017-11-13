using System;
using System.Threading.Tasks;
using MultiPull.Contracts.Commands;
using MultiPull.Contracts.Events;
using NServiceBus;

namespace MultiPull.MessageBusWithKafka.Handlers
{
    public class MessageHandler : IHandleMessages<Message>, IHandleMessages<CreateOrderCommand>
    {
        public async Task Handle(Message message, IMessageHandlerContext context)
        {
            await Console.Out.WriteLineAsync($"Message with id was handled {message.Id}");
        }

        public async Task Handle(CreateOrderCommand message, IMessageHandlerContext context)
        {
            await Console.Out.WriteLineAsync($"Start creating new order {message.Id}");
            
            await context.Publish<ICreateOrderEvent>(inst =>
            {
                inst.Id = message.Id;
                inst.CustomerId = message.CustomerId;
                inst.OrderHeader = message.OrderHeader;
                inst.TotalCount = message.TotalCount;
                inst.TotalPrice = message.TotalPrice;
            });
        }
    }
}
