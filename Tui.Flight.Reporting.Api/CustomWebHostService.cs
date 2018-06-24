namespace Tui.Flights.Persistence.Api
{
    using System.ServiceProcess;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.WindowsServices;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class permit to define extensions
    /// </summary>
    public static class WebHostServiceExtensions
    {
        /// <summary>
        /// Method permit to run microservice
        /// </summary>
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new CustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }

    /// <summary>
    /// Class permit to host my microservice as service windows
    /// </summary>
    public class CustomWebHostService : WebHostService
    {
        private ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWebHostService"/> class.
        /// Ctor
        /// </summary>
        public CustomWebHostService(IWebHost host)
            : base(host)
        {
            this._logger = host.Services.GetRequiredService<ILogger<CustomWebHostService>>();
        }
    }
}
