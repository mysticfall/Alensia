using Alensia.Core.Control;

namespace Alensia.Core.Locomotion
{
    public interface ILocomotionControl<out T> : IControl where T : ILocomotion
    {
        T Locomotion { get; }
    }
}