using Alensia.Core.Locomotion;

namespace Alensia.Core.Control
{
    public interface ILocomotionControl<out T> : IControl where T : ILocomotion
    {
        T Locomotion { get; }
    }
}