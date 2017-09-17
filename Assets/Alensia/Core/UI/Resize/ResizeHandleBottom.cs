using Alensia.Core.UI.Cursor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Resize
{
    public class ResizeHandleBottom : ResizeHandle
    {
        public override string Cursor => CursorNames.ResizeSouth;

        protected override Vector2 CalculateSizeDelta(PointerEventData e) => new Vector2(0, -e.delta.y);

        protected override Vector2 CalculateAnchor(Rect rect) =>
            new Vector2(rect.xMin + (rect.xMax - rect.xMin) * 0.5f, rect.yMax);

        protected override void UpdatePosition(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 0);

            rectTransform.offsetMin = new Vector2(Size, 0);
            rectTransform.offsetMax = new Vector2(-Size, Size);
        }
    }
}