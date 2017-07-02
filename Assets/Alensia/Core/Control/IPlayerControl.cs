using Alensia.Core.Actor;

namespace Alensia.Core.Control
{
    public interface IPlayerControl : IControl
    {
        IHumanoid Player { get; set; }
    }
}