using System.Collections.Generic;
using Alensia.Core.Common;

namespace Alensia.Core.Character
{
    public interface IRaceRepository : IDirectory<Race>
    {
        IEnumerable<Race> Races { get; }
    }
}