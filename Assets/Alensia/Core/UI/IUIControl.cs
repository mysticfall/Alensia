using Alensia.Core.Control;

namespace Alensia.Core.UI
{
    public interface IUIControl : IControl
    {
        IUIManager UIManager { get; }
    }
}