using Alensia.Core.Character;

namespace Alensia.Core.Control
{
    public interface IPlayerControl : IControl
    {
        IHumanoid Player { get; set; }
    }
}