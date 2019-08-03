using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Relay.Services;

namespace Relay
{
    internal static class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        // ReSharper disable once MemberCanBePrivate.Global

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");
            
            if (!File.Exists("Config/Relay.json"))
                File.Copy("Relay.json", "Config/Relay.json", false);
            
            var config = new ConfigurationBuilder()
                .AddJsonFile("Config/Relay.json", true)
                .AddEnvironmentVariables("relay_")
                .Build();

            return new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseStartup<Server>()
                .UseUrls("http://0.0.0.0:80", "http://0.0.0.0:5004")
                .ConfigureLogging((_, logging) =>
                {
                    logging
                        .ClearProviders()
                        .SetMinimumLevel(LogLevel.Warning)
                        .AddConsole()
                        .AddFilter("Relay", LogLevel.Information)
                        .AddFilter("Microsoft.AspNetCore.Hosting", LogLevel.Information);
                });
        }
    }
}