using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Relay.Providers;

[assembly: ApiController]
namespace Relay
{
    public sealed class Server
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Server> _log;
        
        public Server(IConfiguration config, ILogger<Server> log)
        {
            _config = config;
            _log = log;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RelayConfiguration>(_config);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var cfg = new RelayConfiguration();
            _config.Bind(cfg);

            switch(cfg.Provider)
            {
                case LineupProvider.Tvheadend:
                    services.Configure<TvheadendLineupProviderConfig>(_config.GetSection("tvheadend"));
                    services.AddSingleton<ILineupProvider, TvheadendLineupProvider>();
                    break;
            }
        }
        
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}