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
        Task<IList<LineupEntry>> Entries { get; }
    }
}