using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using MultiPull.Contracts.Commands;
using MultiPull.Contracts.Events;

namespace MultiPull.MessageBus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "MessageBus";

            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("MultiPull.MessageBus");

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            endpointConfiguration.UseSerialization<JsonSerializer>();

            var routing = transport.Routing();

            RegisterPublishers(routing);

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            while (true)
            {
                string input = Console.ReadLine();

                if (input != null && input.ToLower().Equals("exit"))
                {
                    break;
                }

                if(input != null && input.ToLower().Equals("new-order"))
                {
                    // other commands
                    await CreateNewOrder(endpointInstance);
                }
            }

            await endpointInstance.Stop();
        }

        private static async Task CreateNewOrder(IEndpointInstance endpointInstance)
        {
            var order = new CreateOrderCommand
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                OrderHeader = Guid.NewGuid().ToString(),

                TotalCount = 8,
                TotalPrice = 550.5d
            };

            await endpointInstance.SendLocal(order);
        }

        private static void RegisterPublishers(RoutingSettings routing)
        {
            var assembly = typeof(ICreateOrderEvent).Assembly;

            routing.RouteToEndpoint(assembly:assembly, destination:"MultiPull.Clients.ClientProcessor");
            routing.RouteToEndpoint(assembly: assembly, destination: "MultiPull.Clients.OrderProcessor");
        }
    }
}