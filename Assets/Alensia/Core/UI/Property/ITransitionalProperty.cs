namespace Alensia.Core.UI.Property
{
    public interface ITransitionalProperty<out T>
    {
        T Normal { get; }

        T Highlighted { get; }

        T Disabled { get; }

        T Active { get; }

        T ValueFor(IInteractableComponent component);
    }
}