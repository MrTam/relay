using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Relay
{
    internal static class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Run();

        public static IWebHost CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables("relay_")
                .Build();

            return new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseStartup<Server>()
                .UseUrls("http://0.0.0.0:5004")
                .ConfigureLogging((_, logging) =>
                {
                    logging
                        .ClearProviders()
                        .SetMinimumLevel(LogLevel.Information)
                        .AddConsole(); 
                })
                .Build();
        }
    }
}