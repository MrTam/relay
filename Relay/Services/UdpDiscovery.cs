using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Relay.Models;
using Relay.Utils;

namespace Relay.Services
{
    public class UdpDiscovery
    {
        private readonly ILogger _log;
        private readonly RelayConfiguration _config;
        private readonly UdpClient _client;

        private bool _running;

        public UdpDiscovery(
            ILogger<UdpDiscovery> log,
            IOptionsSnapshot<RelayConfiguration> config)
        {
            _log = log;
            _config = config.Value;
            _client = new UdpClient(Constants.UdpDiscoveryPort);
        }

        public void Start()
        {
            _log.LogInformation("Starting UDP discovery protocol on {0}", _client.Client.LocalEndPoint);
            Task.Run(Run);
        }

        public void Stop()
        {
            _running = false;
            _client.Close();
        }

        private async void Run()
        {
            _running = true;
            
            while (_running)
            {
                try
                {
                   var data = await _client.ReceiveAsync();
                   await HandleDatagram(data.RemoteEndPoint, data.Buffer);
                   
                }
                catch (SocketException e)
                {
                    _log.LogError("Error with socket: {1}", e.Message);
                    _running = false;
                }
            }
        }

        private async Task HandleDatagram(IPEndPoint source, byte[] data)
        {
            _log.LogInformation("Received {0} bytes from: {1}", source, data.Length);
        }
    }
}