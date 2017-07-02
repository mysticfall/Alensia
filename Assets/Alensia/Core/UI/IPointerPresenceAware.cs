using UniRx;
using UnityEngine.EventSystems;

namespace Alensia.Core.UI
{
    public interface IPointerPresenceAware
    {
        IObservable<PointerEventData> OnPointerEnter { get; }

        IObservable<PointerEventData> OnPointerExit { get; }
    }
}