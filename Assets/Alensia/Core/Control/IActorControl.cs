using Alensia.Core.Actor;

namespace Alensia.Core.Control
{
    public interface IActorControl<out T> where T : IActor
    {
        T Actor { get; }
    }
}