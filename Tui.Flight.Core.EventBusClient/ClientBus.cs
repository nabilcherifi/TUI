// ReSharper disable All
namespace Tui.Flights.Core.EventBusClient
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.Exceptions;
    using Tui.Flights.Core.EventBus;
    using Tui.Flights.Core.EventBusClient;

    /// <summary>
    /// ClientBus
    /// </summary>
    public class ClientBus : IEventBus, IDisposable
    {
        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly ILogger<ClientBus> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        private IModel _consumerChannel;
        private string _queueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBus"/> class.
        /// ClientBus Constructor
        /// </summary>
        public ClientBus(
            IRabbitMqPersistentConnection persistentConnection,
            ILogger<ClientBus> logger,
            IServiceProvider svcProvider,
            IEventBusSubscriptionsManager subsManager,
            string queueName)
        {
            this._persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._queueName = string.Concat(QueueContext.TuiQueue, ".", queueName);
            this._serviceProvider = svcProvider;
            this._subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            this._subsManager.OnEventRemoved += this.SubsManager_OnEventRemoved;

            this._consumerChannel = this.CreateConsumerChannel();
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!this._persistentConnection.IsConnected)
            {
                this._persistentConnection.TryConnect();
            }

            using (var channel = this._persistentConnection.CreateModel())
            {
                channel.QueueUnbind(
                    queue: this._queueName,
                    exchange: QueueContext.TuiExchange,
                    routingKey: eventName);

                if (this._subsManager.IsEmpty)
                {
                    this._queueName = string.Empty;
                    this._consumerChannel.Close();
                }
            }
        }

        /// <summary>
        /// Subscribe
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="TH">TH</typeparam>
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationMessageHandler<T>
        {
            var eventName = this._subsManager.GetEventKey<T>();
            this.DoInternalSubscription(eventName);
            this._subsManager.AddSubscription<T, TH>();
        }

        /// <summary>
        /// Unsubscribe
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="TH">TH</typeparam>
        public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationMessageHandler<T>
        {
            this._subsManager.RemoveSubscription<T, TH>();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing ressources
        /// </summary>
        /// <param name="disposing">Controlling managed ressources disposal</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._consumerChannel != null)
                {
                    this._consumerChannel.Dispose();
                }
                this._subsManager.Clear();
            }
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = this._subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!this._persistentConnection.IsConnected)
                {
                    this._persistentConnection.TryConnect();
                }

                using (var channel = this._persistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: this._queueName, exchange: QueueContext.TuiExchange, routingKey: eventName);
                }
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!this._persistentConnection.IsConnected)
            {
                this._persistentConnection.TryConnect();
            }

            var channel = this._persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: QueueContext.TuiExchange, type: QueueContext.TuiType);

            channel.QueueDeclare(queue: this._queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);

                this.ProcessEvent(eventName, message);

                channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: this._queueName, autoAck: false, consumer: consumer);
            channel.CallbackException += (sender, ea) =>
            {
                this._consumerChannel.Dispose();
                this._consumerChannel = this.CreateConsumerChannel();
            };

            return channel;
        }

        private void ProcessEvent(string eventName, string message)
        {
            if (this._subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = this._subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var eventType = this._subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var handler = this._serviceProvider.GetService(subscription.HandlerType);
                    var concreteType = typeof(IIntegrationMessageHandler<>).MakeGenericType(eventType);

                    concreteType.GetMethod("Handle")?.Invoke(handler, new object[] { integrationEvent });
                }
            }
        }

        /// <summary>
        /// Publish
        /// </summary>
        /// <param name="reportEvent">reportEvent</param>
        public void Publish(IntegrationEvent reportEvent)
        {
            if (!this._persistentConnection.IsConnected)
            {
                this._persistentConnection.TryConnect();
            }

            using (var channel = this._persistentConnection.CreateModel())
            {
                var eventName = reportEvent.GetType().Name;

                channel.ExchangeDeclare(exchange: QueueContext.TuiExchange, type: QueueContext.TuiType);

                var message = JsonConvert.SerializeObject(reportEvent);
                var body = Encoding.UTF8.GetBytes(message);

                if (channel != null)
                {
                    var properties = channel.CreateBasicProperties();
                     properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(exchange: QueueContext.TuiExchange, routingKey: eventName, mandatory: true, basicProperties: properties, body: body);
                }
            }
        }
    }
}
