namespace Tui.Flights.Core.EventBus
{
    using System;

    /// <summary>
    /// SubscriptionInfo
    /// </summary>
    public class SubscriptionInfo
    {
        /// <summary>
        /// Gets a value indicating whether isDynamic
        /// </summary>
        public bool IsDynamic { get; }

        /// <summary>
        /// Gets handlerType
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionInfo"/> class.
        /// SubscriptionInfo
        /// </summary>
        /// <param name="isDynamic">isDynamic</param>
        /// <param name="handlerType">handlerType</param>
        private SubscriptionInfo(bool isDynamic, Type handlerType)
        {
            this.IsDynamic = isDynamic;
            this.HandlerType = handlerType;
        }

        /// <summary>
        /// SubscriptionInfo
        /// </summary>
        /// <param name="handlerType">handlerType</param>
        /// <returns><see cref="SubscriptionInfo"/></returns>
        public static SubscriptionInfo Dynamic(Type handlerType)
        {
            return new SubscriptionInfo(true, handlerType);
        }

        /// <summary>
        /// SubscriptionInfo
        /// </summary>
        /// <param name="handlerType">handlerType</param>
        /// <returns><see cref="SubscriptionInfo"/></returns>
        public static SubscriptionInfo Typed(Type handlerType)
        {
            return new SubscriptionInfo(false, handlerType);
        }
    }
}
