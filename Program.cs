using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var ports = new List<int>{5004};

            // Don't enable port 80 in dev mode, requires root on unix

            var devMode = Environment
                .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                .Equals(EnvironmentName.Development);

            if(!devMode) ports.Add(80);

            return new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseStartup<Server>()
                .UseUrls(ports.Select(p => $"http://0.0.0.0:{p}").ToArray())
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