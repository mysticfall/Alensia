namespace Alensia.Core.Character.Morph
{
    public interface IMorphable
    {
        IMorphSet Morphs { get; }

        IBodyPartContainer BodyParts { get; }
    }
}