using System.Collections.Generic;
using Alensia.Core.Collection;

namespace Alensia.Core.Character
{
    public interface IRaceRepository : IDirectory<Race>
    {
        IEnumerable<Race> Races { get; }
    }
}