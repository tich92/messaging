using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.Kafka;
using MultiPull.Contracts.Commands;
using MultiPull.Contracts.Events;

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


            var routing = transport.Routing();

            RegisterPublishers(routing);

            var persistance = endpointConfiguration.UsePersistence<InMemoryPersistence>();
            //var connection = @"Data Source=.\;Initial Catalog=NSBPersistence;Integrated Security=True";

            //persistance.SqlVariant(SqlVariant.MsSqlServer);
            //persistance.ConnectionBuilder(() => new SqlConnection(connection));

            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            var order = CreateOrder();

            //await endpointInstance.Send("MultiPull.Clients.StoreProccessor", order);
            
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
                {
                    await endpointInstance.Send(order);
                    //await endpointInstance.Send("MultiPull.MessageBusWithKafka", order);
                }
            }

            Console.ReadKey();
        }

        private static void RegisterPublishers(RoutingSettings<KafkaTransport> routing)
        {
            var assembly = typeof(ICreateOrderEvent).Assembly;

            routing.RouteToEndpoint(assembly: assembly, destination: "MultiPull.Clients.StoreProcessor");
            routing.RouteToEndpoint(typeof(CreateOrderCommand).Assembly, "MultiPull.MessageBusWithKafka");
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