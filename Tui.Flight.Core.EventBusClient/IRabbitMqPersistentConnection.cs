namespace Tui.Flights.Core.EventBusClient
{
    using System;
    using RabbitMQ.Client;

    /// <summary>
    /// INetMQPersistentConnection
    /// </summary>
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        /// <summary>
        /// Gets serviceBusConnectionString
        /// </summary>
        string ServiceBusConnectionString { get; }

        /// <summary>
        /// Gets a value indicating whether isConnected
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// TryConnect
        /// </summary>
        /// <returns>bool</returns>
        bool TryConnect();

        /// <summary>
        /// CreateModel
        /// </summary>
        /// <returns>IModel</returns>
        IModel CreateModel();
    }
}
