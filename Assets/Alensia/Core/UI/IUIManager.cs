using Alensia.Core.UI.Cursor;
using Alensia.Core.UI.Screen;

namespace Alensia.Core.UI
{
    public interface IUIManager : IUIContextHolder, IScreenManager, ICursorManager
    {
    }
}