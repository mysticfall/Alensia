using Zenject;

namespace Alensia.Core.Character.Customize
{
    public class CustomizableHumanoid : Humanoid, ICustomizable
    {
        public override IRace Race => Morphs.Race;

        public override Sex Sex => Morphs.Sex;

        [Inject]
        public IMorphSet Morphs { get; }
    }
}