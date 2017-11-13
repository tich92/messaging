using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.Kafka;

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

            var transport = endpointConfiguration.UseTransport<KafkaTransport>();
            transport.ConnectionString("172.29.10.25:9092");
            
            var persistance = endpointConfiguration.UsePersistence<SqlPersistence>();
            var connection = @"Data Source=.\;Initial Catalog=NSBPersistence;Integrated Security=True";

            persistance.SqlVariant(SqlVariant.MsSqlServer);
            persistance.ConnectionBuilder(() => new SqlConnection(connection));

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
