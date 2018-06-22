namespace Tui.Flights.Core.EventBus
{
    /// <summary>
    /// IEventBus
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publish
        /// </summary>
        /// <param name="reportEvent">reportEvent</param>
        void Publish(IntegrationEvent reportEvent);

        /// <summary>
        /// Subscribe
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="TH">TH</typeparam>
        void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationMessageHandler<T>;

        /// <summary>
        /// Unsubscribe
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="TH">TH</typeparam>
        void Unsubscribe<T, TH>() where TH : IIntegrationMessageHandler<T> where T : IntegrationEvent;
    }
}
