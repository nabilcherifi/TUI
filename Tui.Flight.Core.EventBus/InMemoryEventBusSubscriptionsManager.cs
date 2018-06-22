using System;
using System.Collections.Generic;
using System.Linq;

namespace Tui.Flights.Core.EventBus
{
    /// <summary>
    /// InMemoryEventBusSubscriptionsManager
    /// </summary>
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly List<Type> _eventTypes;
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryEventBusSubscriptionsManager"/> class.
        /// InMemoryEventBusSubscriptionsManager
        /// </summary>
        public InMemoryEventBusSubscriptionsManager()
        {
            this._handlers = new Dictionary<string, List<SubscriptionInfo>>();
            this._eventTypes = new List<Type>();
        }

        /// <summary>
        /// IsEmpty
        /// </summary>
        public bool IsEmpty => !this._handlers.Keys.Any();

        /// <summary>
        /// EventHandler
        /// </summary>
        public event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// AddSubscription
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="TH">TH</typeparam>
        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationMessageHandler<T>
        {
            var eventName = this.GetEventKey<T>();
            this.DoAddSubscription(typeof(TH), eventName, false);
            this._eventTypes.Add(typeof(T));
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            this._handlers.Clear();
        }

        /// <summary>
        /// GetEventKey
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>string</returns>
        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// GetEventTypeByName
        /// </summary>
        /// <param name="eventName">eventName</param>
        /// <returns>Type</returns>
        public Type GetEventTypeByName(string eventName)
        {
            return this._eventTypes.SingleOrDefault(t => t.Name == eventName);
        }

        /// <summary>
        /// GetHandlersForEvent
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>IEnumerable</returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>()
            where T : IntegrationEvent
        {
            var key = this.GetEventKey<T>();
            return this.GetHandlersForEvent(key);
        }

        /// <summary>
        /// GetHandlersForEvent
        /// </summary>
        /// <param name="eventName">eventName</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
        {
            return this._handlers[eventName];
        }

        /// <summary>
        /// eventName
        /// </summary>
        /// <param name="eventName">param</param>
        /// <returns>bool</returns>
        public bool HasSubscriptionsForEvent(string eventName)
        {
            return this._handlers.ContainsKey(eventName);
        }

        /// <summary>
        /// HasSubscriptionsForEvent
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>bool</returns>
        public bool HasSubscriptionsForEvent<T>()
            where T : IntegrationEvent
        {
            var key = this.GetEventKey<T>();
            return this.HasSubscriptionsForEvent(key);
        }

        /// <summary>
        /// RemoveSubscription
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="TH">TH</typeparam>
        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationMessageHandler<T>
        {
            var handlerToRemove = this.FindSubscriptionToRemove<T, TH>();
            var eventName = this.GetEventKey<T>();
            this.DoRemoveHandler(eventName, handlerToRemove);
        }

        /// <summary>
        /// DoAddSubscription
        /// </summary>
        /// <param name="handlerType">handlerType</param>
        /// <param name="eventName">eventName</param>
        /// <param name="isDynamic">isDynamic</param>
        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!this.HasSubscriptionsForEvent(eventName))
            {
                this._handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (this._handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            this._handlers[eventName]
                .Add(isDynamic ? SubscriptionInfo.Dynamic(handlerType) : SubscriptionInfo.Typed(handlerType));
        }

        /// <summary>
        /// DoRemoveHandler
        /// </summary>
        /// <param name="eventName">eventName</param>
        /// <param name="subsToRemove">subsToRemove</param>
        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove == null)
            {
                return;
            }

            this._handlers[eventName].Remove(subsToRemove);

            if (this._handlers[eventName].Any())
            {
                return;
            }

            this._handlers.Remove(eventName);
            var eventType = this._eventTypes.SingleOrDefault(e => e.Name == eventName);
            if (eventType != null)
            {
                this._eventTypes.Remove(eventType);
            }
            this.RaiseOnEventRemoved(eventName);
        }

        /// <summary>
        /// FindSubscriptionToRemove
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <typeparam name="TH">TH</typeparam>
        /// <returns>SubscriptionInfo</returns>
        private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationMessageHandler<T>
        {
            var eventName = this.GetEventKey<T>();
            return this.DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        /// <summary>
        /// DoFindSubscriptionToRemove
        /// </summary>
        /// <param name="eventName">eventName</param>
        /// <param name="handlerType">handlerType</param>
        /// <returns>SubscriptionInfo</returns>
        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            return !this.HasSubscriptionsForEvent(eventName) ? null : this._handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }

        /// <summary>
        /// RaiseOnEventRemoved
        /// </summary>
        /// <param name="eventName">eventName</param>
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = this.OnEventRemoved;
            if (handler != null)
            {
                this.OnEventRemoved?.Invoke(this, eventName);
            }
        }
    }
}
