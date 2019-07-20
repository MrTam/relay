using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Relay.Models;

namespace Relay.Providers
{
    public enum LineupProvider
    {
        Tvheadend
    }

    public interface ILineupProvider
    {
        LineupProvider ProviderType { get; }
        
        Uri InfoUri { get; }

        Task<IList<LineupEntry>> UpdateLineup();
    }
}