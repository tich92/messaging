using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.Kafka;

namespace MultiPull.Clients.StoreProccessor
{
    class Program
    {
        static readonly Dictionary<string, string> CommandDictionary = new Dictionary<string, string>()
        {
            {"new", "creating new request" },
            {"stop", "stoping service bus instance" }
        };

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var endpointConfig = new EndpointConfiguration("MultiPull.Clients.StoreProccessor");
            
            var transport = endpointConfig.UseTransport<KafkaTransport>();
            transport.ConnectionString("172.29.10.25:9092");
            
            endpointConfig.SendFailedMessagesTo("error");
            endpointConfig.UsePersistence<InMemoryPersistence>();
            endpointConfig.EnableInstallers();

            endpointConfig.UseSerialization<JsonSerializer>();

            var endpointInstance = await Endpoint.Start(endpointConfig);

            while (true)
            {
                Console.WriteLine("Input command or 'help' to work with console:");

                string input = Console.ReadLine()?.ToLower();

                if (input != null && input.Equals("help"))
                {
                    foreach (var command in CommandDictionary)
                    {
                        Console.WriteLine($"{command.Key} - {command.Value}");
                    }
                }
                if (input != null && input.Equals("stop"))
                {
                    await endpointInstance.Stop();
                    break;
                }

            }
        }
    }
}
