using System;
using System.Threading.Tasks;
using NServiceBus;

namespace MultiPull.Clients.ClientProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ClientProcessor";

            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("MultiPull.Clients.ClientProcessor");

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();

            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
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
