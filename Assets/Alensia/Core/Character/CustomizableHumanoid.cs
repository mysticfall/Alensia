using Alensia.Core.Character.Morph;
using Alensia.Core.Item;
using Zenject;

namespace Alensia.Core.Character
{
    public class CustomizableHumanoid : Humanoid, IMorphable, IClothed
    {
        public override IRace Race => Morphs.Race;

        public override Sex Sex => Morphs.Sex;

        [Inject]
        public IMorphSet Morphs { get; }

        [Inject]
        public IBodyPartContainer BodyParts { get; }

        [Inject]
        public IClothingContainer Clothings { get; }
    }
}