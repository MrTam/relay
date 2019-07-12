using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Relay.Models;
using Relay.Providers;
using Relay.Utils;

[assembly: ApiController]
namespace Relay
{
    public sealed class Server
    {
        private readonly IConfiguration _config;
        private readonly ILogger _log;
        private LineupUpdater _lineupUpdater;
        
        public Server(IConfiguration config, ILogger<Server> log)
        {
            _config = config;
            _log = log;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RelayConfiguration>(_config);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<LineupContext>(options => options.UseSqlite(Constants.DbDataSource));

            var cfg = new RelayConfiguration();
            _config.Bind(cfg);

            switch(cfg.Provider)
            {
                case LineupProvider.Tvheadend:
                    services.AddSingleton<ILineupProvider, TvheadendLineupProvider>();
                    break;
            }

            services.AddSingleton<LineupUpdater>();
        }
        
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();

            _lineupUpdater = app.ApplicationServices.GetService<LineupUpdater>();
            _lineupUpdater.Start();
        }
    }
}