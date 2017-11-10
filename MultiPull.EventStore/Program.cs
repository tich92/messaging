using MultiPull.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MultiPull.EventStore.Services;
using MultiPull.EventStore.Unity;
using Unity;

namespace MultiPull.EventStore
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = DependencyContainer.Register();

                var service = container.Resolve<OrderService>();

                for (int i = 0; i < 10; i++)
                {
                    var order = CreateOrder();

                    service.Create(order);

                    service.Agree(order.Id);
                }

                var orders = service.GetOrders();

                var changedOrder = ChangeOrder(orders);

                service.ChangeOrder(changedOrder);

                service.GetOrderEvents(changedOrder.Id).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.ReadLine();
        }

        private static Order ChangeOrder(IEnumerable<Order> orders)
        {
            var order = orders.FirstOrDefault();
            
            order?.AddItem(15, 44);

            return order;
        }

        private static Order CreateOrder()
        {
            var order = new Order();

            order.Initialize();

            order.AddItem(2, 6);
            order.AddItem(5, 66.6);
            order.AddItem(1, 50);
            order.AddItem(1, 105);

            return order;
        }
    }
}
