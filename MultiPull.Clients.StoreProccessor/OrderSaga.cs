using System;
using System.Threading.Tasks;
using MultiPull.Contracts.Events;
using NServiceBus;

namespace MultiPull.Clients.StoreProccessor
{
    public class OrderSaga : Saga<OrderSagaData>, IAmStartedByMessages<ICreateOrderEvent>, IHandleMessages<IPurchaseOrderEvent>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<ICreateOrderEvent>(message => message.Id)
                .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<IPurchaseOrderEvent>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        }

        public async Task Handle(ICreateOrderEvent message, IMessageHandlerContext context)
        {
            await Console.Out.WriteLineAsync($"Created new order {message.Id}");
        }

        public async Task Handle(IPurchaseOrderEvent message, IMessageHandlerContext context)
        {
            await Console.Out.WriteLineAsync($"Order was purchased {message.OrderId}");
        }
    }

    public class OrderSagaData : ContainSagaData
    {
        public Guid OrderId { get; set; }
    }
}
