using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Relay.Models;
using Relay.Providers;
using Relay.Services.Discovery;
using Relay.Utils;

[assembly: ApiController]
namespace Relay.Services
{
    public sealed class Server
    {
        private readonly IConfiguration _config;
        
        // Services
        
        private LineupUpdater _lineupUpdater;
        private UdpDiscovery _udpDiscovery;
        
        public Server(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            var cfg = new RelayConfiguration();
            _config.Bind(cfg);

            var dbConnectionString = $"Data Source = {cfg.DatabasePath}/relay.db";
            
            services
                .Configure<RelayConfiguration>(_config)
                .AddDbContext<LineupContext>(options => options.UseSqlite(dbConnectionString))
                .AddSingleton<LineupUpdater>()
                .AddSingleton<UdpDiscovery>();
            

            switch(cfg.Provider)
            {
                case LineupProvider.Tvheadend:
                    services.AddSingleton<ILineupProvider, TvheadendLineupProvider>();
                    break;
            }
        }
        
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                using (var ctx = serviceScope.ServiceProvider.GetRequiredService<LineupContext>())
                {
                    ctx.Database.Migrate();
                }
                
                _lineupUpdater = serviceScope.ServiceProvider.GetRequiredService<LineupUpdater>();
                _udpDiscovery = serviceScope.ServiceProvider.GetRequiredService<UdpDiscovery>();
            }
            
            _lineupUpdater.Start();
            _udpDiscovery.Start();
        }
    }
}