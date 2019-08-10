using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Relay.Models;
using Relay.Providers;
using Relay.Services.Discovery;

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
            services.AddSpaStaticFiles(config => config.RootPath = "wwwroot/build");
            
            var cfg = new RelayConfiguration();
            _config.Bind(cfg);

            const string dbConnectionString = "Data Source = Config/relay.db";
            
            services
                .Configure<RelayConfiguration>(_config)
                .AddDbContext<RelayDbContext>(options => options.UseSqlite(dbConnectionString))
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
            app
                .UseStaticFiles()
                .UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}"
                );
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot";

                if(env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                using (var ctx = serviceScope.ServiceProvider.GetRequiredService<RelayDbContext>())
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