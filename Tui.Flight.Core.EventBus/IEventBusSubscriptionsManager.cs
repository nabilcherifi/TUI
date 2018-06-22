using System;
using System.Collections.Generic;

namespace Tui.Flights.Core.EventBus
{
    /// <summary>
    /// IEventBusSubscriptionsManager
    /// </summary>
    public interface IEventBusSubscriptionsManager
    {
        /// <summary>
        /// IsEmpty
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// EventHandler
        /// </summary>
        event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// AddSubscription
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="TH">TH</typeparam>
        void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationMessageHandler<T>;

        /// <summary>
        /// RemoveSubscription
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="TH">TH</typeparam>
        void RemoveSubscription<T, TH>() where TH : IIntegrationMessageHandler<T> where T : IntegrationEvent;

        /// <summary>
        /// GetEventKey
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>string</returns>
        string GetEventKey<T>();

        /// <summary>
        /// GetEventTypeByName
        /// </summary>
        /// <param name="eventName">eventName</param>
        /// <returns>Type</returns>
        Type GetEventTypeByName(string eventName);

        /// <summary>
        /// GetHandlersForEvent
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>IEnumerable</returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;

        /// <summary>
        /// GetHandlersForEvent
        /// </summary>
        /// <param name="eventName">eventName</param>
        /// <returns>IEnumerable</returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        /// <summary>
        /// HasSubscriptionsForEvent
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>bool</returns>
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

        /// <summary>
        /// HasSubscriptionsForEvent
        /// </summary>
        /// <param name="eventName">eventName</param>
        /// <returns>bool</returns>
        bool HasSubscriptionsForEvent(string eventName);

        /// <summary>
        /// Clear
        /// </summary>
        void Clear();
    }
}
