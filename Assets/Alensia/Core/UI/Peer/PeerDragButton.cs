using UnityEngine.EventSystems;
using UEButton = UnityEngine.UI.Button;

namespace Alensia.Core.UI.Peer
{
    public class PeerDragButton : UEButton, IInitializePotentialDragHandler,
        IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }
    }
}