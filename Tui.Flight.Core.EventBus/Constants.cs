namespace Tui.Flights.Core.EventBus
{
    /// <summary>
    /// internal static class QueueContext
    /// </summary>
    public static class QueueContext
    {
        /// <summary>
        ///  TuiQueue
        /// </summary>
        public const string TuiQueue = "TUI.Queue.Direct.TuiApp";

        /// <summary>
        /// TuiExchange
        /// </summary>
        public const string TuiExchange = "TUI.Event.Bus";

        /// <summary>
        /// TuiType must be named : "direct"
        /// </summary>
        public const string TuiType = "direct";
    }
}