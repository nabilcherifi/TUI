using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tui.Flights.ReportManager.Api
{
    /// <summary>
    /// Web Api Launcher
    /// </summary>
    public class Program
    {
        private static string pathToContentRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// Constructor
        /// </summary>
        private Program()
        {
        }

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">Argument lists</param>
        public static void Main(string[] args)
        {
            bool isService = true;
            if (args.Contains("--console") || Debugger.IsAttached)
            {
                isService = false;
            }

            pathToContentRoot = Directory.GetCurrentDirectory();
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }

            if (isService)
            {
                BuildWebHost(args).RunAsCustomService();
            }
            else
            {
                BuildWebHost(args).Run();
            }
        }

        /// <summary>
        /// doc
        /// </summary>
        /// <param name="args">permit to define arguments</param>
        /// <returns>permit to define result</returns>
        public static IWebHost BuildWebHost(string[] args)
        {
            var webHostBuilder = new WebHostBuilder();
            var env = webHostBuilder.GetSetting("environment");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            IWebHost host = WebHost.CreateDefaultBuilder(args)
                    .UseKestrel()
                    .UseContentRoot(pathToContentRoot)
                    .UseUrls(string.Format(System.Globalization.CultureInfo.InvariantCulture, "http://{0}:{1}", configuration["ServerName"], configuration["ServerPort"]))
                    .UseStartup<Startup>()
                    .Build();

            return host;
        }
    }
}
