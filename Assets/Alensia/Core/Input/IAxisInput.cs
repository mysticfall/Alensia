using Alensia.Core.Input.Generic;

namespace Alensia.Core.Input
{
    public interface IAxisInput : IInput<float>, IModifierInput
    {
        string Axis { get; }
    }
}