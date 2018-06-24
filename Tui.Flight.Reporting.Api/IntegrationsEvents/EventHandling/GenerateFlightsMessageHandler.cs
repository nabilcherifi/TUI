namespace Tui.Flights.Reporting.Api.IntegrationsEvents.EventHandling
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Services.Protocols;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
    using Tui.Flights.Core.EventBus;
    using Tui.Flights.Reporting.Api.IntegrationsEvents.Events;

    /// <summary>
    /// GenerateFlightsMessageHandler
    /// </summary>
    public class GenerateFlightsMessageHandler : IIntegrationMessageHandler<GenerateFlightsIntegrationEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GenerateFlightsMessageHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateFlightsMessageHandler"/> class.
        /// GenerateFlightsMessageHandler
        /// </summary>
        /// <param name="configuration">configuration</param>
        /// <param name="logger">logger</param>
        public GenerateFlightsMessageHandler(IConfiguration configuration, ILogger<GenerateFlightsMessageHandler> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        /// <summary>
        /// Handle flight persistence
        /// </summary>
        /// <param name="flightEvent">flightEvent</param>
        /// <returns>Task</returns>
        public async Task Handle(GenerateFlightsIntegrationEvent flightEvent)
        {
            if (flightEvent == null)
            {
                throw new ArgumentNullException(nameof(flightEvent));
            }

            this._logger?.LogInformation($"GenerateFlightsIntegrationMessageHandler Handle {flightEvent.Id}");
            await this.RunFlightTasks(flightEvent).ConfigureAwait(false);
        }

        /// <summary>
        /// Execute reports
        /// </summary>
        /// <param name="flightEvent">flightEvent</param>
        /// <returns>Task result</returns>
        private Task RunFlightTasks(GenerateFlightsIntegrationEvent flightEvent)
        {
            var serialNumbers = flightEvent.SerialNumbers;

            Task[] taskArray = new Task[serialNumbers.Length];
            for (int i = 0; i < serialNumbers.Length; i++)
            {
                var state = new TuiDataTask
                {
                    SerialNumber = serialNumbers[i],
                    Period = flightEvent.FlightPeriod,
                    FlightName = flightEvent.FlightName
                };

                taskArray[i] = Task.Factory.StartNew(
                    async obj =>
                    {
                        var data = obj as TuiDataTask;
                        if (data == null)
                        {
                            return;
                        }

                        await this.GenerateTuiReport(data.SerialNumber, data.FlightName, data.Period).ConfigureAwait(false);
                    },
                    state);
                Thread.Sleep(1000);
            }
            Task.WaitAll(taskArray);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Generate TUI Reports
        /// </summary>
        /// <param name="serialNumber">Serial Number</param>
        /// <param name="flightName">flightName</param>
        /// <param name="period">Period</param>
        /// <returns>Task result </returns>
        private Task GenerateTuiReport(string serialNumber, string flightName, string period)
        {
            var pdfDirectory = this._configuration["SSRSInfo:PDFDirectory"] ?? throw new ArgumentNullException($"_configuration[\"SSRSInfo:PDFDirectory\"]");
            var reportServerUrl = this._configuration["SSRSInfo:ServerUrl"] ?? throw new ArgumentNullException($"_configuration[\"SSRSInfo:ServerUrl\"]");
            var userSsrs = this._configuration["SSRSInfo:SSRSUser"] ?? throw new ArgumentNullException($"_configuration[\"SSRSInfo:SSRSUser\"]");
            var pwdSsrs = this._configuration["SSRSInfo:SSRSPwd"] ?? throw new ArgumentNullException($"_configuration[\"SSRSInfo:SSRSPwd\"]");
            var domainSsrs = this._configuration["SSRSInfo:SSRSDomain"] ?? throw new ArgumentNullException($"_configuration[\"SSRSInfo:SSRSDomain\"]");
            var reportPath = this._configuration["SSRSInfo:ReportPath"] ?? throw new ArgumentNullException($"_configuration[\"SSRSInfo:ReportPath\"]");

            using (var rs = new ReportExecutionService())
            {
                try
                {
                    rs.Timeout = Timeout.Infinite;
                    rs.Credentials = new NetworkCredential(userSsrs, pwdSsrs, domainSsrs);
                    rs.Url = $"{reportServerUrl}/ReportExecution2005.asmx";

                    ParameterValue[] parameters = new ParameterValue[3];
                    parameters[0] = new ParameterValue
                    {
                        Name = "SerialNumber",
                        Value = serialNumber
                    };
                    parameters[1] = new ParameterValue
                    {
                        Name = "FlightName",
                        Value = flightName
                    };
                    parameters[2] = new ParameterValue
                    {
                        Name = "Period",
                        Value = period
                    };

                    rs.LoadReport(reportPath, null);
                    rs.SetExecutionParameters(parameters, CultureInfo.CurrentCulture.ToString());

                    string devInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";
                    var result = rs.Render("pdf", devInfo, Extension: out _, MimeType: out _, Encoding: out _, Warnings: out _, StreamIds: out _);

                    rs.GetExecutionInfo();

                    File.WriteAllBytes(
                        $"{pdfDirectory}\\{serialNumber}_{period}_{DateTime.Now.ToString("ddMMyyyyHHmmss", CultureInfo.InvariantCulture)}.pdf",
                        result);
                    this._logger?.LogDebug(
                        $"GenerateTUIReport {Thread.CurrentThread.ManagedThreadId} :  GeneratedPdf {serialNumber}_{period}");

                    rs.Dispose();
                }
                catch (SoapException se)
                {
                    this._logger?.LogError(se, "ReportExecutionTask se: ");
                    this._logger?.LogError(se, "SerialNumber is: " + serialNumber);
                }
                catch (WebException we)
                {
                    Debug.WriteLine(we);
                    this._logger?.LogError(we, "ReportExecutionTask we: ");
                    throw;
                }
                catch (UnauthorizedAccessException e)
                {
                    this._logger?.LogError(e, "ReportExecutionTask e : ");
                    throw;
                }
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// TUI data task
        /// </summary>
        internal class TuiDataTask
        {
            /// <summary>
            /// Gets or sets instrument serial number
            /// </summary>
            public string SerialNumber { get; set; }

            /// <summary>
            /// Gets or sets period
            /// </summary>
            public string Period { get; set; }

            /// <summary>
            /// Gets or sets flightName
            /// </summary>
            public string FlightName { get; set; }
        }
    }
}
