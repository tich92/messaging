using System.Threading.Tasks;
using MultiPull.Contracts.Commands;
using MultiPull.Contracts.Events;
using NServiceBus;

namespace MultiPull.MessageBus.Handler
{
    /// <inheritdoc />
    /// <summary>
    /// urn:message:
    /// </summary>
    public class OrderHandler : IHandleMessages<CreateOrderCommand>
    {
        public async Task Handle(CreateOrderCommand message, IMessageHandlerContext context)
        {
            await context.Publish<ICreateOrderEvent>(inst =>
            {
                inst.CustomerId = message.CustomerId;
                inst.Id = message.Id;
                inst.OrderHeader = message.OrderHeader;
                inst.TotalCount = message.TotalCount;
                inst.TotalPrice = message.TotalPrice;
            });
        }
    }
}
