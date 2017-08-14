namespace Alensia.Core.UI
{
    public interface IControl : IComponent
    {
        bool Interactable { get; set; }
    }
}