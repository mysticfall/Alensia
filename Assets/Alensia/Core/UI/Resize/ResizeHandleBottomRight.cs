using Alensia.Core.UI.Cursor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Resize
{
    public class ResizeHandleBottomRight : ResizeHandle
    {
        public override string Cursor => CursorNames.ResizeSouthEast;

        protected override Vector2 CalculateSizeDelta(PointerEventData e) => new Vector2(e.delta.x, -e.delta.y);

        protected override Vector2 CalculateAnchor(Rect rect) => new Vector2(rect.xMin, rect.yMax);

        protected override void UpdatePosition(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
            rectTransform.sizeDelta = new Vector2(Size, Size);

            rectTransform.anchoredPosition = new Vector2(Size * -0.5f, Size * 0.5f);
        }
    }
}