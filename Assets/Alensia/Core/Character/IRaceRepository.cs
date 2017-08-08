using System.Collections.Generic;
using Alensia.Core.Common;

namespace Alensia.Core.Character
{
    public interface IRaceRepository : IDirectory<Race>
    {
        IReadOnlyList<Race> Races { get; }
    }
}