using Alensia.Core.Locomotion;

namespace Alensia.Core.Actor
{
    public interface ILocomotiveActor<out T> : IActor where T : ILocomotion
    {
        T Locomotion { get; }
    }
}