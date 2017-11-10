using System;
using System.Threading.Tasks;
using NServiceBus;

namespace MultiPull.Clients.OrderProcessor
{
    class Program
    {
        static void Main()
        {
            Console.Title = "OrderProcessor";

            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("MultiPull.Clients.OrderProcessor");

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.EnableInstallers();

            endpointConfiguration.UseSerialization<JsonSerializer>();
            
            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            await RunLoop(endpointInstance);

        }

        private static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (input != null && input.ToLower().Equals("exit"))
                    break;
            }

            await endpointInstance.Stop();
        }
    }
}
