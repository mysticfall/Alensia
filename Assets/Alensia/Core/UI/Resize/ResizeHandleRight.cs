using Alensia.Core.UI.Cursor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Resize
{
    public class ResizeHandleRight : ResizeHandle
    {
        public override string Cursor => CursorNames.ResizeEast;

        protected override Vector2 CalculateSizeDelta(PointerEventData e) => new Vector2(e.delta.x, 0);

        protected override Vector2 CalculateAnchor(Rect rect) =>
            new Vector2(rect.xMin, rect.yMin + (rect.yMax - rect.yMin) * 0.5f);

        protected override void UpdatePosition(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 1);

            rectTransform.offsetMin = new Vector2(-Size, Size);
            rectTransform.offsetMax = new Vector2(0, -Size);
        }
    }
}