using UniRx;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI.Event
{
    public interface IPointerDragAware
    {
        IObservable<PointerEventData> OnDragBegin { get; }

        IObservable<PointerEventData> OnDrag { get; }

        IObservable<PointerEventData> OnDragEnd { get; }
    }
}