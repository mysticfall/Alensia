using Alensia.Core.Common;
using Alensia.Core.Geom;
using UnityEngine;

namespace Alensia.Core.UI
{
    public interface IUIElement : IUIContextHolder, INamed, IHideable, ITransformable
    {
        RectTransform RectTransform { get; }

        void Initialize(IUIContext context);
    }
}