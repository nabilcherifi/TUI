using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Tui.Flights.ReportManager.Api.Controllers
{
    /// <summary>
    /// Home controller
    /// </summary>
    [Route("api/v1/[controller]")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// Constructor
        /// </summary>
        public HomeController()
        {
        }

        /// <summary>
        /// Default Get methods (Deployment test)
        /// </summary>
        /// <returns>The Web API project name</returns>
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "TUIProto2.Report.Api" };
        }
    }
}