using System.Threading.Tasks;
using NServiceBus.MessageMutator;

namespace MultiPull.MessageBus.Mutations
{
    public class MutateIncomingMessage : IMutateIncomingMessages
    {
        public Task MutateIncoming(MutateIncomingMessageContext context)
        {
            var headers = context.Headers;

            return Task.CompletedTask;
        }
    }
}