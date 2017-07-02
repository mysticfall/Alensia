using Alensia.Core.Common;
using Alensia.Core.Geom;

namespace Alensia.Core.UI
{
    public interface IUIElement : IUIContextHolder, INamed, IHideable, ITransformable
    {
        void Initialize(IUIContext context);
    }
}