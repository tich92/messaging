using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace MultiPull.EventStore.StoreTypes
{
    public class EventStoreContext
    {
        private static IEventStoreConnection _connection;

        public ICollection<IEvent> Events { get; set; }

        public EventStoreContext()
        {
            _connection = InitEventStore();
            Events = new List<IEvent>();
        }

        private IEventStoreConnection InitEventStore()
        {
            _connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));

            _connection.ConnectAsync();

            return _connection;
        }

        public void Commit()
        {
            foreach (var @event in Events)
            {
                var jsonString = JsonConvert.SerializeObject(@event,
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None });

                var jsonPayLoad = Encoding.UTF8.GetBytes(jsonString);

                string eventType = @event.GetType().FullName;

                var eventStoreDataType = new EventData(Guid.NewGuid(), eventType, true, jsonPayLoad, null);

                _connection.AppendToStreamAsync($"Event with Id {@event.Id}", ExpectedVersion.Any, eventStoreDataType);
            }

            Events.Clear();
        }

        public async Task GetAllEvents()
        {
            var allEvents = new List<ResolvedEvent>();

            AllEventsSlice currentSlice;

            var nextSliceStart = Position.Start;

            do
            {
                currentSlice = await _connection.ReadAllEventsForwardAsync(nextSliceStart, 200, false);

                nextSliceStart = currentSlice.NextPosition;
                allEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);

            Console.ReadLine();
        }

        public async Task GetEventsByStreamAsync<TOrder>(Guid entityId)
        {
            string streamName = $"Event with Id {entityId}";
            
            var result =
                await _connection.ReadStreamEventsForwardAsync(streamName, StreamPosition.Start, 999, false);

            foreach (var @event in result.Events)
            {
                var type = Type.GetType(@event.Event.EventType);

                string dataString = Encoding.UTF8.GetString(@event.Event.Data);

                var order = JsonConvert.DeserializeObject<TOrder>(dataString);

                //var obj = Activator.CreateInstance(type, )
            }
        }
    }
}
