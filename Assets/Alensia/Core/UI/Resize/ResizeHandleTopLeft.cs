using Alensia.Core.UI.Cursor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Resize
{
    public class ResizeHandleTopLeft : ResizeHandle
    {
        public override string Cursor => CursorNames.ResizeNorthWest;

        protected override Vector2 CalculateSizeDelta(PointerEventData e) => new Vector2(-e.delta.x, e.delta.y);

        protected override Vector2 CalculateAnchor(Rect rect) => new Vector2(rect.xMax, rect.yMin);

        protected override void UpdatePosition(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.sizeDelta = new Vector2(Size, Size);

            rectTransform.anchoredPosition = new Vector2(Size * 0.5f, Size * -0.5f);
        }
    }
}