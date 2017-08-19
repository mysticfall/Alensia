namespace Alensia.Core.UI
{
    public interface IInteractableComponent : IComponent
    {
        bool Interactable { get; set; }
    }
}