using Alensia.Core.Character.Morph;
using Zenject;

namespace Alensia.Integrations.UMA
{
    public class UMABodyPartContainer : BodyPartContainer<UMABodyPart, UMABodyPartList>, IUMAMorphSetContainer
    {
        [Inject]
        public UMAMorphSet MorphSet { get; }

        protected override IMorphableRace Race => MorphSet.Race;

        protected override void AddItem(UMABodyPart item) => this.AddRecipeItem(item);

        protected override void RemoveItem(UMABodyPart item) => this.RemoveRecipeItem(item);
    }
}