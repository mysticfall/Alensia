using System.Collections.Generic;
using Alensia.Core.Common;

namespace Alensia.Core.Character
{
    public interface IRace : IForm
    {
        IEnumerable<Sex> Sexes { get; }
    }
}