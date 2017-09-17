using Alensia.Core.UI.Cursor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Resize
{
    public class ResizeHandleBottomLeft : ResizeHandle
    {
        public override string Cursor => CursorNames.ResizeSouthWest;

        protected override Vector2 CalculateSizeDelta(PointerEventData e) => e.delta * -1;

        protected override Vector2 CalculateAnchor(Rect rect) => new Vector2(rect.xMax, rect.yMax);

        protected override void UpdatePosition(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(Size, Size);

            rectTransform.anchoredPosition = new Vector2(Size * 0.5f, Size * 0.5f);
        }
    }
}