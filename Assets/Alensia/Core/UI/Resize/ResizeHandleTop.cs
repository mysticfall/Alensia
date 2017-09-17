using Alensia.Core.UI.Cursor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Resize
{
    public class ResizeHandleTop : ResizeHandle
    {
        public override string Cursor => CursorNames.ResizeNorth;

        protected override Vector2 CalculateSizeDelta(PointerEventData e) => new Vector2(0, e.delta.y);

        protected override Vector2 CalculateAnchor(Rect rect) =>
            new Vector2(rect.xMin + (rect.xMax - rect.xMin) * 0.5f, rect.yMin);

        protected override void UpdatePosition(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);

            rectTransform.offsetMin = new Vector2(Size, -Size);
            rectTransform.offsetMax = new Vector2(-Size, 0);
        }
    }
}