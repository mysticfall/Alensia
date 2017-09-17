using Alensia.Core.UI.Cursor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Resize
{
    public class ResizeHandleTopRight : ResizeHandle
    {
        public override string Cursor => CursorNames.ResizeNorthEast;

        protected override Vector2 CalculateSizeDelta(PointerEventData e) => e.delta;

        protected override Vector2 CalculateAnchor(Rect rect) => rect.position;

        protected override void UpdatePosition(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.sizeDelta = new Vector2(Size, Size);

            rectTransform.anchoredPosition = new Vector2(Size * -0.5f, Size * -0.5f);
        }
    }
}