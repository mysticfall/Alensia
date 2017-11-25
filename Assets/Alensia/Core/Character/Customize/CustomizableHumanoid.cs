using Zenject;

namespace Alensia.Core.Character.Customize
{
    public class CustomizableHumanoid : Humanoid, ICustomizable
    {
        [Inject]
        public IMorphSet Morphs { get; }

        public override Race Race => Morphs.Race;

        public override Sex Sex => Morphs.Sex;
    }
}