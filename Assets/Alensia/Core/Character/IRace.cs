using System.Collections.Generic;
using Alensia.Core.Common;

namespace Alensia.Core.Character
{
    public interface IRace : ILabelled
    {
        IEnumerable<Sex> Sexes { get; }
    }
}