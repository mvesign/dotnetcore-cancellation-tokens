using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace SlowService
{
    /// <summary>
    /// Main class for the web API.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point of the web API.
        /// </summary>
        /// <param name="args">List of starting arguments.</param>
        public static void Main(
            string[] args
        )
        {
            CreateWebHost(args).Run();
        }

        /// <summary>
        /// Build the web host.
        /// </summary>
        /// <param name="args">List of starting arguments.</param>
        /// <returns>Details of the web host.</returns>
        public static IWebHost CreateWebHost(
            string[] args
        ) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging
                        .AddConfiguration(context.Configuration.GetSection("Logging"))
                        .AddConsole();
                })
                .UseStartup<Startup>()
                .Build();
    }
}