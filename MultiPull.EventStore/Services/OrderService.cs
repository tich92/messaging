using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiPull.Contracts.Models;
using MultiPull.EventStore.Events;
using MultiPull.EventStore.StoreTypes;

namespace MultiPull.EventStore.Services
{
    public class OrderService
    {
        private readonly DataContext<Order> _orderContext;
        private readonly EventStoreContext _context;

        public OrderService(EventStoreContext context, DataContext<Order> orderContext)
        {
            _context = context;
            _orderContext = orderContext;
        }

        public void Create(Order order)
        {
            var @event = new CreateOrderEvent(order.Id, order);

            _orderContext.Add(order);

            _context.Events.Add(@event);
            _context.Commit();
        }

        public void Agree(Guid orderId)
        {
            var order = _orderContext.FirstOrDefault(o => o.Id == orderId);

            if(order == null)
                throw new Exception("Cannot agree order with id " + orderId);

            order.Agree();

            _context.Events.Add(new AgreeOrderEvent(orderId, order));
            _context.Commit();
        }

        public void ChangeOrder(Order order)
        {
            var existsOrder = _orderContext.FirstOrDefault(o => o.Id == order.Id);

            _orderContext.Remove(existsOrder);

            _orderContext.Add(existsOrder);

            _context.Events.Add(new ChangeOrderEvent(order.Id, order));
            _context.Commit();
        }

        public IEnumerable<Order> GetOrders()
        {
            return _orderContext;
        }

        public async Task GetOrderEvents(Guid orderId)
        {
            await _context.GetEventsByStreamAsync<Order>(orderId);
        }
    }
}
