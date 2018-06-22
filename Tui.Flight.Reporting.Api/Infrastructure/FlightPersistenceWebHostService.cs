using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tui.Flights.Reporting.Api;

namespace Tui.Flights.Persistence.Api.Infrastructure
{
    /// <summary>
    /// TuiReportWebHostService
    /// </summary>
    public class TuiPersistenceWebHostService : WebHostService
    {
        private readonly System.Diagnostics.EventLog _eventLog;
        private readonly IWebHost _host;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TuiPersistenceWebHostService"/> class.
        /// </summary>
        /// <param name="host">Web Host</param>
        public TuiPersistenceWebHostService(IWebHost host)
            : base(host)
        {
            this._host = host;
            this.ServiceName = PersistenceContext.ServiceName;
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this._eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(PersistenceContext.ServiceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(PersistenceContext.ServiceName, PersistenceContext.LogRegister);
            }

            // Configure the event log
            this._eventLog.Source = PersistenceContext.ServiceName;
            this._eventLog.Log = PersistenceContext.LogRegister;
            this._logger = host.Services.GetRequiredService<ILogger<CustomWebHostService>>();
        }

        /// <summary>
        /// OnStarting override
        /// </summary>
        /// <param name="args">Args</param>
        protected override void OnStarting(string[] args)
        {
            base.OnStarting(args);
            this._eventLog.WriteEntry("OnStarting");
            this._logger?.LogInformation("OnStarting");
        }

        /// <summary>
        /// Method permit to start
        /// </summary>
        protected override void OnStarted()
        {
            base.OnStarted();

            this._eventLog.WriteEntry("OnStarted");
            this._logger?.LogInformation("OnStarted");
        }

        /// <summary>
        /// OnStopping
        /// </summary>
        protected override void OnStopping()
        {
            this._logger?.LogInformation("OnStopping");
            base.OnStopping();
            this._host?.Dispose();
        }
    }
}