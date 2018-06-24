namespace Tui.Flights.ReportManager.Api.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Home controller
    /// </summary>
    [Route("api/v1/[controller]")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Default Get methods (Deployment test)
        /// </summary>
        /// <returns>The Web API project name</returns>
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "TUIProto2.Report.Api" };
        }
    }
}