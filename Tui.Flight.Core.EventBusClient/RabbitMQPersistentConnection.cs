namespace Tui.Flights.Core.EventBusClient
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    // ReSharper disable All

    /// <summary>
    ///  RabbitMqPersistentConnection : IRabbitMqPersistentConnection
    /// </summary>
    public class RabbitMqPersistentConnection : IRabbitMqPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqPersistentConnection> _logger;
        private readonly int _retryCount;
        private IConnection _connection;
        private object syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqPersistentConnection"/> class.
        /// RabbitMqPersistentConnection
        /// </summary>
        /// <param name="connectionFactory">connectionFactory</param>
        /// <param name="logger">logger</param>
        /// <param name="retryCount">retryCount</param>
        public RabbitMqPersistentConnection(
            IConnectionFactory connectionFactory,
            ILogger<RabbitMqPersistentConnection> logger,
            int retryCount = 5)
        {
            this._connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._retryCount = retryCount;
        }

        /// <summary>
        /// Gets serviceBusConnectionString
        /// </summary>
        public string ServiceBusConnectionString { get; }

        /// <summary>
        /// Gets a value indicating whether isConnected
        /// </summary>
        public bool IsConnected => this._connection != null && this._connection.IsOpen && !this._disposed;

        /// <summary>
        /// CreateModel
        /// </summary>
        /// <returns>IModel</returns>
        public IModel CreateModel()
        {
            if (!this.IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return this._connection.CreateModel();
        }

        /// <summary>
        /// TryConnect
        /// </summary>
        /// <returns>bool</returns>
        public bool TryConnect()
        {
            this._logger?.LogInformation("RabbitMQ Client is trying to connect");

            lock (this.syncRoot)
            {
                this._connection = this._connectionFactory.CreateConnection();

                if (this.IsConnected)
                {
                    this._connection.ConnectionShutdown += this.OnConnectionShutdown;
                    this._connection.CallbackException += this.OnCallbackException;
                    this._connection.ConnectionBlocked += this.OnConnectionBlocked;

                    this._logger?.LogInformation(
                        $"RabbitMQ persistent connection acquired a connection {this._connection.Endpoint.HostName} and is subscribed to failure events");

                    return true;
                }
                else
                {
                    this._logger?.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    return false;
                }
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (this._disposed)
            {
                return;
            }

            this._logger?.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            this.TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (this._disposed)
            {
                return;
            }

            this._logger?.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            this.TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (this._disposed)
            {
                return;
            }

            this._logger?.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            this.TryConnect();
        }

        private bool _disposed;

        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;

            try
            {
                this._connection?.Dispose();
            }
            catch (IOException ex)
            {
                this._logger?.LogCritical(ex.ToString());
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// RabbitMQMessageEventArgs
    /// </summary>
    public class RabbitMqMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageEventArgs"/> class.
        /// RabbitMQMessageEventArgs
        /// </summary>
        /// <param name="eventName">eventName</param>
        /// <param name="message">message</param>
        public RabbitMqMessageEventArgs(string eventName, string message)
        {
            this.EventName = eventName;
            this.Message = message;
        }

        /// <summary>
        /// Gets eventName
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        /// Gets message
        /// </summary>
        public string Message { get; private set; }
    }
}
