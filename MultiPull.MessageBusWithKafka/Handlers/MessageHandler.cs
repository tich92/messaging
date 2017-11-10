using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace MultiPull.MessageBusWithKafka.Handlers
{
    public class MessageHandler : IHandleMessages<Message>
    {
        public async Task Handle(Message message, IMessageHandlerContext context)
        {
            await Console.Out.WriteLineAsync($"Message with id was handled {message.Id}");
        }
    }
}
