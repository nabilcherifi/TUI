using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace Tui.Flights.Core.Logger
{
    /// <summary>
    /// Log4NetProvider
    /// </summary>
    public class Log4NetProvider : ILoggerProvider
    {
        /// <summary>
        /// ConcurrentDictionary
        /// </summary>
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        /// <summary>
        /// log4NetConfigFile
        /// </summary>
        private readonly string _log4NetConfigFile;

        /// <summary>
        /// exceptionFormatter
        /// </summary>
        private readonly Func<object, Exception, string> _exceptionFormatter;

        /// <summary>
        /// FormatExceptionByDefault
        /// </summary>
        /// <typeparam name="TState">TState</typeparam>
        /// <param name="state">state</param>
        /// <param name="exception">exception</param>
        /// <returns>string</returns>
        private static string FormatExceptionByDefault<TState>(TState state, Exception exception)
        {
            var builder = new StringBuilder();
            builder.Append(state.ToString());
            builder.Append(" - ");
            if (exception != null)
            {
                builder.Append(exception.ToString());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetProvider"/> class.
        /// Log4NetProvider
        /// </summary>
        /// <param name="log4NetConfigFile">log4NetConfigFile</param>
        /// <param name="exceptionFormatter">exceptionFormatter</param>
        public Log4NetProvider(string log4NetConfigFile, Func<object, Exception, string> exceptionFormatter)
        {
            this._log4NetConfigFile = log4NetConfigFile;
            this._exceptionFormatter = exceptionFormatter ?? FormatExceptionByDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetProvider"/> class.
        /// Log4NetProvider
        /// </summary>
        /// <param name="log4NetConfigFile">log4NetConfigFile</param>
        public Log4NetProvider(string log4NetConfigFile)
            : this(log4NetConfigFile, null)
        {
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <param name="categoryName">categoryName</param>
        /// <returns>Log4NetLogger</returns>
        public Log4NetLogger CreateLogger(string categoryName)
        {
            var logger = this.CreateLoggerImplementation(categoryName, this._exceptionFormatter);
            var log4NetLoggers = this._loggers;
            return log4NetLoggers?.GetOrAdd(categoryName, logger);
        }

        private bool _disposed;

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this.Dispose();
                }
            }
            this._disposed = true;
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
        /// Parselog4NetConfigFile
        /// </summary>
        /// <param name="filename">filename</param>
        /// <returns>XmlElement</returns>
        private static XmlElement Parselog4NetConfigFile(string filename)
        {
            using (FileStream fp = File.OpenRead(filename))
            {
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Prohibit
                };

                var log4NetConfig = new XmlDocument();
                using (var reader = XmlReader.Create(fp, settings))
                {
                    log4NetConfig.Load(reader);
                }

                fp.Flush();
                fp.Dispose();

                return log4NetConfig["log4net"];
            }
        }

        /// <summary>
        /// Log4NetLogger
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="exceptionFormatter">exceptionFormatter</param>
        /// <returns><see cref="Log4NetLogger"/></returns>
        private Log4NetLogger CreateLoggerImplementation(string name, Func<object, Exception, string> exceptionFormatter)
        {
            return new Log4NetLogger(name, Parselog4NetConfigFile(this._log4NetConfigFile)).UsingCustomExceptionFormatter(exceptionFormatter);
        }

        /// <summary>
        /// ILoggerProvider.CreateLogger
        /// </summary>
        /// <param name="categoryName">categoryName</param>
        /// <returns>ILogger</returns>
        ILogger ILoggerProvider.CreateLogger(string categoryName)
        {
            var logger = this.CreateLoggerImplementation(categoryName, this._exceptionFormatter);
            return this._loggers.GetOrAdd(categoryName, logger);
        }
    }
}
