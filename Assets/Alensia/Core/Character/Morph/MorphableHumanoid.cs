using Zenject;

namespace Alensia.Core.Character.Morph
{
    public class MorphableHumanoid : Humanoid, IMorphable
    {
        [Inject]
        public IMorphSet Morphs { get; }

        public override Race Race => Morphs.Race;

        public override Sex Sex => Morphs.Sex;
    }
}