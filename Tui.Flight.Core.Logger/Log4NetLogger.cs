namespace Tui.Flights.Core.Logger
{
    using System;
    using System.Reflection;
    using System.Xml;
    using log4net;
    using log4net.Config;
    using log4net.Repository;
    using log4net.Repository.Hierarchy;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Log4NetLogger
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;
        private readonly ILoggerRepository _loggerRepository;
        private Func<object, Exception, string> _exceptionDetailsFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogger"/> class.
        /// Log4NetLogger
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="xmlElement">xmlElement</param>
        public Log4NetLogger(string name, XmlElement xmlElement)
        {
            ILoggerRepository loggerRepository2;
            ILoggerRepository loggerRepository1;
            ILoggerRepository loggerRepository;
            this._loggerRepository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(Hierarchy));
            this._log = LogManager.GetLogger(this._loggerRepository.Name, name);
            XmlConfigurator.Configure(this._loggerRepository, xmlElement);
        }

        /// <summary>
        /// BeginScope
        /// </summary>
        /// <typeparam name="TState">TState</typeparam>
        /// <param name="state">state</param>
        /// <returns>IDisposable</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// IsEnabled
        /// </summary>
        /// <param name="logLevel">logLevel</param>
        /// <returns>bool</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return this._log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return this._log.IsDebugEnabled;
                case LogLevel.Error:
                    return this._log.IsErrorEnabled;
                case LogLevel.Information:
                    return this._log.IsInfoEnabled;
                case LogLevel.Warning:
                    return this._log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <typeparam name="TState">TState</typeparam>
        /// <param name="logLevel">logLevel</param>
        /// <param name="eventId">eventId</param>
        /// <param name="state">state</param>
        /// <param name="exception">exception</param>
        /// <param name="formatter">formatter</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = null;
            if (formatter != null)
            {
                message = formatter(state, exception);
            }

            if (exception != null && this._exceptionDetailsFormatter != null)
            {
                message = this._exceptionDetailsFormatter(message, exception);
            }

            if (!string.IsNullOrEmpty(message)
                || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        this._log.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        this._log.Debug(message);
                        break;
                    case LogLevel.Error:
                        this._log.Error(message);
                        break;
                    case LogLevel.Information:
                        this._log.Info(message);
                        break;
                    case LogLevel.Warning:
                        this._log.Warn(message);
                        break;
                    default:
                        this._log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        this._log.Info(message, exception);
                        break;
                }
            }
        }

        /// <summary>
        /// UsingCustomExceptionFormatter
        /// </summary>
        /// <param name="formatter">formatter</param>
        /// <returns>Log4NetLogger</returns>
        public Log4NetLogger UsingCustomExceptionFormatter(Func<object, Exception, string> formatter)
        {
            this._exceptionDetailsFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            return this;
        }
    }
}
