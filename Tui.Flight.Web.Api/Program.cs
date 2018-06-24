namespace Tui.Flights.Web.Api
{
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Web Api Launcher
    /// </summary>
    public class Program
    {
        private static string _pathToContentRoot;

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
            var isService = !(args.Contains("--console") || Debugger.IsAttached);

            _pathToContentRoot = Directory.GetCurrentDirectory();
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                _pathToContentRoot = Path.GetDirectoryName(pathToExe);
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

            var host = WebHost.CreateDefaultBuilder(args)
                    .UseKestrel()
                    .UseContentRoot(_pathToContentRoot)
                    .UseUrls(string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}", configuration["ServerName"], configuration["ServerPort"]))
                    .UseStartup<Startup>()
                    .Build();

            return host;
        }
    }
}
