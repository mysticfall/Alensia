using Alensia.Core.Character.Morph;
using Zenject;

namespace Alensia.Core.Character
{
    public class CustomizableHumanoid : Humanoid, IMorphable
    {
        public override IRace Race => Morphs.Race;

        public override Sex Sex => Morphs.Sex;

        [Inject]
        public IMorphSet Morphs { get; }
    }
}