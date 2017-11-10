using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.Kafka;
using MultiPull.Contracts.Commands;

namespace MultiPull.MessageBusWithKafka
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("MultiPull.MessageBusWithKafka");

            var transport = endpointConfiguration.UseTransport<KafkaTransport>();
            transport.ConnectionString("172.29.10.25:9092");
            
            var persistance = endpointConfiguration.UsePersistence<InMemoryPersistence>();
            
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            var order = CreateOrder();

            await endpointInstance.Send("MultiPull.Clients.StoreProccessor", order);
            
            while (true)
            {
                Console.WriteLine("Enter 'stop' to exit or 'new' to sending new message . . .");
                string input = Console.ReadLine();

                if (input != null && input.ToLower().Equals("stop"))
                {
                    await endpointInstance.Stop();
                    break;
                }

                if (input != null && input.ToLower().Equals("new"))
                    await endpointInstance.Send("MultiPull.Clients.StoreProccessor", order);
            }

            Console.ReadKey();
        }

        private static CreateOrderCommand CreateOrder()
        {
            return new CreateOrderCommand()
            {
                Id = Guid.NewGuid(),CustomerId = Guid.NewGuid(),
                OrderHeader = $"{Guid.NewGuid()} - test",
                TotalCount = 5,
                TotalPrice = 15.55
            };
        }
    }

    public class Message : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}