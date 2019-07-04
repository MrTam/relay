using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Relay
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Server>()
                .UseUrls("http://0.0.0.0:5004")
                .Build()
                .Run();
        }
    }
}
