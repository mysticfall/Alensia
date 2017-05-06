using System.Collections.Generic;

namespace Alensia.Core.Input
{
    public interface IModifierInput : IInput
    {
        IList<ITrigger> Modifiers { get; }
    }
}