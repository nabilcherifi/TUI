namespace Tui.Flights.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Tui.Flights.Core.EventBus;
    using Tui.Flights.Web.Api.RequestModels;
    using Tui.Flights.Web.Infrastructure.DataLayer;
    using Tui.Flights.Web.Infrastructure.DataLayer.Itf;
    using Tui.Flights.Web.Infrastructure.IntegrationEvents;
    using Tui.Flights.Web.Infrastructure.IntegrationEvents.Events;

    /// <summary>
    /// Home controller
    /// </summary>
    [Route("api/v1/[controller]")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// Constructor
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="dataService">dataService</param>
        /// <param name="eventService">eventService</param>
        public HomeController(ILogger<HomeController> logger, ITuiDataServices dataService, ITuiIntegrationEventService eventService)
            : base(dataService, eventService)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Default Get methods (Deployment test)
        /// </summary>
        /// <returns>The Web API project name</returns>
        // GET api/values
        [HttpGet]
        [Route("Get")]
        public IEnumerable<string> Get()
        {
            return new[] { "TUI.Web.Api" };
        }

        /// <summary>
        /// GetFlights
        /// </summary>
        /// <param name="requestedFight">requestedFight</param>
        /// <returns>IEnumerable.TuiNewFlight</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("GetFlights")]
        public IEnumerable<TuiNewFlight> GetFlights([FromBody]FlightRequest requestedFight)
        {
            this._logger?.LogInformation("Entering HomeController GetFlights()");
            var flightRequest = requestedFight ?? throw new ArgumentNullException(nameof(requestedFight));

            var flights = this.DataService.GetFlights(
                flightRequest.DepartureAirport,
                flightRequest.ArrivalAirport,
                flightRequest.FlightStartDate,
                flightRequest.FlightEndDate);

            // Persist requested filghts through event bus
            // Publish Through EventBus for Persistence of Flight Request
            var tuiNewFlights = flights.ToList();
            this.PersistRequestedFlights(tuiNewFlights);

            return tuiNewFlights;
        }

        /// <summary>
        /// FlightReport
        /// </summary>
        /// <param name="flightRequest">flightRequest</param>
        /// <returns>true</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("CampaignReport")]
        public OkObjectResult FlightReport([FromBody]FlightRequest flightRequest)
        {
            _logger?.LogInformation("Entering HomeController FlightReport()");

            if (flightRequest == null)
            {
                throw new ArgumentNullException(nameof(flightRequest));
            }

            var dataflight = DataService.FlightReport(flightRequest.DepartureAirport, flightRequest.ArrivalAirport, flightRequest.FlightStartDate, flightRequest.FlightEndDate);

            // Publish Through RabbitMQ EventBus for Persistence of Flight Request
            GenerateReport(dataflight);

            _logger?.LogInformation("Leaving HomeController FlightReport()");

            return Ok($"Done : {true}");
        }

        // Publish Through RabbitMQ EventBus for Persistence of Flight Request
        private void PersistRequestedFlights(IEnumerable<TuiNewFlight> dataFlight)
        {
            this._logger?.LogInformation("Entering HomeController PersistRequestedFlights()");

            var tuiNewFlights = dataFlight as TuiNewFlight[] ?? dataFlight.ToArray();
            IntegrationEvent flightReportEvent = null;
            foreach (var flight in tuiNewFlights)
            {
                // RabbitMQ event definition
                flightReportEvent = new GenerateFlightsIntegrationEvent(
                    flight.FlightId,
                    flight.FlightPeriod,
                    flight.DepartureAirport,
                    flight.ArrivalAirport);
            }

            // Requested Flight Persistence :
            // RabbitMQ : Publish Through EventBus for Persistence of Flight Request
            this.TuiIntegrationEventService.PublishThroughEventBusAsync(flightReportEvent);

            this._logger?.LogInformation("Leaving HomeController PersistRequestedFlights()");
        }

        // Write fligth Report through event bus
        private void GenerateReport(TuiNewFlight fligth)
        {
            this._logger?.LogInformation("Entering HomeController GenerateReport()");

            var fligthReportEvent = new GenerateReportsIntegrationEvent(
                fligth.FlightId,
                fligth.DepartureAirport,
                fligth.ArrivalAirport,
                fligth.FlightPeriod);

            // Flight report : Publish Through EventBus
            this.TuiIntegrationEventService.PublishThroughEventBusAsync(fligthReportEvent);

            this._logger?.LogInformation("Leaving HomeController GenerateReport()");
        }
    }
}
