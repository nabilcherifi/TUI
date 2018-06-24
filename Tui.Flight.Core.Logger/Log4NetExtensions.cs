namespace Tui.Flights.Core.Logger
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Log4NetExtensions
    /// </summary>
    public static class Log4NetExtensions
    {
        /// <summary>
        /// DefaultLog4NetConfigFile
        /// </summary>
        private const string DefaultLog4NetConfigFile = "log4net.config";

        /// <summary>
        /// AddLog4Net
        /// </summary>
        /// <param name="factory">factory</param>
        /// <param name="log4NetConfigFile">log4NetConfigFile</param>
        /// <param name="exceptionFormatter">exceptionFormatter</param>
        /// <returns>ILoggerFactory</returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfigFile, Func<object, Exception, string> exceptionFormatter)
        {
            Log4NetProvider provider = new Log4NetProvider(log4NetConfigFile, exceptionFormatter);
            factory.AddProvider(provider);
            return factory;
        }

        /// <summary>
        /// AddLog4Net
        /// </summary>
        /// <param name="factory">factory</param>
        /// <param name="log4NetConfigFile">log4NetConfigFile</param>
        /// <returns>ILoggerFactory</returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfigFile)
        {
            factory.AddProvider(new Log4NetProvider(log4NetConfigFile, null));
            return factory;
        }

        /// <summary>
        /// AddLog4Net
        /// </summary>
        /// <param name="factory">factory</param>
        /// <param name="exceptionFormatter">exceptionFormatter</param>
        /// <returns>ILoggerFactory</returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, Func<object, Exception, string> exceptionFormatter)
        {
            factory.AddLog4Net(DefaultLog4NetConfigFile, exceptionFormatter);
            return factory;
        }

        /// <summary>
        /// AddLog4Net
        /// </summary>
        /// <param name="factory">factory</param>
        /// <returns>ILoggerFactory</returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            factory.AddLog4Net(DefaultLog4NetConfigFile, null);
            return factory;
        }
    }
}
